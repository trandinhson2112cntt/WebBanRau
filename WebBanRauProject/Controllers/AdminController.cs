using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

namespace WebBanRauProject.Controllers
{
    public class AdminController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rau()
        {
            return View(data.SANPHAMs.ToList());
        }
        public ActionResult KhachHang()
        {
            return View(data.KHACHHANGs.ToList());
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi"] = "Không được bỏ trống tên đăng nhập,mật khẩu";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PasswordAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Rau", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThemmoiRau()
        {
            //Dua du lieu vao dropdownlist
            //sap xep tang dan theo ten loai rau va ten ncc de lay
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");

            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            return View();
        }
        [HttpPost]
        public ActionResult ThemmoiRau(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");

            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        fileupload.SaveAs(path);//Luu file vao duong dan

                    sp.ANHSP = fileName;

                    data.SANPHAMs.InsertOnSubmit(sp);
                    data.SubmitChanges();
                }
                return RedirectToAction("Rau");
            }
            return View();
        }

        public ActionResult Chitietrau(int id)
        {
            //lay doi tuong
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MaSP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        public ActionResult MonAn()
        {
            return View(data.QuanLyGoiYMonAns.ToList());
        }
        [HttpGet]
        public ActionResult ThemMonAn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMonAn(QuanLyGoiYMonAn mon, HttpPostedFileBase fileupload)
        {
            
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh cho món ăn";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        fileupload.SaveAs(path);//Luu file vao duong dan

                    mon.HINHANH = fileName;

                    data.QuanLyGoiYMonAns.InsertOnSubmit(mon);
                    data.SubmitChanges();
                }
                return RedirectToAction("MonAn");
            }
            return View();
        }

        public ActionResult ChitietMonAn(int id)
        {
            //lay doi tuong
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
            ViewBag.MaSO = mon.MASO;
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mon);
        }

        public ActionResult XoaMonAn(int id)
        {
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
            ViewBag.MaSO = mon.MASO;
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mon);
        }

        [HttpPost,ActionName("XoaMonAn")]
        public ActionResult XacNhanXoa(int id)
        {
            //lấy ra đối tượng món ăn xóa theo mã số
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
             ViewBag.MaSO = mon.MASO;
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.QuanLyGoiYMonAns.DeleteOnSubmit(mon);
            data.SubmitChanges();
            return RedirectToAction("MonAn");

        }

    //Chỉnh sửa món ăn:

        [HttpGet]
        public ActionResult SuaMonAn(int id)
        // lấy ra đối tượng món ăn theo mã số
        {
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mon);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaMonAn(QuanLyGoiYMonAn ql,HttpPostedFileBase fileUpload)
        {
            if(fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh món ăn.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        fileUpload.SaveAs(path);//Luu file vao duong dan

                    ql.HINHANH = fileName;

                    UpdateModel(ql);
                    data.SubmitChanges();
                    
                }
                return RedirectToAction("Rau");
            }
            return RedirectToAction("Rau");
        }
        

        


    }
    
   


}

        public ActionResult ChitietKhachHang(int makh)
        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaSP = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult DeleteKhachHang(int makh)

        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaSP = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("DeleteKhachHang")]

        public ActionResult DeleteKH(int makh)
        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaKH = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(kh);
            data.SubmitChanges();

            return RedirectToAction("KhachHang");
        }
        public ActionResult Lienhe()
        {
            return null;
        }
        [HttpGet]
        public ActionResult SuaKH(int makh)
        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaKH = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaKH(KHACHHANG khachhang)
        {


            if (ModelState.IsValid)
            {

                //Luu vao CSDL  

                UpdateModel(khachhang);
                data.SubmitChanges();

            }
            return RedirectToAction("KhachHang");

        }
        
        [HttpGet]
        public ActionResult SuaRau(int id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View();
        }

        public ActionResult SuaRau(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");

            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        fileupload.SaveAs(path);//Luu file vao duong dan

                    sp.ANHSP = fileName;

                    UpdateModel(sp);
                    data.SubmitChanges();
                }
                return RedirectToAction("Rau");
            }
            return View();
        }
    }

}