using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class HomeController : Controller
    {
        private dbContext db = new dbContext();
        public ActionResult Index()
        {

            var userID = User.Identity.GetUserId();
            var count = db.CartDetail.Where(p => p.Cart.IDUser == userID).ToList();
            Session["CartCounter"] = count.Select(c => c.Quality).Sum();

            var listPro = db.Product.Where(p => p.Status == 1).ToList();
            return View(listPro);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}