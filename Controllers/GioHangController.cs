using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebsiteQuanLyBanHang.Controllers
{
    public class GioHangController : Controller
    {
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();
        // GET: GioHang
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> listGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(listGioHang == null)
            {
                listGioHang = new List<ItemGioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }
        public ActionResult ThemGioHang(int MaSP,string URl)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> listGioHang = LayGioHang();
            ItemGioHang spCheck = listGioHang.SingleOrDefault(n => n.MaSp == MaSP);
            if(spCheck != null)
            {
                if(sp.SoLuongTon < spCheck.SoLuongSp)
                {
                    return View("Thongbao");
                }
                else
                {
                    spCheck.SoLuongSp++;
                    spCheck.ThanhTien = spCheck.SoLuongSp * spCheck.DonGia;
                    return Redirect(URl);
                }
            }
            ItemGioHang GH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < GH.SoLuongSp)
            {
                return View("Thongbao");
            }
            
            listGioHang.Add(GH);
            return Redirect(URl);
        }

        public double TinhTongSoLuong()
        {
            List < ItemGioHang > listGH = Session["GioHang"] as List<ItemGioHang>;
            if(listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n => n.SoLuongSp);
        }
        public decimal TinhTongTien()
        {
            List < ItemGioHang > listGH = Session["GioHang"] as List<ItemGioHang>;
            if(listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n => n.ThanhTien);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.Tongsoluong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            else 
            {
                ViewBag.Tongsoluong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView();
            }
        }
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }
        public ActionResult XemGioHangPartial()
        {
    
            return PartialView();
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSp == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.GioHang = lstGioHang;
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(int MaSP,int SoLuong)
        {
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == MaSP);
            if (spCheck.SoLuongTon < SoLuong)
            {
                return View("thongbao");
            }
            List<ItemGioHang> lstGioHang = LayGioHang();

            ItemGioHang itemGHUpDate = lstGioHang.Find(n => n.MaSp == MaSP);
            itemGHUpDate.SoLuongSp = SoLuong;
            itemGHUpDate.ThanhTien = itemGHUpDate.SoLuongSp * itemGHUpDate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSp == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang(KhachHang KH)
        {
            if (Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khachHang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                khachHang = KH;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            else
            {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khachHang.TenKH = tv.HoTen;
                khachHang.DiaChi= tv.DiaChi;
                khachHang.Email= tv.Email;
                khachHang.SDT= tv.SDT;
                khachHang.MaThanhVien = tv.MaTV;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            // them don hang
            DonDatHang donDatHang = new DonDatHang();
            donDatHang.MaKH = khachHang.MaKH;
            donDatHang.NgayDat = DateTime.Now;
            donDatHang.TinhTrangGiao = false;
            donDatHang.DaThanhToan = false;
            donDatHang.UuDai = 0;
            db.DonDatHangs.Add(donDatHang);
            db.SaveChanges();
            //them chi tiet don hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                ChiTietDonDatHang ctddh = new ChiTietDonDatHang();
                ctddh.MaDDH = donDatHang.MaDDH;
                ctddh.MaSP = item.MaSp;
                ctddh.TenSP = item.TenSp;
                ctddh.SoLuong = item.SoLuongSp;
                ctddh.DonGia= item.DonGia;
                db.ChiTietDonDatHangs.Add(ctddh);
                
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("DatHangThanhCong");
        }
        public ActionResult DatHangThanhCong()
        {
            return View();
        }
    }
}