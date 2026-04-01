using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        // GET: Admin/Auth
        private dbContext db = new dbContext();
        public ActionResult Login()
        {
            ViewBag.StrError = "";

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection filed)
        {
            string user = filed["username"];
            string pass = filed["password"];
            //xử lí
            string error = "";
            SystemUser userRow = db.SystemUser.SingleOrDefault(p=>p.UserName==user);
            if (userRow != null)
            {
                if (userRow.Password.Equals(pass))
                {
                    Session["UserAdmin"] = userRow.UserName;
                    var role = userRow.IDRole;
                        if (role == 1)
                        {
                            Session["Role"] = "Admin";
                        }
                        else if (role == 2)
                        {
                            Session["Role"] = "";
                        }
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    error = "Mật khẩu Không chính xác ";
                }
            }
            else
            {
                error = "Tài khoản không tồn tại";
            }
            ViewBag.StrError = error;
            return View();
        }

        public ActionResult Logout()
        {
            Session["UserAdmin"] = "";
            return Redirect("~/Admin/login");
        }
        public ActionResult DetailInfoLogin()
        {
            string id = Session["UserAdmin"].ToString();
            var user = db.SystemUser.SingleOrDefault(p => p.UserName == id);
            return View(user);
        }
        public ActionResult ChangePassWord()
        {
           
           
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassWord(FormCollection filed)
        {
            string OldPassword = filed["OldPassword"].ToString();
            string NewPassword = filed["NewPassword"].ToString();
            string ConfirmNewPassword = filed["ConfirmNewPassword"].ToString();

            string id = Session["UserAdmin"].ToString();
            var user = db.SystemUser.SingleOrDefault(p => p.UserName == id);
            string pass = user.Password;
            if (pass != OldPassword)
            {
                TempData["kthanhcong"] = "pass cũ không đúng";
                return View();
            }
            if (NewPassword != ConfirmNewPassword)
            {
                TempData["kthanhcong"] = "Confirm password không đúng";
                return View();
            }
            user.Password = NewPassword;
            db.SaveChanges();
            return RedirectToAction("DetailInfoLogin");
        }
    }
}