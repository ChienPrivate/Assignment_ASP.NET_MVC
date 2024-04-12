using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PS27765_NguyenNgocChien_SD18317.Data;
using PS27765_NguyenNgocChien_SD18317.Models;
using System.Collections.Immutable;
using System.Diagnostics;
namespace PS27765_NguyenNgocChien_SD18317.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index(string orderBy, int currentPage = 1, string term = "")
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index","Admin");
            }
            ViewData["SortByName"] = orderBy == "Name" ? "name_desc" : "Name";
            ViewData["SortByPrice"] = orderBy == "Price" ? "price_desc" : "Price";
            ViewData["SortByDate"] = orderBy != "date_desc" ? "date_desc" : "";
            ViewData["CurrentSort"] = orderBy;
            ViewData["CurrentFilter"] = term;
            term = term.ToLower();
            var empData = new ViewModel();
            var products = (from emp in _db.Product
                            where (term == "" || emp.ProductName.ToLower().Contains(term) || emp.CategoryId.ToLower().Contains(term))
                            select new Product
                            {
                                ProductId = emp.ProductId, 
                                ProductName = emp.ProductName,
                                ImageUrl = emp.ImageUrl,
                                Price = emp.Price,
                                ProductQuantity = emp.ProductQuantity,
                                DescriptionText = emp.DescriptionText,
                                CategoryId = emp.CategoryId,
                                RealeaseDate = emp.RealeaseDate,
                                Discount = emp.Discount,
                            });
            List<Category> categories = _db.Category.ToList();
            List<EUser> users = _db.EUser.ToList();
            List<Cart> carts = _db.Cart.ToList();
            empData.Categories = categories;
            empData.Users = users;
            empData.Carts = carts;

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
            int pageSize = 8;
            var tolalPages = (int)Math.Ceiling(totalRecord / (double)pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            empData.CurrentPage = currentPage;
            empData.TotalPages = tolalPages;
            empData.PageSize = pageSize;
            empData.Products = products;

            return View(empData);
        }

        public IActionResult ProductDetails(string pId,int qty = 1)
        {
            var product = _db.Product.Find(pId);
            product.ShoppingQuantity = qty;
            return View(product);
        }

        [HttpPost]
        public IActionResult AdjustQuantity(string productId,int quantity)
        {
            var product = _db.Product.Find(productId);
            if (quantity >= 1)
            {
                return RedirectToAction("ProductDetails", "Home", new { pId = productId, qty = quantity });
            }
            return RedirectToAction("ProductDetails", "Home", new { pId = productId});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
