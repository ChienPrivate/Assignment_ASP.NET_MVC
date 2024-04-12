using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.Security.Claims;
using PS27765_NguyenNgocChien_SD18317.Data;
using PS27765_NguyenNgocChien_SD18317.Models;
using PS27765_NguyenNgocChien_SD18317.Common;

namespace PS27765_NguyenNgocChien_SD18317.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly CommonMethods _methods;
        public AdminController(ApplicationDbContext db, CommonMethods methods)
        {
            _db = db;
            _methods = methods;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            if (ModelState.IsValid)
            {
                var user = _db.EUser.FirstOrDefault(u => u.Email == login.Email && _methods.Encryption(login.Password) == u.Pwd);
                if (user != null)
                {
                    if (user.IsConfirm == false)
                    {
                        HttpContext.Session.SetString("SecretKey", _methods.GenerateRandomString(8));
                        string randomString = HttpContext.Session.GetString("SecretKey");
                        string sendMailNotify = _methods.SendKeyToMail(user.Email, "Samsung Account Verify Key", randomString);
                        if (sendMailNotify == "gửi mail thành công")
                        {
                            TempData["success"] = $"Email Đã được gửi đên {user.Email} của bạn";
                        }
                        else
                        {
                            TempData["error"] = $"Gủi mail thất bại\nVui lòng nhấn gửi lại để nhận mã xác minh";
                        }
                        return RedirectToAction("Confirm", "Auth", new { id = user.EUserId });
                    }
                    if (user.EUserRole == "Admin")
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Email),
                            new Claim(ClaimTypes.Name,user.Email),
                            new Claim(ClaimTypes.Role,user.EUserRole),
                            new Claim("UserId",user.EUserId.ToString())
                        };
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = login.KeepLogIn,
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                      new ClaimsPrincipal(claimsIdentity), authProperties);
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Auth");
                    }

                    TempData["success"] = "Đăng nhập thành công";
                    return RedirectToAction("Index", "EUser");
                }
            }
            TempData["error"] = "Đăng nhập không thành công,\nEmail hoặc mật khẩu không đúng";
            return View();

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
