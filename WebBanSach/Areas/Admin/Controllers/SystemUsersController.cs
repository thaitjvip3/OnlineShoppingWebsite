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
    public class SystemUsersController : BaseController
    {
        private dbContext db = new dbContext();

        // GET: Admin/SystemUsers
        public ActionResult Index()
        {
            var systemUser = db.SystemUser.Where(p=>p.IDRole!=1);
            return View(systemUser.ToList());
        }

        // GET: Admin/SystemUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUser.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // GET: Admin/SystemUsers/Create
        public ActionResult Create()
        {
           
          
            return View();
        }

        // POST: Admin/SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Password,DisplayName,Email,Phone,Gender,Status,IDRole")] SystemUser systemUser, FormCollection form)
        {
            systemUser.IDRole = 2;
            ModelState.Remove("Password");
            ModelState.Remove("UserID");

            if (ModelState.IsValid)
            {
                string NewPassword = form["NewPassword"].ToString();
                string ConfirmNewPassword = form["ConfirmPassword"].ToString();
                if (NewPassword == "" && ConfirmNewPassword == "")
                {
                    TempData["kthanhcong"] = "Vui lòng nhập Password";
                    return View(systemUser);
                }
                else
                {
                    if (NewPassword != ConfirmNewPassword)
                    {
                        TempData["kthanhcong"] = "Confirm password không đúng";
                        return View(systemUser);
                    }
                    systemUser.Password = NewPassword;
                }
                systemUser.IDRole = 2;
                db.SystemUser.Add(systemUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDRole = new SelectList(db.Role, "ID", "RoleName", systemUser.IDRole);
            return View(systemUser);
        }

        // GET: Admin/SystemUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUser.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDRole = new SelectList(db.Role, "ID", "RoleName", systemUser.IDRole);
            return View(systemUser);
        }

        // POST: Admin/SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Password,DisplayName,Email,Phone,Gender,Status,IDRole")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDRole = new SelectList(db.Role, "ID", "RoleName", systemUser.IDRole);
            return View(systemUser);
        }

        // GET: Admin/SystemUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUser.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: Admin/SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SystemUser systemUser = db.SystemUser.Find(id);
            db.SystemUser.Remove(systemUser);
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
        public void ChangeStatus(string id)
        {
            var user = db.SystemUser.SingleOrDefault(p => p.UserName == id);
            if (user == null)
            {
                return;
            }
            user.Status = user.Status == 1 ? 0 : 1;
            db.SaveChanges();
        }
        public void DeleteAjax(string id)
        {
            var user = db.SystemUser.SingleOrDefault(p => p.UserName == id);
            db.SystemUser.Remove(user);
            db.SaveChanges();
         
        }
        public void ResetPass(string id)
        {
            var user = db.SystemUser.SingleOrDefault(p => p.UserName == id);
            if (user == null)
            {
                return;
            }
            user.Password = user.UserName;
            db.SaveChanges();
        }
       
    }
}
