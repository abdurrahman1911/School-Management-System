using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SchoolManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        // In-memory token store: Token -> (UserId, ExpiryTime)
        private static readonly ConcurrentDictionary<string, (int UserId, DateTime Expiry)> _resetTokens = new();

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // GET: /Home/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("forgot_password");
        }

        // POST: /Home/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("forgot_password", model);
            }

            // Find user by SSN and Email
            var user = _context.Users
                .FirstOrDefault(u => u.SSN == model.SSN && u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "البيانات المدخلة غير صحيحة. تأكد من الرقم القومي والبريد الإلكتروني.");
                return View("forgot_password", model);
            }

            // Generate a secure token
            var token = Guid.NewGuid().ToString("N");

            // Clean expired tokens
            var expiredTokens = _resetTokens.Where(t => t.Value.Expiry < DateTime.UtcNow).Select(t => t.Key).ToList();
            foreach (var expiredToken in expiredTokens)
            {
                _resetTokens.TryRemove(expiredToken, out _);
            }

            // Store token with 15-minute expiry
            _resetTokens[token] = (user.ID, DateTime.UtcNow.AddMinutes(15));

            // Redirect to reset password page with token
            return RedirectToAction("ResetPassword", new { token });
        }

        // GET: /Home/ResetPassword?token=xxx
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token) || !_resetTokens.ContainsKey(token))
            {
                TempData["ErrorMessage"] = "رابط إعادة تعيين كلمة المرور غير صالح أو منتهي الصلاحية.";
                return RedirectToAction("ForgotPassword");
            }

            var tokenData = _resetTokens[token];
            if (tokenData.Expiry < DateTime.UtcNow)
            {
                _resetTokens.TryRemove(token, out _);
                TempData["ErrorMessage"] = "انتهت صلاحية رابط إعادة تعيين كلمة المرور. الرجاء المحاولة مرة أخرى.";
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordViewModel { Token = token };
            return View("reset_password", model);
        }

        // POST: /Home/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("reset_password", model);
            }

            // Validate token
            if (!_resetTokens.TryGetValue(model.Token, out var tokenData))
            {
                TempData["ErrorMessage"] = "رابط إعادة تعيين كلمة المرور غير صالح.";
                return RedirectToAction("ForgotPassword");
            }

            if (tokenData.Expiry < DateTime.UtcNow)
            {
                _resetTokens.TryRemove(model.Token, out _);
                TempData["ErrorMessage"] = "انتهت صلاحية رابط إعادة تعيين كلمة المرور.";
                return RedirectToAction("ForgotPassword");
            }

            // Find user and update password
            var user = _context.Users.Find(tokenData.UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "حدث خطأ غير متوقع. الرجاء المحاولة مرة أخرى.";
                return RedirectToAction("ForgotPassword");
            }

            // Hash and save new password
            user.Password = clsBCrypt.GetHash(model.NewPassword);
            _context.Users.Update(user);
            _context.SaveChanges();

            // Remove used token
            _resetTokens.TryRemove(model.Token, out _);

            TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح! يمكنك الآن تسجيل الدخول.";
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
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
