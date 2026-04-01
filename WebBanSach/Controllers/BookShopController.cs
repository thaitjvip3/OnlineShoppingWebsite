using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;
using PagedList;
namespace WebBanSach.Controllers
{
    public class BookShopController : Controller
    {
        // GET: BookShop
        private dbContext db = new dbContext();
        public ActionResult Detail(int id)
        {
            Product product = db.Product.SingleOrDefault(p => p.ID == id && p.Status==1);

            ViewBag.Rela = db.Product.Where(p => p.ID != id && product.IDCategory == p.IDCategory && p.Status==1).ToList().Take(5);
            return View(product);
        }
        public ActionResult SearchBook(FormCollection f, int? size, int? page, string cate, string suplier, string name1)
        {
            string name = f["search"];
            var Listproduct = db.Product.Where(p => p.Status == 1 && p.Quality >= 1).ToList();

            if (cate != null)
            {
                ViewBag.CateChose1 = cate;
                string[] arrListStr = cate.Split(',');
                List<Product> listCate = new List<Product>();
                bool check = false;
                foreach (var item in arrListStr)
                {
                    if (!item.Equals(""))
                    {
                        listCate.AddRange(Listproduct.Where(p => p.Category.CateName == item).ToList());
                        check = true;
                    }
            

                }
                if (listCate.Count>=0)
                {
                   
                        if (check)
                        {
                            Listproduct = listCate;
                        }
                    
                   
                }
              
                ViewBag.CateChose = arrListStr;
            }
            if (suplier != null)
            {
                ViewBag.SuplierChose1 = suplier;
                string[] arrListSuplier = suplier.Split(',');
                List<Product> listSuplier = new List<Product>();
                bool check = false;
                foreach (var item in arrListSuplier)
                {
                    if (!item.Equals(""))
                    {
                        listSuplier.AddRange(Listproduct.Where(p => p.Supplier.SuplierName == item ).ToList());
                        check = true;
                    }


                }
                if (listSuplier.Count >= 0 )
                {
                    
                        if (check)
                        {
                            Listproduct = listSuplier;
                        }
                    
                  
                }

                ViewBag.SuplierChose = arrListSuplier;
            }

            if (name != null)
            {
                if (!name.Equals(""))
                {
                    Listproduct = Listproduct.Where(p=> RemoveUnicode(p.ProductName).ToLower().Contains(RemoveUnicode(name).ToLower())).ToList();
                    ViewBag.Name = name;
                }
            }
            else if (name1 != null)
            {
                if (!name1.Equals(""))
                {
                    Listproduct = Listproduct.Where(p => RemoveUnicode(p.ProductName).ToLower().Contains(RemoveUnicode(name1).ToLower())).ToList();
                    ViewBag.Name = name1;
                }
            }
            //ViewBag.size=
            ViewBag.currentSize = size;
            int pageNumber = (page??1) ;
            int pageSize = (size ?? 6);
            int total = (int)(Listproduct.Count / pageSize) + 1;

            if(pageNumber > total)
            {
                pageNumber = total;
            }
            ViewBag.Supplier= db.Supplier.Where(p => p.Status == 1).ToList();
            ViewBag.Cate = db.Category.Where(p => p.Status == 1).ToList();
            return View(Listproduct.ToPagedList(pageNumber, pageSize));
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

    }
   
}