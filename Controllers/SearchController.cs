using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyBanHang.Models;

namespace WebsiteQuanLyBanHang.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities();
        public ActionResult KQTimKiem(string sTuKhoa)
        {
            var listSP = db.SanPhams.Where(n=>n.TenSP.Contains(sTuKhoa));
            return View(listSP.OrderBy(n=>n.TenSP));
        }
    }
}