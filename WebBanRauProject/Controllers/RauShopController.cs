using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

namespace WebBanRauProject.Controllers
{
    public class RauShopController : Controller
    {
        //Tao doi tuong chua toan bo CSDL
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        private List<SANPHAM> LaySanPhamMoiNhap(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NGAYCAPNHAT).Take(count).ToList();
        }
        // GET: RauShop
        public ActionResult Index()
        {
            //Lay 4 Rau moi nhap
            var raumoi = LaySanPhamMoiNhap(4);
            return View(raumoi);
        }
        //Dua loai rau vao menu list
        public ActionResult LoaiSanPham()
        {
            //Lay 4 Rau moi nhap
            var loai = from sp in data.LOAIRAUs select sp;
            return PartialView(loai);
        }
        public ActionResult SanPhamTheoLoai(int id)
        {
            //Lay 4 Rau moi nhap
            var rau = from sp in data.SANPHAMs where sp.MALOAI == id select sp;
            return View(rau);
        }
        //Chi tiet 1 loai Rau
        public ActionResult Details(int id)
        {
            var rau = from sp in data.SANPHAMs where sp.MASP == id select sp;
            return View(rau.Single());
        }
        //Hien thi tat ca san pham
        public ActionResult ShowAll()
        {
            var rau = from sp in data.SANPHAMs select sp;
            return View(rau);
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult ThongTin()
        {
            return View();

        }

    }
}