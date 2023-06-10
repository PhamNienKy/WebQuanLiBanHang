using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebsiteQuanLyBanHang.Controllers
{
    [Authorize(Roles = "Quantri")]
    public class QuanLySanPhamController : Controller
    {
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();
        // GET: QuanLySanPham
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n=>n.DaXoa==false));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            var str = Server.MapPath("~/Content/HinhAnhSp/");
            for (int i = 0; i < 4; i++)
            {
                //kiem tra hinh anh trong csdl ton tai chua
                if (HinhAnh[i]!=null && HinhAnh[i].ContentLength > 0)
                {
                    //lay ten hinh anh
                    var filename= Path.GetFileName(HinhAnh[i].FileName);

                    var path = Path.Combine(str, filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.upload = "Hình thu " +i+ " đã tồn tại!";
                        return View();
                    }
                    else
                    {
                        //lay hinh anh dua vao thu muc                
                        HinhAnh[i].SaveAs(path);
                        switch (i)
                        {
                            case 0:
                                sp.HinhAnh = filename;
                                break;
                            case 1:
                                sp.HinhAnh1 = filename;
                                break;
                            case 2:
                                sp.HinhAnh2 = filename;
                                break;
                            case 3:
                                sp.HinhAnh3 = filename;
                                break;

                        }
                    }
                }
            }
            
            sp.NgayCapNhat = DateTime.Now;
            
            sp.DaXoa = false;
           
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? MaSP)
        {
            //lay san pham can chinh sua
            if( MaSP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n=>n.MaSP== MaSP);
            if(sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSp);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);

            return View(sp);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSp);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            model.NgayCapNhat = DateTime.Now;
            var str = Server.MapPath("~/Content/HinhAnhSp/");
            for (int i = 0; i < 4; i++)
            {
                //kiem tra hinh anh lay duoc 
                if (HinhAnh[i] != null && HinhAnh[i].ContentLength > 0)
                {
                    //lay ten hinh anh
                    var filename = Path.GetFileName(HinhAnh[i].FileName);
                    //lay duong dan
                    var path = Path.Combine(str, filename);

                    // neu anh co trong thu muc thi khong luu vao thu muc 
                    if (System.IO.File.Exists(path))
                    {
                        switch (i)
                        {
                            case 0:
                                model.HinhAnh = filename;
                                break;
                            case 1:
                                model.HinhAnh1 = filename;
                                break;
                            case 2:
                                model.HinhAnh2 = filename;
                                break;
                            case 3:
                                model.HinhAnh3 = filename;
                                break;

                        }
                        
                    }
                    // neu anh khong co trong thu muc thi luu vao thu muc 
                    else
                    {
                        //lay hinh anh dua vao thu muc                
                        HinhAnh[i].SaveAs(path);


                        switch (i)
                        {
                            case 0:
                                model.HinhAnh = filename;
                                break;
                            case 1:
                                model.HinhAnh1 = filename;
                                break;
                            case 2:
                                model.HinhAnh2 = filename;
                                break;
                            case 3:
                                model.HinhAnh3 = filename;
                                break;

                        }
                    }
                    
                }
                //else
                //{
                //    switch (i)
                //    {
                //        case 0:
                //            model.HinhAnh = hinhAnh;
                //            break;
                //        case 1:
                //            model.HinhAnh1 = hinhAnh1;
                //            break;
                //        case 2:
                //            model.HinhAnh2 = hinhAnh2;
                //            break;
                //        case 3:
                //            model.HinhAnh3 = hinhAnh3;
                //            break;

                //    }
                //}
            }
            // luu thay doi vao csdl 
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Xoa(int? MaSP)
        {
            if (MaSP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSp);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);

            return View(sp);
            
        }

        [HttpPost]
        public ActionResult Xoa(int MaSP)
        {
           
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //xoa san pham khoi csdl
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}