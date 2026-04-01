using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private dbContext db = new dbContext();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var order = db.Order.Include(o => o.AspNetUsers);
            order = order.Where(p => p.Status == 1);
            return View(order.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Create
       

       
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void DeleteAjax(int id)
        {

            Order order = db.Order.Find(id);
            if (order != null)
            {
                var orderDetails = db.OrderDetail.Where(p => p.IDOrder == id).ToList();

                List<Product> listProduct = db.Product.ToList();
                foreach (var details in orderDetails)
                {
                    foreach (var pro in listProduct)
                    {
                        if (details.IDProduct == pro.ID)
                        {
                            pro.Quality = (int)(pro.Quality + details.Quality);
                            db.SaveChanges();
                        }
                    }
                }
                order.Status = 0;
                db.SaveChanges();
            }
        

        }
        public void Approve(int id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return;
            }
            order.Status = 2;
            db.SaveChanges();

        }

        public void Compelete(int id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return;
            }
            order.Status = 3;
            db.SaveChanges();


        }
        public ActionResult ApproveOrder()
        {
           
                var order = db.Order.Include(o => o.AspNetUsers);
                order = order.Where(p => p.Status == 2);
                return View(order.ToList());
           
        }
        public ActionResult CompeleteOrder()
        {

            var order = db.Order.Include(o => o.AspNetUsers);
            order = order.Where(p => p.Status == 3);
            return View(order.ToList());

        }
        public ActionResult DeleteOrder()
        {

            var order = db.Order.Include(o => o.AspNetUsers);
            order = order.Where(p => p.Status == 0);
            return View(order.ToList());

        }
    }
}
