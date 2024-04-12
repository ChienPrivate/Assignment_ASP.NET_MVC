using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PS27765_NguyenNgocChien_SD18317.Common;
using PS27765_NguyenNgocChien_SD18317.Data;
using PS27765_NguyenNgocChien_SD18317.Models;
using System.Security.Cryptography;
using System.Text;

namespace PS27765_NguyenNgocChien_SD18317.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public Common.CommonMethods _methods;
        public ProductController(ApplicationDbContext db,CommonMethods methods)
        {
            _db = db;
            _methods = methods;
        }

        public async Task<IActionResult> Index(string orderBy, int currentPage = 1, string term = "")
        {


            ViewData["SortByName"] = orderBy == "Name" ? "name_desc" : "Name";
            ViewData["SortByPrice"] = orderBy == "Price" ? "price_desc" : "Price";
            ViewData["SortByDate"] = orderBy != "date_desc" ? "date_desc" : "";
            ViewData["CurrentSort"] = orderBy;
            ViewData["CurrentFilter"] = term;
            term = term.ToLower();
            var empData = new ViewModel();
            var products = (from emp in _db.Product
                            where term == "" || emp.ProductName.ToLower().Contains(term)
                            select new Product
                            {
                                ProductId = emp.ProductId,
                                ProductName = emp.ProductName,
                                Price = emp.Price,
                                ImageUrl = emp.ImageUrl,
                                ProductQuantity = emp.ProductQuantity,
                                DescriptionText = emp.DescriptionText,
                                CategoryId = emp.CategoryId,
                                RealeaseDate = emp.RealeaseDate,
                                Discount = emp.Discount,
                                Info = emp.Info,
                            });
            List<Category> categories = _db.Category.ToList();
            empData.Categories = categories;

            switch (orderBy)
            {
                case "Name":
                    products = products.OrderBy(s => s.ProductName);
                    break;
                case "name_Desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.Select(s => s);
                    break;
            }
            int totalRecord = products.Count();
            int pageSize = 5;
            var tolalPages = (int)Math.Ceiling(totalRecord / (double)pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            empData.CurrentPage = currentPage;
            empData.TotalPages = tolalPages;
            empData.PageSize = pageSize;
            empData.Products = products;

            return View(empData);
        }

        public IActionResult Create()
        {
            var objCategories = _db.Category.ToList();
            var categoryList = new List<SelectListItem>();
            foreach (var c in objCategories)
            {
                categoryList.Add(new SelectListItem { Text = c.CategoryName, Value = c.CategoryId });
            }
            string newId = _methods.GenerateNewProductId();
            ViewBag.CategoryList = categoryList;
            ViewData["NewPId"] = newId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            string newId = _methods.GenerateNewProductId();
            obj.ProductId = newId;

            if (ModelState.IsValid)
            {
                _db.Product.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            var objCategories = _db.Category.ToList();
            var categoryList = new List<SelectListItem>();
            foreach (var c in objCategories)
            {
                categoryList.Add(new SelectListItem { Text = c.CategoryName, Value = c.CategoryId });
            }
            ViewBag.CategoryList = categoryList;

            return View();
        }

        public IActionResult Edit(string? id)
        {
            var objProduct = _db.Product.Find(id);
            var objCategories = _db.Category.ToList();

            var categoryList = new List<SelectListItem>();

            foreach (var c in objCategories)
            {
                categoryList.Add(new SelectListItem { Text = c.CategoryName, Value = c.CategoryId });
            }
            ViewBag.ImgUrl = objProduct?.ImageUrl;
            ViewBag.CategoryList = categoryList;

            return View(objProduct);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var objProduct = _db.Product.Find(id);
            if (objProduct == null)
            {
                return NotFound();
            }
            return View(objProduct);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(string? id)
        {
            var objProduct = _db.Product.Find(id);
            if (objProduct == null)
            {
                return NotFound();
            }
            _db.Product.Remove(objProduct);
            _db.SaveChanges();
            TempData["success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }

        #region Support Method
        

        public string Encryption(string pwd)
        {
            byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);
            MD5 md5 = MD5.Create();
            byte[] encryptBytes = md5.ComputeHash(pwdBytes);
            StringBuilder pwdString = new StringBuilder();
            for (int i = 0; i < encryptBytes.Length; i++)
            {
                pwdString.Append(encryptBytes[i].ToString("x2"));
            }
            return pwdString.ToString();
        }
        #endregion
    }
}
