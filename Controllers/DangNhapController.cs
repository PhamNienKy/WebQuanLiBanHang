using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WebsiteQuanLyBanHang.Models;

namespace WebsiteQuanLyBanHang.Controllers
{
    public class DangNhapController : Controller
    {
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {

            //var taikhoan = f["TaiKhoan"].ToString();
            //var matkhau = f["MatKhau"].ToString(); ;
            //var data = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan.Equals(taikhoan) && n.MatKhau.Equals(matkhau));
            //if (data != null)
            //{
            //    //add session
            //    Session["TaiKhoan"] = data;
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    ViewBag.error = "Đăng nhập thất bại ";
            //    return RedirectToAction("Index", "DangNhap");
            //}
            string taikhoan = f["TaiKhoan"].ToString();
            string matkhau = f["MatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan.Equals(taikhoan) && n.MatKhau.Equals(matkhau));
            if (tv != null)
            {
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                string quyen = "";
                
                    foreach (var item in lstQuyen)
                    {
                        quyen += item.Quyen.MaQuyen + ",";
                    }
                if (quyen != "")
                {
                    quyen = quyen.Substring(0, quyen.Length - 1);
                    PhanQuyen(tv.TaiKhoan, quyen);
                }
                if (tv.MaLoaiTV == 1) {
                    return RedirectToAction("Index", "QuanLySanPham");
                }
                else { 
                Session["TaiKhoan"] = tv;
                
                return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.error = "Tài khoản hoặc mật khẩu đăng nhập không đúng!";
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public void PhanQuyen(string TaiKhoan,string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, TaiKhoan, DateTime.Now, DateTime.Now.AddHours(3), false, Quyen, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;

            }
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] =null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}