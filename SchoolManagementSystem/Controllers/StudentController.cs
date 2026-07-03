using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Controllers
{
    //[Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly AppDbContext _context;

        public StudentController(StudentService studentService, AppDbContext context, UserService userService)
        {
            _studentService = studentService;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetStudentIndexData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }



        public IActionResult Schedule()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetScheduleData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Grades()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetGradesData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Attendance()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetAttendanceData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Tests()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetTestsData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult TakeExam(int examId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetExamForTaking(userId, examId);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "لا يمكن الوصول لهذا الاختبار. قد يكون غير متاح حالياً أو تم حله مسبقاً.";
                return RedirectToAction("Tests");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitExam(int examId, IFormCollection form)
        {
            try 
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Parse answers from form: answer_questionId = answerId
                var answers = new Dictionary<int, int>();
                foreach (var key in form.Keys)
                {
                    if (key.StartsWith("answer_"))
                    {
                        var questionIdStr = key.Replace("answer_", "");
                        if (int.TryParse(questionIdStr, out int questionId))
                        {
                            var valStr = form[key].ToString();
                            if (int.TryParse(valStr, out int answerId))
                            {
                                answers[questionId] = answerId;
                            }
                        }
                    }
                }

                var result = _studentService.SubmitExam(userId, examId, answers);

                if (result == null)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ الإجابات. تأكد من أنك لم تقم بحل الاختبار مسبقاً.";
                    return RedirectToAction("Tests");
                }

                return View("ExamResult", result);
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                return Content($" حدث خطأ أثناء الحفظ:\n{ex.Message}\n{innerMsg}");
            }
        }



        public IActionResult ExamReview(int examId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetExamReview(userId, examId);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "لا يمكن مراجعة هذا الاختبار. قد يكون الاختبار لم ينتهِ بعد أو لم تقم بحله.";
                return RedirectToAction("Tests");
            }

            return View(viewModel);
        }

        public IActionResult Level()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetLevelData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Assignments()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetAssignmentsData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(int homeworkId, IFormFile submissionFile, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (submissionFile == null || submissionFile.Length == 0)
            {
                TempData["ErrorMessage"] = "يرجى اختيار ملف لرفعه.";
                return RedirectToAction("Assignments");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var result = await _studentService.SubmitAssignmentAsync(userId, homeworkId, submissionFile, webHostEnvironment.WebRootPath);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Assignments");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetEditProfileData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _studentService.GetEditProfileData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                viewModel.ErrorMessage = "الرجاء ملء جميع الحقول المطلوبة";
                return View(viewModel);
            }

            if (newPassword != confirmPassword)
            {
                viewModel.ErrorMessage = "كلمة المرور الجديدة غير متطابقة مع التأكيد";
                return View(viewModel);
            }

            if (newPassword.Length < 6)
            {
                viewModel.ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل";
                return View(viewModel);
            }

            bool success = _studentService.ChangePassword(userId, currentPassword, newPassword);
            if (success)
            {
                viewModel.SuccessMessage = "تم تغيير كلمة المرور بنجاح";
            }
            else
            {
                viewModel.ErrorMessage = "كلمة المرور الحالية غير صحيحة";
            }

            return View(viewModel);
        }

    }
}
