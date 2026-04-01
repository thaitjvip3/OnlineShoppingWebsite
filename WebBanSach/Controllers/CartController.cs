using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class CartController : Controller
    {
        private dbContext db = new dbContext();
       private List<Cart> listCart= new List<Cart>();
        // GET: Cart
       
       
        public int? Add(string id,int? Quality)
        {
            if (Request.IsAuthenticated)
            {
                if (Quality == null)
                {
                    Quality = 1;
                }
                var userID = User.Identity.GetUserId();
                var cart = db.Cart.Where(model => model.IDUser == userID).SingleOrDefault();
                if (cart == null)
                {
                    Cart newCart = new Cart()
                    {
                        IDUser = User.Identity.GetUserId(),
                    };
                    db.Cart.Add(newCart);
                    db.SaveChanges();
                    //tạo chi tiết
                    CartDetail cartDetail = new CartDetail()
                    {
                        IDCart = db.Cart.SingleOrDefault(model => model.IDUser == userID).ID,
                        IdProduct = Int32.Parse(id),
                        Quality = Quality
                    };
                    db.CartDetail.Add(cartDetail);
                    db.SaveChanges();
                }
                else
                {
                    int idPro = Int32.Parse(id);
                    var check = db.CartDetail.SingleOrDefault(p => p.IdProduct == idPro && p.IDCart== cart.ID);

                    if (check == null)
                    {
                        CartDetail cartDetail = new CartDetail()
                        {
                            IDCart = db.Cart.SingleOrDefault(model => model.IDUser == userID).ID,
                            IdProduct = Int32.Parse(id),
                            Quality = Quality
                        };
                        
                        db.CartDetail.Add(cartDetail);
                        db.SaveChanges();
                    }
                    else
                    {
                   
                        check.Quality+=Quality;
                        var quantityRemaining = db.Product.SingleOrDefault(p => p.ID == idPro).Quality;
                        if (check.Quality > quantityRemaining)
                        {
                            check.Quality = quantityRemaining;
                        }
                        db.SaveChanges();
                    }
                }
                   
                   
                var listDetail = db.CartDetail.Where(p => p.Cart.IDUser == userID).ToList();
                var count= listDetail.Select(c => c.Quality).Sum();
                Session["CartCounter"] = count;
                //Session["Cartitem"]=
                return count;
            }
            else
            {
              
                return -1;
            }

        }
        

        // GET: Cart/Details/5
        public ActionResult Details()
        {
            if (Request.IsAuthenticated)
            {
                var userID = User.Identity.GetUserId();
                var cart = db.Cart.SingleOrDefault(model => model.IDUser == userID);
                if (cart == null)
                {
                    return View("Error");
                }
                var listCartDetail = db.CartDetail.Where(p => p.IDCart == cart.ID).ToList();
                return View(listCartDetail);
            }
            else
            {
                return RedirectToAction(actionName: "Login", controllerName: "Account");
            }
        }

       

      

        // GET: Cart/Edit/5
        public void Edit(int id, int qua)
        {
           
            var cart = db.CartDetail.Find(id);
            if (cart != null)
            {
                cart.Quality = qua;
                db.SaveChanges();
            }
       
          
           
        }


        // GET: Cart/Delete/5
        public void Delete(int id)
        {
          
            var  cartDetail = db.CartDetail.Find(id);
            if (cartDetail != null)
            {
                db.CartDetail.Remove(cartDetail);
                db.SaveChanges();
            }
         
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
