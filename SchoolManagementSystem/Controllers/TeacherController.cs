using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.Services.Teacher;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.Teacher;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserService userService;
        private readonly TeacherService teacherService;

        public TeacherController(AppDbContext context, UserService userService, TeacherService teacherService)
        {
            this.context = context;
            this.userService = userService;
            this.teacherService = teacherService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }
        [HttpGet]
        public async Task<IActionResult> Notes() 
        {
            var note=await teacherService.GetNoteToTeacherAsync(GetUserId());
            return View(note);
        }



        public async Task<IActionResult> Schedule()
        {
            
            var model = await teacherService.GetTimeTableAsync(GetUserId());
            return View(model);
        }
        #region Assignments

        public async Task<IActionResult> Assignments(int? levelid, int? stadgeid, int? classid, int? subjectid, string? status)
        {
            var teacheruserId = GetUserId();
            var dashboardVM = await teacherService.GetTeacherAssignmentsDashboard(teacheruserId, levelid, stadgeid, classid, subjectid, status);
            return View(dashboardVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var result = await teacherService.DeleteAssignmentAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "تم حذف الواجب بنجاح." });
            }
            return Json(new { success = false, message = "حدث خطأ أثناء محاولة الحذف." });
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "الرجاء ملء جميع الحقول بشكل صحيح");
                return View("Assignments", model);
            }

            var result = await teacherService.AddAssignmentAsync(model, GetUserId());

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error);
                return View("Assignments", model);
            }

            return RedirectToAction("Assignments");
        }



        [HttpGet]
        public async Task<IActionResult> AddAssignment()
        {
            var teacheruserId = GetUserId();
            var teacherId = await teacherService.GetTeacherIdAsync(teacheruserId);
            var subjects = await teacherService.GetTeacherSubjectsAsync(teacherId);
            var clesses = await teacherService.GetTeacherClassesAsync(teacherId);
            var model = new AddAssignmentViewModel
            {
                Subjects = subjects,
                Classes = clesses

            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditAssignment(int id)
        {
            var assignment = await teacherService.GetAssignmentAsync(id);

            return View(assignment);
        }

        [HttpPost]
        public async Task<IActionResult> EditAssignment(EditAssignmentViewModel model)
        {

            await teacherService.UpdateAssignmentAsync(model);

            return RedirectToAction("Assignments");
        }

        public async Task<IActionResult> AssignmentSubmissions(int id)
        {
            var model = await teacherService.GetAllSubmitionForHomewoekAsync(id);
            return View(model);
        }

        #endregion


        #region Students
        [HttpGet]
        public async Task<IActionResult> Students(int? levelId, int? stageId, int? classId)
        {
            var teacheruserId = GetUserId();
            var studentsPageData = await teacherService.GetTeacherStudentsAsync(teacheruserId, levelId, stageId, classId);

            studentsPageData.SelectedLevelsId = levelId;
            studentsPageData.SelectedStageId = stageId;
            studentsPageData.SelectedClassId = classId;


            return View(studentsPageData);
        }



        [HttpGet]
        public async Task<IActionResult> GetAbsenceDates(int studentId)
        {
            int studentUserId = await teacherService.GetStudentUserId(studentId);

            if (studentUserId == 0) return Json(new List<string>());

            var dates = await teacherService.GetStudentAbsenceDates(studentUserId);

            return Json(dates);
        }

        #endregion



        #region Setting
        public async Task<IActionResult> Setting()
        {
            
            SettingViewModel settingViewModel = new SettingViewModel
            {
                NavigationInfo =await teacherService.GetNavigationDataAsync(GetUserId())
            };
            return View(settingViewModel);
        }

        [HttpPost]
        public async Task <IActionResult> Setting(SettingViewModel model)
        {

            model.NavigationInfo = await teacherService.GetNavigationDataAsync(GetUserId());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "الباسورد لازم يكون 8 حروف على الأقل");
                return View(model);

            }

            var userId = GetUserId();
            var user = context.Users.FirstOrDefault(u => u.ID == userId);

            if (user == null)
                return NotFound();

            bool isPasswordCorrect =await userService.Setting(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!isPasswordCorrect)
            {
                ModelState.AddModelError("", "كلمة المرور الحالية غير صحيحة");
                return View(model);
            }

            TempData["Success"] = "تم تحديث كلمة المرور بنجاح";
            return RedirectToAction("Setting");
        }
        #endregion

        public async Task <IActionResult> Teacherdashboard()
        {
            var model =await teacherService.GetTeacherDashboard(GetUserId());
            return View(model);
        }


        #region Exams

        [HttpGet]
        public async Task<IActionResult> EditExam(int id)
        {


            var exam = await context.MultiChoiceExam
                .Include(e => e.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (exam.ActualDate <= DateTime.Now)
            {

                TempData["ErrorMessage"] = "عذراً، لا يمكن تعديل هذا الاختبار لأن وقته الفعلي قد بدأ بالفعل!";

                return RedirectToAction("Exams");
            }
            var viewModel = await teacherService.GetExamForEditAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExam(EditExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var teacherId =await teacherService.GetTeacherIdAsync(GetUserId());
                model.Subjects = await teacherService.GetTeacherSubjectsAsync(teacherId);
                model.Classes = await teacherService.GetTeacherClassesAsync(teacherId);
                return View(model);
            }

            var result = await teacherService.UpdateExamAsync(model);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("ExamDetails", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ExamDetails(int id)
        {
            var viewModel = await teacherService.GetExamDetailsAsync(id); 
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExam(AddExamViewModel model)
        {
            var teacheruserId = GetUserId();
            var teacherId = await teacherService.GetTeacherIdAsync(teacheruserId);
            if (!ModelState.IsValid)
            {
                model.Subjects = await teacherService.GetTeacherSubjectsAsync(teacherId);
                model.Classes = await teacherService.GetTeacherClassesAsync(teacherId);
                return View(model);
            }
            try
            {
                await teacherService.AddExamWithQuestions(model, teacheruserId);
                TempData["SuccessMessage"] = "تم إضافة الاختبار بنجاح!";
                return RedirectToAction("Exams");
            }
            catch (Exception ex)
            {
                // var innerError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                model.Subjects = await teacherService.GetTeacherSubjectsAsync(teacherId);
                model.Classes = await teacherService.GetTeacherClassesAsync(teacherId);

                ModelState.AddModelError("", "خطأ تقني: " /*+ innerError*/);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddExam()
        {
            var teacherId = GetUserId();
            var model = await teacherService.GetAddExamViewModel(teacherId);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Exams(int? levelid, int? stageid, int? classid, int? subjectid, string? status,string?type)
        {
            var teacheruserId = GetUserId();
            var dashboardVM = await teacherService.GetTeacherExamsDashboard(teacheruserId,levelid,stageid,classid,subjectid,status,type);
            return View(dashboardVM);


        }



        public async Task< IActionResult> ExamGrades(int id)
        {
            var grades =await teacherService.GetExamGradesAsync(id);
           
            return View(grades);
        }

       
        [HttpPost]
        public async Task<IActionResult> DeleteExam(int id)
        {
            bool isDeleted = await teacherService.DeleteExamAsync(id);

            if (isDeleted)
            {
                return Json(new { success = true, message = "تم حذف الاختبار بنجاح." });
            }
            else
            {
                return Json(new { success = false, message = "حدث خطأ، إما أن الاختبار غير موجود أو مرتبطة به بيانات أخرى كالإجابات والدرجات." });
            }
        }

        #endregion

        #region Notes

        [HttpGet]
        public async Task<JsonResult> GetLevelsByStage(int stageId)
        {
            var filteredLevels = await context.Levels
                .Where(l => l.StageID == stageId) 
                .Select(l => new { id = l.ID, name = l.Name })
                .ToListAsync();

            return Json(filteredLevels);
        }

        [HttpGet]
        public async Task<JsonResult> GetClassesByLevel(int levelId)
        {
            if (levelId <= 0)
            {
                return Json(new List<object>());
            }

            var filteredClasses = await context.Classes
                .Where(c => c.LevelID == levelId)
                .Select(c => new { id = c.ID, name = c.Name })
                .ToListAsync();

            return Json(filteredClasses);
        }
        [HttpGet]
    
        public async Task<IActionResult> Notesstudents(int? levelid, int? classid, int? stageid)
        {
            var teacheruserId = GetUserId();

            var notesPageData = await teacherService.GetTeacherNotesAsync(teacheruserId,  stageid, levelid, classid);

            return View(notesPageData);
        }

        [HttpGet]
        public async Task<IActionResult> AddStudentNote(int id)
        {
            var viewModel = new AddNoteViewModel
            {
                StudentId = id
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentNote(AddNoteViewModel model)
        {

            var teacheruserId = GetUserId();

            var added = await teacherService.AddNoteAsync(teacheruserId, model);
            return RedirectToAction("NotesStudents");
        }


        [HttpGet]
        public async Task<IActionResult> GetStudentNotesPartial(int id)
        {
            int teacherId = GetUserId();

            var model = await teacherService.GetNoteAsync(teacherId, id);

            if (model == null)
            {
                model = new AllNotesForStudent { StudentName = "غير معروف", Notes = new List<NoteItem>() };
            }

            return PartialView("_StudentNotesPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                bool isDeleted = await teacherService.DeleteNoteAsync(id);

                if (isDeleted)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "لم يتم العثور على الملاحظة أو فشل الحذف." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ غير متوقع في الخادم." });
            }
        }
        #endregion

        public async Task<IActionResult> Grades(int? subjectid, int? levelid,int? stageid, int? classid  )
        {
            var teacheruserId = GetUserId();
            var exams = await teacherService.GetTeacherGrades(teacheruserId, subjectid, levelid,stageid, classid);
            return View(exams);
        }

       


   
    }

}
