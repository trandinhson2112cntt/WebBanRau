using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

namespace WebBanRauProject.Controllers
{
    public class GioHangController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                //Khoi tao gio hang
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Them Hang vao gio
        public ActionResult ThemGioHang(int iMaSP, string strURL)
        {
            //Lay session gio hang
            List<GioHang> lstGioHang = LayGioHang();
            //Kiem tra sp ton tai trong gio hang khong?
            GioHang sp = lstGioHang.Find(n => n.iMaSP == iMaSP);
            if(sp == null)
            {
                sp = new GioHang(iMaSP);
                lstGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(strURL);
            }
        }

        private double TongSoLuong()
        {
            double iTong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                iTong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "RauShop");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult Xoagiohang(int iMaSP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if(sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "RauShop");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Capnhatgiohang(int iMaSP, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (sp != null)
            {
                sp.iSoLuong = double.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        //Xu ly button Dat hang
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if(Session["TaiKhoan"]==null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "RauShop");
            }
            //Lay gio hang tu session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGioHang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GioHang> listGioHang = LayGioHang();
            ddh.MAKH = kh.MAKH;
            ddh.NGAYDAT = DateTime.Now;
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["Ngaygiao"]);
            ddh.NGAYGIAO = DateTime.Parse(ngaygiao);
            ddh.TINHTRANGGIAOHANG = false;
            ddh.DATHANHTOAN = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Them chi tiet don hang
            foreach (var item in listGioHang)
            {
                CHITIETDONHANG ctdh = new CHITIETDONHANG();
                ctdh.MADH = ddh.MADH;
                ctdh.MASP = item.iMaSP;
                ctdh.SOLUONG = item.iSoLuong;
                ctdh.DONGIA = (decimal)item.dGiaBan;
                data.CHITIETDONHANGs.InsertOnSubmit(ctdh);

            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}