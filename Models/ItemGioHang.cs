 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteQuanLyBanHang.Models
{
    public class ItemGioHang
    {
        public string TenSp { get; set; }
        public int MaSp { get; set; } 
        public int SoLuongSp { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }

        public ItemGioHang(int masp)
        {
            using (WebQuanLyBanHangEntities db= new WebQuanLyBanHangEntities())
            {
                this.MaSp = masp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == masp);
                TenSp =sp.TenSP;
                HinhAnh=sp.HinhAnh;
                DonGia = sp.DonGia.Value;
                ThanhTien = DonGia * SoLuongSp;

            }
        }
        public ItemGioHang(int masp, int sl)
        {
            using (WebQuanLyBanHangEntities db = new WebQuanLyBanHangEntities())
            {
                this.MaSp = masp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == masp);
                TenSp = sp.TenSP;
                HinhAnh = sp.HinhAnh;
                DonGia = sp.DonGia.Value;
                SoLuongSp = sl;
                ThanhTien = DonGia * SoLuongSp;

            }
        }

        public ItemGioHang()
        {

        }


    }
}