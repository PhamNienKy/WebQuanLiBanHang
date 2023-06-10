using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebsiteQuanLyBanHang.Controllers
{
    [Authorize(Roles ="Quantri")]
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();
        public ActionResult ChuaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            var lstDSDHCG = db.DonDatHangs.Where(n => (n.TinhTrangGiao == false) && (n.DaThanhToan == true)).OrderBy(n => n.NgayDat);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lstDSDH = db.DonDatHangs.Where(n => n.TinhTrangGiao == true && n.DaThanhToan == true).OrderBy(n => n.NgayDat);
            return View(lstDSDH);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.lstChiTietDonHang = lstChiTietDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang DDH)
        {
            DonDatHang ddhUpDate = db.DonDatHangs.Single(n => n.MaDDH == DDH.MaDDH);
            ddhUpDate.DaThanhToan = DDH.DaThanhToan;
            ddhUpDate.TinhTrangGiao = DDH.TinhTrangGiao;

            string noidung;
            if (ddhUpDate.DaThanhToan == true)
            {
                if(ddhUpDate.TinhTrangGiao == true)
                {
                    //noidung = "Đơn hàng của bạn đã được giao thành công. Cảm ơn đã tin tưởng chúng tôi";
                    //GuiMail("THÔNG BÁO HOÀN TẤT ĐƠN HÀNG", ddhUpDate.KhachHang.Email, "lexuanluongllhh@gmail.com", "xuanluong21022002", noidung);
                    ddhUpDate.NgayGiao = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("DaGiaoDaThanhToan");

                }
                else
                {
                    //noidung = "Đơn hàng của bạn đã được đặt thành công";
                    //GuiMail("XÁC NHẬN ĐƠN HÀNG",ddhUpDate.KhachHang.Email,"lexuanluongllhh@gmail.com","xuanluong21022002",noidung);
                    return RedirectToAction("ChuaGiao");
                }
            }
            db.SaveChanges();
            return RedirectToAction("ChuaThanhToan");
        }
        public void GuiMail(string Title, string toMail,string fromMail,string Password, string Content) 
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(toMail);
            mail.From = new MailAddress(fromMail);
            mail.Subject = Title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(fromMail, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }   

}