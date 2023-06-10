using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebQuanLyBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities ();
        // GET: SanPham
        [ChildActionOnly]
        public ActionResult SanPhamStyle1()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2()
        {
            return PartialView();
        }
        public ActionResult XemChiTietSp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }

        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX) 
        {
            if (MaLoaiSP == null|| MaNSX==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n=>n.MaLoaiSp==MaLoaiSP && n.MaNSX==MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            return View();
        }
    }
}