using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;
namespace WebBanRauProject.Controllers
{
    public class MonAnController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        // GET: MonAn
        public ActionResult Index()
        {
            var monan = from mon in data.QuanLyGoiYMonAns select mon;
            return View(monan);
        }

        public ActionResult Details(int id)
        {
            var monan = from mon in data.QuanLyGoiYMonAns where mon.MASO == id select mon;
            return View(monan.Single());
        }

    }
}