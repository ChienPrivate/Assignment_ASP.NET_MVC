using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PS27765_NguyenNgocChien_SD18317.Common;
using PS27765_NguyenNgocChien_SD18317.Data;
using PS27765_NguyenNgocChien_SD18317.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace PS27765_NguyenNgocChien_SD18317.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CommonMethods _methods;
        public AuthController(ApplicationDbContext db,CommonMethods methods)
        {
            _db = db;
            _methods = methods;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            if (ModelState.IsValid)
            {
                var user = _db.EUser.FirstOrDefault(u => u.Email == login.Email && _methods.Encryption(login.Password).ToUpper() == u.Pwd);
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
                    if (user.EUserRole == "Customer")
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
                        TempData["error"] = "Tài khoản của bạn không phải là tài khoản khách hàng.";
                        return View();
                    }

                    TempData["success"] = "Đăng nhập thành công";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Đăng nhập không thành công,\nEmail hoặc mật khẩu không đúng";
            return View();
            
        }

        public IActionResult Register() 
        {
            ViewData["newId"] = _methods.GenerateNewEUserId();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(EUser user,string pwd,string rePwd)
        {
            ViewData["newId"] = _methods.GenerateNewEUserId();
            string randomString;
            string sendMailNotify;
            string checkMailNotify = await _methods.CheckEmail(user.Email);
            var existUser = _db.EUser.FirstOrDefault(u => u.Email == user.Email);
            if (ModelState.IsValid)
            {
                if (checkMailNotify != "Email hợp lệ")
                {
                    ModelState.AddModelError("Email", checkMailNotify);
                    TempData["error"] = checkMailNotify;
                    return View();
                }
                if (existUser != null)
                {
                    ModelState.AddModelError("Email","Email này đã được sử dụng");
                    return View();
                }
                if (pwd != rePwd)
                {
                    ModelState.AddModelError("Pwd", "Mật khẩu và xác nhận mật khẩu không giống nhau");
                    return View();
                }
                user.Pwd = _methods.Encryption(user.Pwd).ToUpper();
                await _db.EUser.AddAsync(user);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetString("SecretKey", _methods.GenerateRandomString(8));
                randomString = HttpContext.Session.GetString("SecretKey");
                sendMailNotify = _methods.SendKeyToMail(user.Email, "Samsung Account Verify Key", randomString);
                if (sendMailNotify == "gửi mail thành công")
                {
                    TempData["success"] = $"Email Đã được gửi đên {user.Email} của bạn";
                }
                else
                {
                    TempData["error"] = $"Gủi mail thất bại\nVui lòng nhấn gửi lại để nhận mã xác minh";
                }
                return RedirectToAction("Confirm", "Auth",new {id = user.EUserId});
                
            }
            TempData["error"] = "Đăng ký tài khoản không thành công\nVui lòng kiểm tra lại thông tin tài khoản";
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Confirm(string id)
        {
            TempData["id"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(string id,string inputKey)
        {
            var user = _db.EUser.Find(id);
            if (user != null)
            {
                string? SessionKey = HttpContext.Session.GetString("SecretKey");
                if (inputKey == SessionKey)
                {
                    user.IsConfirm = true;
                    _db.EUser.Update(user);
                    _db.SaveChanges();
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
                        IsPersistent = false,
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                  new ClaimsPrincipal(claimsIdentity), authProperties);

                    var userCart = await _db.Cart.FirstOrDefaultAsync(c => c.EUserId == user.EUserId);

                    if (userCart == null)
                    {
                        Cart newCart = new Cart()
                        {
                            EUserId = user.EUserId,
                            CartId = _methods.GenerateNewCartId(),
                        };
                        _db.Cart.Add(newCart);
                        _db.SaveChanges();
                        return View();
                    }
                    TempData["success"] = "Tài Khoản của bạn đã được xác minh";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Mã xác minh không chính xác");
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult ResendMail()
        {
            string id = TempData["id"] as string;
            var user = _db.EUser.Find(id);
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

        [Authorize]
        public IActionResult Profile(string id)
        {
            var user = _db.EUser.Find(id);
            return View(user);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string id, string pwd, string newPwd,string reNewPwd )
        {
            if (pwd == null || newPwd == null || reNewPwd == null)
            {
                ModelState.AddModelError("Pwd", "Không thể để trống 1 trong 3 thông tin trên");
                return View();
            }
            var user = _db.EUser.Find(id);
            if (user != null)
            {
                bool match = user.Pwd == _methods.Encryption(pwd).ToUpper() ? true : false;
                if (match)
                {
                    if (newPwd == reNewPwd)
                    {
                        user.Pwd = _methods.Encryption(newPwd).ToUpper();
                        _db.EUser.Update(user);
                        _db.SaveChanges();
                        TempData["success"] = "Đổi mật khẩu thành công";
                        return RedirectToAction("Profile","Auth",new {id = user.EUserId});
                    }
                    else
                    {
                        ModelState.AddModelError("Pwd","Mật khẩu mới và xác nhận mật khẩu không trùng khớp");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("Pwd", "Mật khẩu cũ không chính xác");
                    return View();
                }
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            string validMail = await _methods.CheckEmail(email);
            if (validMail != "Email hợp lệ")
            {
                TempData["error"] = validMail;
                return View();
            }
            var user = _db.EUser.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                string randomNewPwd = _methods.GenerateRandomString(8);
                user.Pwd = _methods.Encryption(randomNewPwd).ToUpper();
                _db.EUser.Update(user);
                string sended = _methods.SendNewPasswordToMail(email, "Your New Password", randomNewPwd);
                if (sended != "gửi mail thành công")
                {
                    TempData["error"] = sended;
                    return View();
                }
                _db.SaveChanges();
                TempData["success"] = "Mật khẩu mới đã được gửi vào email của bạn";
                return RedirectToAction("Login","Auth");
            }
            TempData["error"] = "Email chưa được đăng ký hoặc liên kết với tài khoản của trang web";
            return View();
        }
    }
}
