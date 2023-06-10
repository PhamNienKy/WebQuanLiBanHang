using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebsiteQuanLyBanHang.Controllers
{
    [Authorize(Roles = "Quantri")]
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();
            ViewBag.TongDonHang = ThongKeDonHang();
            ViewBag.TongThanhVien = ThongKeDThanhVien();
            return View();
        }
        [HttpPost]
        public ActionResult Index(int thang,int nam)
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();
            ViewBag.TongDonHang = ThongKeDonHang();
            ViewBag.TongThanhVien = ThongKeDThanhVien();
            IEnumerable<DonDatHang> lstDDH;
            if (thang != 0)
            {
                lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
                ViewBag.TongThongKe = ThongKeDoanhThuThang(thang,nam);
                ViewBag.lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
                return View(db.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam));
            }
            else
            {
                ViewBag.TongThongKe = ThongKeDoanhThuNam( nam);
                lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Year == nam);
                ViewBag.lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Year == nam);
                return View(db.DonDatHangs.Where(n => n.NgayDat.Value.Year == nam));
            }
            
        }
        public decimal ThongKeTongDoanhThu()
        {
            decimal tongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return tongDoanhThu/1000;
        } 
        public double ThongKeDonHang()
        {
            
            var slddh = db.DonDatHangs.Count();
            return slddh;
        } 
        public double ThongKeDThanhVien()
        {
            
            var slTV = db.ThanhViens.Count();
            return slTV;
        }
        public decimal ThongKeDoanhThuThang(int thang,int nam)
        {
            var lstDDH = db.DonDatHangs.Where(n=>n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
            decimal tongDoanhThu = 0;
            foreach (var item in lstDDH) 
            {
                tongDoanhThu +=decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return tongDoanhThu;
        }
        public decimal ThongKeDoanhThuNam(int nam)
        {
            var lstDDH = db.DonDatHangs.Where(n=> n.NgayDat.Value.Year == nam);
            decimal tongDoanhThu = 0;
            foreach (var item in lstDDH) 
            {
                tongDoanhThu +=decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return tongDoanhThu;
        }
        public ActionResult TongDoanhThu()
        {
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();

            return View(db.DonDatHangs);
        }
        public ActionResult ThanhVien()
        {
            var lstTV = db.ThanhViens.Where(n => n.MaLoaiTV == 5 || n.MaLoaiTV == 6);
            return View(lstTV);
        }
    }
}