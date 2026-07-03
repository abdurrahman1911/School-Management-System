using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.Services.Superbisor;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.Supervisor;
using SchoolManagementSystem.ViewModel.Teacher;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{


    [Authorize(Roles = "Supervisor")]

    public class SupervisorController : Controller
    {
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }
        private readonly AppDbContext context;
        private readonly UserService userService;
        private readonly SupervisorService supervisorService;
        private readonly IWebHostEnvironment webHostEnvironment;


        public SupervisorController(AppDbContext context, UserService userService, SupervisorService supervisorService, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.userService = userService;
            this.supervisorService = supervisorService;
            this.webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> Notes()
        {   
            var teachers = await supervisorService.GetAllTeachersIdAndFullName();
            var model = new AddNotesViewModel
            {
                Teachers = teachers ?? new List<IdNameViewModel>(),
                NavigationInfo = await supervisorService.GetNavigationDataAsync(GetUserId())

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Notes(AddNotesViewModel model)
        {
            var supervisoruserId = GetUserId();
            var teachers = await supervisorService.GetAllTeachersIdAndFullName();

            if (!ModelState.IsValid)
            {
                model.Teachers = teachers;
                return View(model);
            }

            var result = await supervisorService.AddNoteAsync(supervisoruserId, model);

            if (result)
            {
                ModelState.Clear();

                model = new AddNotesViewModel();

                ViewBag.Message = "تم حفظ الملاحظة بنجاح!";
            }
            else
            {
                ViewBag.Error = "حدث خطأ أثناء حفظ الملاحظة.";
            }

            model.Teachers = teachers;
            model.NavigationInfo = await supervisorService.GetNavigationDataAsync(supervisoruserId);
            return View(model);
        }
        public IActionResult TeacherPerformance()
        {
            return View();
        }

        public async Task< IActionResult> SupervisorProfile()
        {
            var model= await supervisorService.GetProfileDataAsync(GetUserId());
            return View(model);
        }

        #region TeachersAbsence
        public async Task<IActionResult> TeachersAbsence(DateTime? date)
        {
           var model= await supervisorService.GetAbsencesByDate(date,GetUserId());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTeachersAbsence([FromBody] AbsenceViewModel model)
        {

           
            if (model.AbsenceDate.Date > DateTime.Now.Date)
            {
                return Json(new { success = false, message = "عفواً، لا يمكن تسجيل الغياب لتاريخ مستقبلي." });
            }
            
            if (ModelState.IsValid)
            {
                await supervisorService.SaveAbsence(model);
                return Json(new { success = true, message = "تم حفظ غياب المعلمين بنجاح" });
            }

            return Json(new { success = false, message = "حدث خطأ في البيانات المرسلة" });
        }

        #endregion

        #region StudentsAbsence
        public async Task<IActionResult> StudentsAbsence()
        {
            var stages = await supervisorService.GetStagesAsync();

            if (stages == null)
            {
                stages = new List<StageViewModel>();
            }

            ViewBag.Levels = new SelectList(stages, "Id", "Name");
            var navigationInfo = await supervisorService.GetNavigationDataAsync(GetUserId());
            var model = new AbsenceViewModel
            {
                NavigationInfo = navigationInfo
            };
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetGrades(int levelId)
        {
            var grades = await supervisorService.GetLevelsByStageId(levelId);
            return Json(grades.Select(g => new { id = g.ID, name = g.Name }));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentList(int gradeId, DateTime absenceDate)
        {
            // تمرير التاريخ المختار إلى الـ Service
            var students = await supervisorService.GetStudentsByGrade(gradeId, absenceDate);
            return PartialView("_StudentTablePartial", students);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAbsence([FromBody] AbsenceViewModel model)
        {
            if (model == null || model.Users == null || !model.Users.Any())
            {
                return Json(new { success = false, message = "عفواً، لا توجد بيانات طلاب لإرسالها." });
            }

            if (model.AbsenceDate.Date > DateTime.Now.Date)
            {
                return Json(new { success = false, message = "عفواً، لا يمكن تسجيل الغياب لتاريخ مستقبلي." });
            }

            // استدعاء الحفظ مباشرة وتجنب تعقيدات الـ ModelState الناتجة عن الـ NavigationInfo
            bool isSaved = await supervisorService.SaveAbsence(model);

            if (isSaved)
            {
                return Json(new { success = true, message = "تم حفظ الغياب بنجاح" });
            }

            return Json(new { success = false, message = "حدث خطأ في السيرفر أثناء محاولة حفظ البيانات في قاعدة البيانات." });
        }
        #endregion

        #region StudentTable


        [HttpGet]
        public async Task<IActionResult> StudentsTable()
        {
            var classes = await supervisorService.GetClassesAsync(null);
            var viewModel = new StudentTableViewModel
            {
                Classes = classes,
                NavigationInfo = await supervisorService.GetNavigationDataAsync(GetUserId())

            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentTable(int classId)
        {
            var imagePath = await supervisorService.GetStudentTablePath(classId);

            if (string.IsNullOrEmpty(imagePath))
            {
                return Content("");
            }
            return PartialView("_StudentTableImagePartial", imagePath);

        }

        [HttpPost]
        public async Task<IActionResult> UploadTable(int classId, IFormFile file)
        {
            var result = await supervisorService.UploadOrUpdateStudentTable(classId, file);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "حدث خطأ أثناء حفظ الملف" });
        }
        #endregion

        #region TeachersPage
        public async Task<IActionResult> TeachersPage()
        {
            var teachers = await supervisorService.GetAllTeacherWithSubject(GetUserId());
            return View(teachers);
        }

        #endregion

        #region StudentPage
        public async Task<IActionResult> StudentPage(int? stageId, int? classId)
        {
            var stages = await supervisorService.GetStagesAsync();
            var levels = await supervisorService.GetLevelAsync(stageId);


            var students = await supervisorService.GetAllStudents(stageId, classId);

            var model = new StudentPageViewModel
            {
                Stages = stages,
                Classes = levels,
                Students = students,
                SelectedStageId = stageId,
                SelectedLevelId = classId,
                NavigationInfo = await supervisorService.GetNavigationDataAsync(GetUserId())

            };

            return View(model);
        }
        #endregion

        #region Setting

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var navigation = await supervisorService.GetNavigationDataAsync(GetUserId());
            var model = new SettingViewModel
            {
                NavigationInfo = navigation
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "الباسورد لازم يكون 8 حروف على الأقل");
                return View(model);
            }

            var userId = GetUserId();

            var user = context.Users.FirstOrDefault(u => u.ID == userId);

            if (user == null)
                return NotFound();

            bool isPasswordCorrect = await userService.Setting(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!isPasswordCorrect)
            {
                ModelState.AddModelError("", "كلمة المرور الحالية غير صحيحة");
                model.NavigationInfo = await supervisorService.GetNavigationDataAsync(GetUserId());
                return View(model);
            }

            TempData["Success"] = "تم تحديث كلمة المرور بنجاح";
            return RedirectToAction("Setting");
        }

        #endregion

        #region TeachersTable
        [HttpGet]
        public async Task<IActionResult> TeachersTable()
        {
            var teachers = await supervisorService.GetAllTeachersIdAndFullName();
            var viewModel = new TeacherTableViewModel
            {
                Teachers = teachers,
                NavigationInfo = await supervisorService.GetNavigationDataAsync(GetUserId())
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherTable(int teacherId)
        {
            var imagePath = await supervisorService.GetTeacherTablePath(teacherId);

            if (string.IsNullOrEmpty(imagePath))
            {
                return Content("");
            }

            return PartialView("~/Views/Supervisor/_TeacherTableImagePartial.cshtml", imagePath);
        }

        [HttpPost]
        public async Task<IActionResult> UploadTeacherTable(int teacherId, IFormFile file)
        {
            var result = await supervisorService.UploadOrUpdateTeacherTable(teacherId, file);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "حدث خطأ أثناء حفظ الجدول" });
        }
        #endregion

        public async Task<IActionResult> ViewNotes()
        {
            var supervisoruserId = GetUserId();
            var notes = await supervisorService.GetNoteToSupervisorAsync(supervisoruserId);
            return View(notes);
        }
    }
}
