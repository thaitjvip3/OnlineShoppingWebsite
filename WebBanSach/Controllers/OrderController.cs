using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class OrderController : Controller
    {
        private dbContext db = new dbContext();

        // GET: Order
       

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return View("Error");
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            var userID = User.Identity.GetUserId();
            var user = db.AspNetUsers.Find(userID);
            var cart = db.Cart.SingleOrDefault(model => model.IDUser == userID);
            if (cart == null)
            {
                return View("Error");
            }
            var listCartDetail = db.CartDetail.Where(p => p.IDCart == cart.ID).ToList();
            ViewBag.Phone = user.PhoneNumber;
         
            return View(listCartDetail);
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection filed, int idCart)
        {
            var listCartDetail = db.CartDetail.Where(p => p.IDCart == idCart).ToList();
            string Address = filed["Address"];
            string Phone = filed["Phone"];
            if (Address == "" || Phone == "")
            {
                ViewBag.Error = "Thông tin bắt buộc";
                return View(listCartDetail);
            }
            string Note = filed["note"];
            Order newOder = new Order() {
                Address = Address,
                OrderDate = DateTime.Now,
                Status = 1,
                UserID = User.Identity.GetUserId(),
                Phone=Phone,
                Note=Note,
            };
            db.Order.Add(newOder);
            db.SaveChanges();
          
            var IdOrder = db.Order.Max(p => p.ID);
                
            OrderDetail orderDetail = new OrderDetail();
            var listProduct = db.Product.Where(p => p.Status == 1).ToList();
            foreach (var item in listCartDetail)
            {
                orderDetail.IDProduct = item.IdProduct;
                orderDetail.Quality =(int) item.Quality;
                orderDetail.Price= item.Product.Discout != 0 ? (double)item.Product.Discout : item.Product.ListPrice ;
                orderDetail.IDOrder = IdOrder;
                foreach (var pro in listProduct)
                {
                    if (orderDetail.IDProduct == pro.ID)
                    {
                        pro.Quality = (int)(pro.Quality - orderDetail.Quality);
                     }
                }
                db.OrderDetail.Add(orderDetail);
                db.SaveChanges();
               
            }

            //xóa giỏ hàng của user này đi
            foreach (var item in listCartDetail)
            {
                db.CartDetail.Remove(item);
                db.SaveChanges();
            }
            Cart cart = db.Cart.Find(idCart);
            if (cart != null)
            {
                db.Cart.Remove(cart);
                db.SaveChanges();
            }
            return View("Compelete");
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", order.UserID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,OrderDate,Phone,Address,Note,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", order.UserID);
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
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
            return RedirectToAction(actionName: "MyOder", controllerName: "Manage");

        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
