using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private dbContext db = new dbContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Category).Include(p => p.Supplier);
            return View(product.ToList());
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.IDCategory = new SelectList(db.Category, "ID", "CateName");
            ViewBag.IDSupplier = new SelectList(db.Supplier, "ID", "SuplierName");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductName,IDSupplier,IDCategory,ListPrice,Discout,Status,Detail,Author,Quality")] Product product, HttpPostedFileBase[] files)
        {
            ViewBag.IDCategory = new SelectList(db.Category, "ID", "CateName", product.IDCategory);
            ViewBag.IDSupplier = new SelectList(db.Supplier, "ID", "SuplierName", product.IDSupplier);
            if (files == null && files[0] == null)
            {
                TempData["kthanhcong"] = "Thêm không thành công";
                ViewBag.ShowError = "Vui lòng chọn ảnh";
                return View(product);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (product.Discout == null)
                    {
                        product.Discout = 0;
                    }
                    db.Product.Add(product);
                    db.SaveChanges();
                    if (files != null && files[0] != null)
                    {
                        List<ImgProduct> listImages = new List<ImgProduct>();
                        var imageIndex = 1;
                        foreach (HttpPostedFileBase file in files)
                        {
                            if (file != null)
                            {
                                ImgProduct image = new ImgProduct();

                                var idNext = db.Product.Max(p => p.ID);
                                var InputFileName = idNext + "_" + imageIndex + ".jpg";
                                var ServerSavePath = Path.Combine(Server.MapPath("~/Public/ImageProducts/") + InputFileName);
                                if (System.IO.File.Exists(ServerSavePath))
                                {
                                    System.IO.File.Delete(ServerSavePath);
                                    file.SaveAs(ServerSavePath);
                                    imageIndex++;
                                }
                                else
                                {
                                    file.SaveAs(ServerSavePath);
                                    imageIndex++;
                                }
                                image.IDProduct = idNext;
                                image.Link = "/Public/ImageProducts/" + InputFileName;
                                listImages.Add(image);
                            }
                        }

                      
                        foreach (var item in listImages)
                        {
                            
                            db.ImgProduct.Add(item);
                        }
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }
            
          
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCategory = new SelectList(db.Category, "ID", "CateName", product.IDCategory);
            ViewBag.IDSupplier = new SelectList(db.Supplier, "ID", "SuplierName", product.IDSupplier);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductName,IDSupplier,IDCategory,ListPrice,Discout,Status,Detail,Author,Quality")] Product product, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (product.Discout == null)
                {
                    product.Discout = 0;
                }


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                if (files != null && files[0] != null)
                {
                    List<ImgProduct> listImages = new List<ImgProduct>();
                    var imageIndex = 1;
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            ImgProduct image = new ImgProduct();

                            var id= product.ID;
                            var InputFileName = id + "_" + imageIndex + ".jpg";
                            var ServerSavePath = Path.Combine(Server.MapPath("~/Public/ImageProducts/") + InputFileName);
                            if (System.IO.File.Exists(ServerSavePath))
                            {
                                System.IO.File.Delete(ServerSavePath);
                                file.SaveAs(ServerSavePath);
                                imageIndex++;
                            }
                            else
                            {
                                file.SaveAs(ServerSavePath);
                                imageIndex++;
                            }
                            image.IDProduct = id;
                            image.Link = "/Public/ImageProducts/" + InputFileName;
                            listImages.Add(image);
                        }
                    }
                    var listOld = db.ImgProduct.Where(p => p.IDProduct == product.ID).ToList();
                    foreach (var item in listOld)
                    {
                        db.ImgProduct.Remove(item);

                    }
                    db.SaveChanges();
                    foreach (var item in listImages)
                    {
                        db.ImgProduct.Add(item);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            TempData["kthanhcong"] = "Thêm không thành công";
            ViewBag.IDCategory = new SelectList(db.Category, "ID", "CateName", product.IDCategory);
            ViewBag.IDSupplier = new SelectList(db.Supplier, "ID", "SuplierName", product.IDSupplier);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ChangeStatus(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return;
            }
            product.Status = product.Status == 1 ? 0 : 1;
            db.SaveChanges();
        }
        public bool DeleteAjax(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return false;
            }
            try
            {
                if (db.OrderDetail.Any(p => p.IDProduct == id))
                {
                    return false;
                }
                else
                {
                    var list = db.ImgProduct.Where(p => p.IDProduct == id).ToList();
                    foreach (var item in list)
                    {
                        db.ImgProduct.Remove(item);
                    }
                    db.SaveChanges();
                    db.Product.Remove(product);
                    db.SaveChanges();
                    return true;
                }
               

            }
            catch (Exception)
            {
                return false;
            }


        }

    }
}
