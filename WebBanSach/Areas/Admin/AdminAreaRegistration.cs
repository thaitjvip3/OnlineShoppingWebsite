using System.Web.Mvc;

namespace WebBanSach.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "Admin_login",
               "Admin/login",
               new { Controller = "Auth", action = "Login", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "Admin_logout",
               "Admin/logout",
               new { Controller = "Auth", action = "Logout", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { Controller = "Auth", action = "Login", id = UrlParameter.Optional }
            );
        }

    }
}