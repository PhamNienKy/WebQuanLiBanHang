using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebQuanLyBanHang.Controllers
{
    public class HomeController : Controller
    {
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();
        //trang chu
        public ActionResult Index()
        {
            var listDTM = db.SanPhams.Where(n => n.MaLoaiSp == 1 && n.Moi == 1 && n.DaXoa == false).ToList();
            ViewBag.dsDTM = listDTM;

            var listLTM = db.SanPhams.Where(n => n.MaLoaiSp == 2 && n.Moi == 1 && n.DaXoa == false).ToList();
            ViewBag.dsLTM = listLTM;

            var listMTBM = db.SanPhams.Where(n => n.MaLoaiSp == 3 && n.Moi == 1 && n.DaXoa == false).ToList();
            ViewBag.dsMTBM = listMTBM;
            return View();
        }
        // load menu
        public ActionResult MenuPartial()
        {
            var listSP = db.SanPhams;
            return PartialView(listSP);
        }
        public ActionResult MenuProductPartial()
        {
            var listSP = db.SanPhams;
            return PartialView(listSP);
        }
        // load san pham
        public ActionResult LoadSanPham(string tenloai, string tenNSX)
        {
            if (tenNSX != null )
            {
                var listsp = db.SanPhams.Where(n => n.LoaiSanPham.TenLoai == tenloai && n.NhaSanXuat.TenNSX == tenNSX).ToList();
                ViewBag.listSP = listsp;
            }
            else
            {
                
                var listsp = db.SanPhams.Where(n => n.LoaiSanPham.TenLoai == tenloai).ToList();
                ViewBag.listSP = listsp;
            }
            return View();
        }

        //dang ky
        [HttpGet]
        public ActionResult Dangky()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Dangky(ThanhVien tv,FormCollection f)
        {
            if(ModelState.IsValid) 
            {
                var data = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == tv.TaiKhoan);
                if (data == null)
                {
                    tv.MaLoaiTV = 5;
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                    return RedirectToAction("Index", "DangNhap");
                }
                else
                {
                    ViewBag.errorTaikhoan = "Tài khoản đã được đăng ký!";
                    return View();
                }
                
            }
            else
            {
                ViewBag.error = "Thêm tài khoản tất bại!";
                return View();
            }

        }
        
    }
}