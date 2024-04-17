using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.EntityFrameworkCore;
using PS27765_NguyenNgocChien_SD18317.Common;
using PS27765_NguyenNgocChien_SD18317.Data;
using PS27765_NguyenNgocChien_SD18317.Models;
using PS27765_NguyenNgocChien_SD18317.Models.ViewModels;
using System.Linq;

namespace PS27765_NguyenNgocChien_SD18317.Controllers
{

    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly CommonMethods _methods;
        public CartController(ApplicationDbContext db, CommonMethods methods)
        {
            _db = db;
            _methods = methods;
        }

        public async Task<IActionResult> Index(string userId)
        {
            // Tìm giỏ hàng của người dùng
            var userCart = await _db.Cart.FirstOrDefaultAsync(c => c.EUserId == userId);

            if (userCart == null)
            {
                Cart newCart = new Cart()
                {
                    EUserId = userId,
                    CartId = _methods.GenerateNewCartId(),
                };
                _db.Cart.Add(newCart);
                _db.SaveChanges();
                return View();
            }

            var products = await (from c in _db.Cart
                                  join cd in _db.CartDetails on c.CartId equals cd.CartId
                                  join p in _db.Product on cd.ProductId equals p.ProductId
                                  where c.EUserId == userCart.EUserId
                                  select new CartViewModel
                                  {
                                      CartId = c.CartId,
                                      Product = p,
                                      Quantity = cd.Quantity
                                  }).ToListAsync();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CartDetails? cartDetails,string cartId,string userId, string productId, int quantity)
        {
            cartDetails.CartId = cartId;
            cartDetails.ProductId = productId;
            var userCart = await _db.Cart.FirstOrDefaultAsync(c => c.EUserId == userId);
            if (userCart == null)
            {
                Cart newCart = new Cart()
                {
                    EUserId = userId,
                    CartId = _methods.GenerateNewCartId(),
                };
                _db.Cart.Add(newCart);
                _db.SaveChanges();
            }
            var existProd =  _db.CartDetails.FirstOrDefault(cd => cd.CartId == cartId && cd.ProductId == productId);
            if (existProd != null)
            {
                existProd.Quantity += quantity;
                _db.CartDetails.Update(existProd);
                await _db.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm vào giỏ hàng công";
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                await _db.CartDetails.AddAsync(cartDetails);
                await _db.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm vào giỏ hàng công";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string cartId,string productId,string uId)
        {
            var product = await _db.CartDetails.FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.ProductId == productId);
            if (product != null)
            {
                _db.CartDetails.Remove(product);
                await _db.SaveChangesAsync();
                TempData["success"] = "Xóa sản phẩm thành công";
            }
            return RedirectToAction("Index","Cart",new { userId = uId });
        }

        [HttpPost]
        public async Task<IActionResult> ModifyQuantity(string cartId, string productId, int quantity, string uId)
        {
            var obj = await _db.CartDetails.FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.ProductId == productId);
            if (obj != null)
            {
                if (quantity >= 1)
                {
                    obj.Quantity = quantity;
                    _db.CartDetails.Update(obj);
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Cart", new { userId = uId });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            CartDetails cartDetails = new();
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var cart = _db.Cart.FirstOrDefault(c => c.EUserId == userId);
            var existCart = _db.CartDetails.FirstOrDefault(cd => cd.CartId == cart.CartId && cd.ProductId == productId);
            if (existCart != null)
            {
                existCart.Quantity += quantity;
                _db.CartDetails.Update(existCart);
                await _db.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index", "Cart", new { userId = cart.EUserId });
            }
            cartDetails.ProductId = productId;
            cartDetails.CartId = cart.CartId;
            cartDetails.Quantity = quantity;
            await _db.CartDetails.AddAsync(cartDetails);
            await _db.SaveChangesAsync();
            TempData["success"] = "Thêm sản phẩm thành công";
            return RedirectToAction("Index","Cart",new {userId = cart.EUserId});
        }

        [HttpPost]
        public IActionResult PayMent()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var cart = _db.Cart.FirstOrDefault(c => c.EUserId == uId);
            var a = _db.CartDetails.Where(cd => cd.CartId == cart.CartId);
            _db.CartDetails.RemoveRange(a);
            _db.SaveChanges();
            return RedirectToAction("Index", "Cart", new { userId = uId });
        }

    }
}
