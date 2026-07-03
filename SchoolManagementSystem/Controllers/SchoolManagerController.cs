using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models.ViewModels;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.Services.Teacher;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.SchoolManager;
using SchoolManagementSystem.ViewModel.Supervisor;
using System.Security.Claims;
using static Azure.Core.HttpHeader;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Headmaster")]
    public class SchoolManagerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;

        public SchoolManagerController(AppDbContext context,UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #region Helper Methods
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }
        #endregion

        #region Supervisors Page
        [HttpGet]
        public async Task<IActionResult> Supervisors()
        {
            var currentUserId = GetUserId();
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.ID == currentUserId);

            string currentManagerName = manager.FullName;
            string currentManagerPhoto = manager?.ProfilPhotoURL ;

            var supervisorsData = await _context.Supervisors
                .Include(s => s.User)
                .Select(s => new SupervisorCardViewModel
                {
                    SupervisorId = s.ID,
                    UserId = s.UserId,
                    Name = s.User != null ? s.User.FullName: "مشرف غير مسجل",
                    Email = s.User != null ? s.User.Email : "لا يوجد إيميل",
                    Phone = s.User != null ? s.User.Phone : "غير مسجل",
                    PhotoUrl =  s.User.ProfilPhotoURL ,
                    HireDate = s.HireDate.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            var viewModel = new SupervisorsPageViewModel
            {
                ManagerName = currentManagerName,
                ManagerPhotoUrl = currentManagerPhoto,
                TotalSupervisorsCount = supervisorsData.Count,  
                Supervisors = supervisorsData
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupervisorNote(int targetUserId, string noteText)
        {
            if (string.IsNullOrEmpty(noteText))
                return Json(new { success = false, message = "لا يمكن إضافة ملاحظة فارغة!" });

            var currentManagerUserId = GetUserId();

            var note = new Note
            {
                WriterUserId = currentManagerUserId,
                TargetUserId = targetUserId,
                NoteDetails = noteText,
                AddedDate = DateTime.Now
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم إضافة الملاحظة بنجاح!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetSupervisorNotes(int targetUserId)
        {
            var notes = await _context.Notes
                .Where(n => n.TargetUserId == targetUserId && n.WriterUserId == GetUserId())
                .OrderByDescending(n => n.AddedDate)
                .Select(n => new {
                    id = n.ID,
                    text = n.NoteDetails,
                    date = n.AddedDate.ToString("yyyy-MM-dd hh:mm tt")
                })
                .ToListAsync();

            return Json(notes);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSupervisorNote(int noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note == null)
                return Json(new { success = false, message = "الملاحظة غير موجودة!" });

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم حذف الملاحظة بنجاح!" });
        }
        #endregion

        #region Teachers Page
        [HttpGet]
        public async Task<IActionResult> Teachers()
        {
            var currentUserId = GetUserId();
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.ID == currentUserId);

            string currentManagerName = manager.FullName;
            string currentManagerPhoto = manager?.ProfilPhotoURL;

            var teachersData = await _context.Teachers
                .Include(t => t.User)
                .Select(t => new TeacherCardViewModel
                {
                    TeacherId = t.ID,
                    UserId = t.UserId,
                    Name = t.User != null ? t.User.FullName : "معلم غير مسجل",
                    Email = t.User != null ? t.User.Email : "لا يوجد إيميل",
                    Phone = t.User != null ? t.User.Phone : "غير مسجل",
                    PhotoUrl = t.User.ProfilPhotoURL ,
                    HireDate = t.HireDate.ToString("yyyy-MM-dd"),
                    TeacherScheduleUrl = _context.TeacherTimeTables
                        .Where(tt => tt.TeacherId == t.ID)
                        .Select(tt => tt.PhotoLink)
                        .FirstOrDefault() ?? "",
                    Subjects = _context.TeacherSubjects
                        .Where(ts => ts.TeacherId == t.ID)
                        .Select(ts => ts.Subject.Name)
                        .Distinct()
                        .ToList(),
                    Classes = _context.StudentsSubjectsEnrollments
                        .Where(sse => sse.TeacherId == t.ID)
                        .Join(_context.StudentClassEnrollments,
                              sse => sse.StudentId,
                              sce => sce.StudentId,
                              (sse, sce) => sce.Class.Name)
                        .Distinct()
                        .ToList()
                })
                .ToListAsync();

            var viewModel = new TeachersPageViewModel
            {
                ManagerName = currentManagerName,
                ManagerPhotoUrl = currentManagerPhoto,
                TotalTeachersCount = teachersData.Count,
                Teachers = teachersData
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(int targetUserId, string noteText)
        {
            if (string.IsNullOrEmpty(noteText))
                return Json(new { success = false, message = "لا يمكن إضافة ملاحظة فارغة!" });

            var currentManagerUserId = GetUserId();

            var note = new Note
            {
                WriterUserId = currentManagerUserId,
                TargetUserId = targetUserId,
                NoteDetails = noteText,
                AddedDate = DateTime.Now
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم إضافة الملاحظة بنجاح!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes(int targetUserId)
        {
            var notes = await _context.Notes
                .Where(n => n.TargetUserId == targetUserId && n.WriterUserId == GetUserId())
                .OrderByDescending(n => n.AddedDate)
                .Select(n => new {
                    id = n.ID,
                    text = n.NoteDetails,
                    date = n.AddedDate.ToString("yyyy-MM-dd hh:mm tt")
                })
                .ToListAsync();

            return Json(notes);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note == null)
                return Json(new { success = false, message = "الملاحظة غير موجودة!" });

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم حذف الملاحظة بنجاح!" });
        }
        #endregion

        #region Students Page
        [HttpGet]
        public async Task<IActionResult> Students()
        {
            var currentUserId = GetUserId();
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.ID == currentUserId);

            var stagesData = await _context.Stages.OrderBy(s => s.Order).Select(s => new StageSelectionViewModel
            {
                Id = s.ID,
                Name = s.Name,
                Levels = _context.Levels.Where(l => l.StageID == s.ID).OrderBy(l => l.Order).Select(l => new LevelSelectionViewModel
                {
                    Id = l.ID,
                    Name = l.Name,
                    Classes = _context.Classes.Where(c => c.LevelID == l.ID).Select(c => new ClassSelectionViewModel
                    {
                        Id = c.ID,
                        Name = c.Name
                    }).ToList()
                }).ToList()
            }).ToListAsync();

            var allStudents = await _context.Students.Include(s => s.User).Select(s => new StudentResultViewModel
            {
                Id = s.ID,
                Name = s.User.FullName
            }).ToListAsync();

            var allSubjects = await _context.Subjects
                .Select(e => new SubjectViewModel
                {
                    Id = e.ID,
                    Name = e.Name
                }).ToListAsync();

            var allExams = await _context.MultiChoiceExam.Select(e => new ExamResultViewModel
            {
                Id = e.ID,
                Name = e.Name
            }).ToListAsync();

            var initialGrades = await _context.StudentExamDegrees
                .Include(d => d.Student).ThenInclude(s => s.User)
                .Include(d => d.Exam)
                .Take(50)
                .Select(d => new GradeRowViewModel
                {
                    StudentName = d.Student.User.FullName,
                    ExamName = d.Exam.Name,
                    SubjectName = "المادة المسجلة",
                    Degree = d.Degree
                }).ToListAsync();

            for (int i = 0; i < initialGrades.Count; i++) { initialGrades[i].Index = i + 1; }

            var viewModel = new StudentsPageViewModel
            {
                ManagerName = manager.FullName,
                ManagerPhotoUrl = manager?.ProfilPhotoURL ,
                Stages = stagesData,
                AllStudents = allStudents,
                AllSubjects = allSubjects,
                AllExams = allExams,
                InitialGrades = initialGrades
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetClassDetails(int classId)
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Where(s => s.StudentClassEnrollments.Select(e => e.ClassId).First() == classId)
                .Select(s => new StudentResultViewModel
                {
                    Id = s.ID,
                    Name = s.User.FullName
                }).ToListAsync();

            var schedulePhoto = await _context.ClassTimeTable
                .Where(t => t.ClassId == classId)
                .Select(t => t.PhotoLink)
                .FirstOrDefaultAsync();

            return Json(new { students = students, scheduleUrl = schedulePhoto ?? "" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredGrades(int? studentId, int? examId, int? classId)
        {
            var query = _context.StudentExamDegrees
                .Include(d => d.Student).ThenInclude(s => s.User)
                .Include(d => d.Exam)
                .AsQueryable();

            if (studentId.HasValue && studentId > 0)
                query = query.Where(d => d.StudentId == studentId.Value);

            if (examId.HasValue && examId > 0)
                query = query.Where(d => d.ExamId == examId.Value);

            if (classId.HasValue && classId > 0)
                query = query.Where(d => d.Student.StudentClassEnrollments.Select(e => e.ClassId).First() == classId.Value);

            var result = await query.Select(d => new GradeRowViewModel
            {
                StudentName = d.Student.User.FullName,
                ExamName = d.Exam.Name,
                Degree = d.Degree
            }).ToListAsync();

            for (int i = 0; i < result.Count; i++) { result[i].Index = i + 1; }

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceData(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return Json(new { attendance = 100, absence = 0 });

            var absenceCount = await _context.Absences.Where(a => a.UserId == student.UserId).CountAsync();
            int totalDays = 180;
            int attendanceCount = Math.Max(0, totalDays - absenceCount);

            return Json(new { attendance = attendanceCount, absence = absenceCount });
        }
        #endregion

        #region Attendance Page
        [HttpGet]
        public async Task<IActionResult> Attendance()
        {
            var currentUserId = GetUserId();
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.ID == currentUserId);

            string currentManagerName = manager.FullName;
            string currentManagerPhoto = manager?.ProfilPhotoURL;

            var stages = await _context.Stages
                .OrderBy(s => s.Order)
                .Select(s => new StageAttendanceResultViewModel
                {
                    Id = s.ID,
                    Name = s.Name
                })
                .ToListAsync();

           var navigationInfo = new NavigationViewModel
           {
               FullName = currentManagerName,
               ProfilePhotoUrl = currentManagerPhoto
           };
            var viewModel = new AttendancePageViewModel
            {
               NavigationInfo = navigationInfo,
                Stages = stages
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceChartData(int stageId, string period)
        {
            DateTime startDate = DateTime.Now;
            int totalWorkingDays = 30; 

            if (period == "week")
            {
                startDate = DateTime.Now.AddDays(-7);
                totalWorkingDays = 5; 
            }
            else if (period == "month")
            {
                startDate = DateTime.Now.AddDays(-30);
                totalWorkingDays = 22; 
            }

            var studentUserIds = await _context.Levels
                .Where(l => l.StageID == stageId)
                .Join(_context.Classes, l => l.ID, c => c.LevelID, (l, c) => c.ID)
                .Join(_context.StudentClassEnrollments, classId => classId, sce => sce.ClassId, (classId, sce) => sce.StudentId)
                .Join(_context.Students, studentId => studentId, s => s.ID, (studentId, s) => s.UserId)
                .Distinct()
                .ToListAsync();

            if (!studentUserIds.Any())
            {
                return Json(new { present = 100, absent = 0 });
            }

            int totalStudents = studentUserIds.Count;
            int totalPossibleAttendanceDays = totalStudents * totalWorkingDays;

            int totalAbsentDays = await _context.Absences
                .Where(a => studentUserIds.Contains(a.UserId) && a.AbsenceDate >= startDate)
                .CountAsync();

            int totalPresentDays = Math.Max(0, totalPossibleAttendanceDays - totalAbsentDays);

            double presentPercentage = Math.Round(((double)totalPresentDays / totalPossibleAttendanceDays) * 100, 1);
            double absentPercentage = Math.Round(((double)totalAbsentDays / totalPossibleAttendanceDays) * 100, 1);

            return Json(new
            {
                present = presentPercentage,
                absent = absentPercentage
            });
        }
        #endregion

        #region Setting
        public async Task<IActionResult> EditProfile()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == GetUserId());
            SettingViewModel settingViewModel = new SettingViewModel
            {

                NavigationInfo = new NavigationViewModel
                {
                    FullName = user.FullName ,
                    ProfilePhotoUrl = user?.ProfilPhotoURL 
                }
            };
            return View(settingViewModel);
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
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);

            if (user == null)
                return NotFound();

            bool isPasswordCorrect = await _userService.Setting(
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
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var currentUserId = GetUserId();
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.ID == currentUserId);

            if (manager == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                FullName = manager.FullName,
                Email = manager.Email ?? "لا يوجد بريد إلكتروني",
                Phone = manager.Phone ?? "غير مسجل",
                AddedDate = manager.AddedDate, 
                PhotoUrl =  manager.ProfilPhotoURL 
            };

            return View(viewModel);
        }

        #region SuccessFailure Page
        [HttpGet]
        public async Task<IActionResult> SuccessFailure()
        {
            var currentUserId = GetUserId();
            var manager = await _context.Users.FirstOrDefaultAsync(u => u.ID == currentUserId);

            string currentManagerName = manager.FullName;
            string currentManagerPhoto = manager?.ProfilPhotoURL;

            var stages = await _context.Stages
                .OrderBy(s => s.Order)
                .Select(s => new StageResultViewModel
                {
                    Id = s.ID,
                    Name = s.Name
                })
                .ToListAsync();

            var viewModel = new SuccessFailurePageViewModel
            {
                ManagerName = currentManagerName,
                ManagerPhotoUrl = currentManagerPhoto,
                Stages = stages
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuccessFailureData(int stageId, string period)
        {
            var levels = await _context.Levels
                .Where(l => l.StageID == stageId)
                .OrderBy(l => l.Order)
                .ToListAsync();

            var labels = new List<string>();
            var successRates = new List<double>();
            var failRates = new List<double>();

            var examQuery = _context.MultiChoiceExam.AsQueryable();
            if (period == "month")
            {
                examQuery = examQuery.Where(e => e.AddedDate >= DateTime.Now.AddDays(-30));
            }
            else if (period == "year")
            {
                examQuery = examQuery.Where(e => e.AddedDate >= DateTime.Now.AddDays(-365));
            }

            foreach (var level in levels)
            {
                labels.Add(level.Name);

                var degrees = await _context.StudentExamDegrees
                    .Include(d => d.Exam)
                    .Where(d => d.Exam.Class.LevelID == level.ID) 
                    .ToListAsync();

                if (degrees.Count == 0)
                {
                    successRates.Add(0);
                    failRates.Add(0);
                    continue;
                }

                int totalStudents = degrees.Count;
                int passedStudents = 0;

                foreach (var degree in degrees)
                {
                    decimal passingGrade = degree.Exam.TotalDegree / 2;
                    if (degree.Degree >= passingGrade)
                    {
                        passedStudents++;
                    }
                }

                int failedStudents = totalStudents - passedStudents;

                double successPercentage = Math.Round(((double)passedStudents / totalStudents) * 100, 1);
                double failPercentage = Math.Round(((double)failedStudents / totalStudents) * 100, 1);

                successRates.Add(successPercentage);
                failRates.Add(failPercentage);
            }

            return Json(new
            {
                labels = labels,
                success = successRates,
                fail = failRates
            });
        }
        #endregion
    }
}