using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.ViewModel;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Home/Login.cshtml", model);

            var user = _context.Users
                .FirstOrDefault(u => u.SSN == model.SSN);

            if (user == null || !clsBCrypt.VerifyPassword(model.Password, user.Password))
            {
                ModelState.AddModelError("", "كلمة المرور أو الرقم القومي خطأ");
                return View("~/Views/Home/Login.cshtml", model);
            }

            var userType = _context.UserUserTypes
                .FirstOrDefault(u => u.UserId == user.ID && u.UserTypeId == model.UserType);

            if (userType == null)
            {
                ModelState.AddModelError("", "ليس مسموح لك بالدخول بهذا التخصص");
                return View("~/Views/Home/Login.cshtml", model);
            }
            var u = _context.UserTypes
                .FirstOrDefault(u => u.ID ==userType.UserTypeId );
            //Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Role, ((UserTypeEnum)model.UserType).ToString()),
                new Claim("UserTypeId", model.UserType.ToString())
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            // SignIn using correct scheme
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            //RedirectToAction based on UserType
            switch ((UserTypeEnum)model.UserType)
            {
                case UserTypeEnum.Supervisor:
                    return RedirectToAction("SupervisorProfile", "Supervisor");

                case UserTypeEnum.Headmaster:
                    return RedirectToAction("Home", "SchoolManager");

                case UserTypeEnum.Student:
                    return RedirectToAction("Index", "Student");

                case UserTypeEnum.Teacher:
                    return RedirectToAction("Teacherdashboard", "Teacher");

                case UserTypeEnum.Parent:
                    return RedirectToAction("index", "Parent");

                case UserTypeEnum.Owner:
                    return RedirectToAction("Dashboard", "Owner");

                case UserTypeEnum.IT:
                    return RedirectToAction("Home", "ITManager");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index","Home");
        }
    }
}
