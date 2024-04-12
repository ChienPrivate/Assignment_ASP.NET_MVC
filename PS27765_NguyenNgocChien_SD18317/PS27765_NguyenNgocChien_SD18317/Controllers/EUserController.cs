using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using PS27765_NguyenNgocChien_SD18317.Common;
using PS27765_NguyenNgocChien_SD18317.Data;
using PS27765_NguyenNgocChien_SD18317.Models;
using PS27765_NguyenNgocChien_SD18317.Utility;
using System.Security.Cryptography;
using System.Text;

namespace PS27765_NguyenNgocChien_SD18317.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class EUserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CommonMethods _methods;
        public EUserController(ApplicationDbContext db,CommonMethods methods)
        {
            _db = db;
            _methods = methods;
        }

        public IActionResult Index()
        {
            var objlist = _db.EUser.ToList();
            return View(objlist);
        }

        public IActionResult Edit(string uId) 
        {
            var user = _db.EUser.Find(uId);
            var rolelist = new List<SelectListItem>
            {
                new SelectListItem{Text = SD.RoleAdmin,Value= SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value= SD.RoleCustomer}
            };
            ViewBag.RoleList = rolelist;
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(EUser obj)
        {
            if (ModelState.IsValid)
            {
                _db.EUser.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Cập nhật người dùng thành công";
                return RedirectToAction("Index");
            }
            var rolelist = new List<SelectListItem>
            {
                new SelectListItem{Text = SD.RoleAdmin,Value= SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value= SD.RoleCustomer}
            };
            ViewBag.RoleList = rolelist;
            return View();
        }

        public IActionResult Delete(string uId) 
        {
            var user = _db.EUser.Find(uId);
            var rolelist = new List<SelectListItem>
            {
                new SelectListItem{Text = SD.RoleAdmin,Value= SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value= SD.RoleCustomer}
            };
            ViewBag.RoleList = rolelist;
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(string uId)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var user = _db.EUser.FirstOrDefault(u => u.EUserId == uId);
            var userCart = _db.Cart.FirstOrDefault(u => u.EUserId == uId);
            if (userCart != null)
            {
                var userCartDetails = _db.CartDetails.Where<CartDetails>(cd => cd.CartId == userCart.CartId);
                if (userCartDetails != null)
                {
                    _db.CartDetails.RemoveRange(userCartDetails);
                    _db.SaveChanges();
                }
            }
            if (currentUserId != user.EUserId)
            {
                if (user == null)
                {
                    return NotFound();
                }
                if (userCart != null)
                {
                    _db.Cart.Remove(userCart);
                }
                _db.EUser.Remove(user);
                _db.SaveChanges();
                TempData["success"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Bạn không thể tự xóa tài khoản của chính mình";
            return RedirectToAction("Delete", "EUser",new {uId = uId });
        }

        public IActionResult Create()
        {
            var rolelist = new List<SelectListItem>
            {
                new SelectListItem{Text = SD.RoleAdmin,Value= SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value= SD.RoleCustomer}
            };
            ViewBag.RoleList = rolelist;
            ViewData["newUId"] = _methods.GenerateNewEUserId();
            ViewData["newPwd"] = _methods.Encryption("abc123").ToUpper();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EUser user)
        {
            string userId = _methods.GenerateNewEUserId();
            string pwd = _methods.Encryption("abc123").ToUpper();
            user.EUserId = userId;
            user.Pwd = pwd;
            if (ModelState.IsValid)
            {
                _db.EUser.Add(user);
                _db.SaveChanges();
                TempData["success"] = "Thêm người dùng thành công";
                return RedirectToAction("Index");
            }
            var rolelist = new List<SelectListItem>
            {
                new SelectListItem{Text = SD.RoleAdmin,Value= SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value= SD.RoleCustomer}
            };
            ViewBag.RoleList = rolelist;
            return View();
        }

    }
}
