using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;
namespace WebBanRauProject.Controllers
{
    public class TinTucController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        // GET: TinTuc
        public ActionResult Index()
        {
            return View();
        }


    }
}