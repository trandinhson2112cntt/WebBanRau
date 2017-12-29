using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanRauProject.Models
{
    public class GioHang
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sANHSP { get; set; }
        public double dGiaBan { get; set; }
        public double iSoLuong { get; set; }
        public Double dThanhTien {
            get { return iSoLuong * dGiaBan; }
        }
        public GioHang(int MaSP)
        {
            iMaSP = MaSP;
            SANPHAM sp = data.SANPHAMs.Single(n => n.MASP == iMaSP);
            sTenSP = sp.TENSP;
            sANHSP = sp.ANHSP;
            dGiaBan = double.Parse(sp.GIABAN.ToString());
            iSoLuong = 1;
        }
    }
}