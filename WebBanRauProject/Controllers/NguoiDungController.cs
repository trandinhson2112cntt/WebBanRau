using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

namespace WebBanRauProject.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Vui lòng nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai) || matkhaunhaplai != matkhau )
            {
                ViewData["Loi4"] = "Vui lòng nhập đúng";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi5"] = "Vui lòng nhập địa chỉ";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi5"] = "Vui lòng nhập số điện thoại";
            }
            else
            {
                kh.HOTEN = hoten;
                kh.TAIKHOAN = tendn;
                kh.MATKHAU = matkhau;
                kh.DiachiKH = diachi;
                kh.NGAYSINH = DateTime.Parse(ngaysinh);
                kh.DIENTHOAIKH = dienthoai;
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }

            return this.DangKy() ;
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TAIKHOAN == tendn && n.MATKHAU == matkhau);
                if(kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công!!!";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("DatHang", "GioHang");
                }
                else
                {
                    ViewBag.Thongbao = "Tài khoản hoặc mật khẩu không chính xác!!!";
                }
            }
            return View();
        }

        public ActionResult DangNhapPartial()
        {
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            if (kh != null)
                ViewBag.TenKhachHang = kh.HOTEN;
            return PartialView();
        }

        public ActionResult ThongTinKhachHang()
        {
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            if (kh != null)
            {
                var khachhang = data.KHACHHANGs.First(k => k.MAKH == kh.MAKH);
                return View(khachhang);
            }
            else
                return RedirectToAction("DangNhap");
        }

        public ActionResult SuaThongTinKH(int id)
        {
            var kh = data.KHACHHANGs.First(k => k.MAKH == id);
            return View(kh);
        }
        [HttpPost]
        public ActionResult SuaThongTinKH(int id,FormCollection collection)
        {
            var kh = data.KHACHHANGs.First(k => k.MAKH == id);
            string tenKH = collection["HOTEN"];
            DateTime ngaySinh = DateTime.Parse(collection["NGAYSINH"]);
            string diaChi = collection["DiachiKH"];
            string dienThoai = collection["DIENTHOAIKH"];

            kh.HOTEN = tenKH;
            kh.NGAYSINH = ngaySinh;
            kh.DiachiKH = diaChi;
            kh.DIENTHOAIKH = dienThoai;

            UpdateModel(kh);
            data.SubmitChanges();
            return RedirectToAction("ThongTinKhachHang");
        }
    }
    
}