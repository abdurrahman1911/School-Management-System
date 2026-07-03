using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using System.Transactions;
using SchoolManagementSystem.ViewModel.Parent;

namespace SchoolManagementSystem.Services
{
    public class ParentService
    {
        readonly AppDbContext _context;
        readonly UserService _userService;
        readonly UserTypeService _userTypeService;
        readonly StudentService _studentService;

        public ParentService(AppDbContext context, UserService userService, UserTypeService userTypeService, StudentService studentService)
        {
            _context = context;
            _userService = userService;
            _userTypeService = userTypeService;
            _studentService = studentService;
        }

        private int AddParent(ParentViewModel model, int userID)
        {
            Parent parent = new Parent
            {
                UserId = userID,
            };

            _context.Add(parent);
            _context.SaveChanges();

            return parent.ID;
        }

        public bool AddNewParent(ParentViewModel model)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    int userID = _userService.AddBaseUser(model, (byte)UserTypeEnum.Parent);

                    AddParent(model, userID);
                    _userTypeService.AddUserType(userID, (byte)UserTypeEnum.Parent);
                    _context.SaveChanges();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fetches all data needed for the Parent Index page:
        /// parent info, children with grades, and teacher contacts per child.
        /// </summary>
        public ParentIndexViewModel GetParentIndexData(int userId)
        {
            // 1. Find the Parent record and include User info
            var parent = _context.Parents
                .Include(p => p.User)
                .Include(p => p.Students)
                    .ThenInclude(s => s.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (parent == null)
                return null;

            var parentUser = parent.User;
            string fullName = string.Join(" ",
                new[] { parentUser.FirstName, parentUser.SecondName, parentUser.ThirdName, parentUser.LastName }
                .Where(n => !string.IsNullOrEmpty(n)));

            string firstLetter = !string.IsNullOrEmpty(parentUser.FirstName)
                ? parentUser.FirstName.Substring(0, 1)
                : "و";

            var viewModel = new ParentIndexViewModel
            {
                ParentFullName = fullName,
                ParentFirstLetter = firstLetter,
                Children = new List<ChildInfo>()
            };

            // 2. For each child (student), get grade info and teacher contacts
            if (parent.Students != null)
            {
                foreach (var student in parent.Students)
                {
                    // Get the latest class enrollment to determine current grade
                    var latestEnrollment = _context.StudentClassEnrollments
                        .Include(e => e.Class)
                            .ThenInclude(c => c.Level)
                        .Where(e => e.StudentId == student.ID)
                        .OrderByDescending(e => e.AcademicTermId)
                        .FirstOrDefault();

                    string gradeName = latestEnrollment?.Class?.Level?.Name ?? "";
                    string className = latestEnrollment?.Class?.Name ?? "";

                    // Get teacher contacts via StudentsSubjectsEnrollments
                    var teacherContacts = _context.StudentsSubjectsEnrollments
                        .Include(sse => sse.Teacher)
                            .ThenInclude(t => t.User)
                        .Include(sse => sse.Subject)
                        .Where(sse => sse.StudentId == student.ID)
                        .OrderByDescending(sse => sse.AcademicTermId)
                        .GroupBy(sse => new { sse.TeacherId, sse.SubjectId })
                        .Select(g => g.First())
                        .ToList()
                        .Select(sse => new TeacherContact
                        {
                            TeacherName = "أ/ " + sse.Teacher.User.FirstName + " " + sse.Teacher.User.LastName,
                            SubjectName = sse.Subject.Name,
                            Phone = sse.Teacher.User.Phone
                        })
                        .ToList();

                    string studentFullName = string.Join(" ",
                        new[] { student.User.FirstName, student.User.SecondName, student.User.LastName }
                        .Where(n => !string.IsNullOrEmpty(n)));

                    viewModel.Children.Add(new ChildInfo
                    {
                        StudentId = student.ID,
                        FullName = studentFullName,
                        GradeName = gradeName,
                        ClassName = className,
                        PhotoUrl = student.User.ProfilPhotoURL,
                        Gender = student.User.Gender,
                        Teachers = teacherContacts
                    });
                }
            }

            return viewModel;
        }

        /// <summary>
        /// Fetches all data needed for the Parent Performance page:
        /// parent info, children with KPI stats and recent grades.
        /// </summary>
        public ParentPerformanceViewModel GetPerformanceData(int userId)
        {
            // 1. Find the Parent record and include User + Students
            var parent = _context.Parents
                .Include(p => p.User)
                .Include(p => p.Students)
                    .ThenInclude(s => s.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (parent == null)
                return null;

            var parentUser = parent.User;
            string fullName = string.Join(" ",
                new[] { parentUser.FirstName, parentUser.SecondName, parentUser.ThirdName, parentUser.LastName }
                .Where(n => !string.IsNullOrEmpty(n)));

            string firstLetter = !string.IsNullOrEmpty(parentUser.FirstName)
                ? parentUser.FirstName.Substring(0, 1)
                : "و";

            var viewModel = new ParentPerformanceViewModel
            {
                ParentFullName = fullName,
                ParentFirstLetter = firstLetter,
                Children = new List<PerformanceChildInfo>()
            };

            // 2. For each child, compute KPIs and recent grades
            if (parent.Students != null)
            {
                foreach (var student in parent.Students)
                {
                    // Get current class enrollment
                    var latestEnrollment = _context.StudentClassEnrollments
                        .Include(e => e.Class)
                            .ThenInclude(c => c.Level)
                        .Include(e => e.AcademicTerm)
                        .Where(e => e.StudentId == student.ID)
                        .OrderByDescending(e => e.AcademicTermId)
                        .FirstOrDefault();

                    string gradeName = latestEnrollment?.Class?.Level?.Name ?? "";
                    int? currentClassId = latestEnrollment?.ClassId;

                    // --- Stages Percentage (for weighted average) ---
                    var examDegrees = _context.StudentExamDegrees
                        .Include(sed => sed.Exam)
                        .Where(sed => sed.StudentId == student.ID)
                        .ToList();

                    decimal gradesRate = 0;
                    if (examDegrees.Any())
                    {
                        decimal totalDegree = examDegrees.Sum(ed => ed.Degree);
                        decimal totalMax = examDegrees.Sum(ed => ed.Exam.TotalDegree);
                        if (totalMax > 0)
                            gradesRate = Math.Round((totalDegree / totalMax) * 100, 1);
                    }

                    // --- KPI 2: Attendance Rate % ---
                    decimal attendanceRate = 100;
                    if (latestEnrollment?.AcademicTerm != null)
                    {
                        var termStart = latestEnrollment.AcademicTerm.StartDate;
                        var today = DateTime.Now.Date;
                        // Count weekdays (school days) from term start to today
                        int totalSchoolDays = 0;
                        for (var d = termStart; d <= today; d = d.AddDays(1))
                        {
                            if (d.DayOfWeek != DayOfWeek.Friday && d.DayOfWeek != DayOfWeek.Saturday)
                                totalSchoolDays++;
                        }

                        int absenceDays = _context.Absences
                            .Count(a => a.UserId == student.UserId
                                     && a.AbsenceDate >= termStart
                                     && a.AbsenceDate <= today);

                        if (totalSchoolDays > 0)
                            attendanceRate = Math.Round(((decimal)(totalSchoolDays - absenceDays) / totalSchoolDays) * 100, 1);
                    }

                    // --- KPI 3: Homework Submission Rate % ---
                    decimal homeworkRate = 0;
                    if (currentClassId.HasValue)
                    {
                        int totalHomeworks = _context.Homeworks
                            .Count(h => h.ClassId == currentClassId.Value);

                        int submittedHomeworks = _context.StudentHomeworkAnswers
                            .Count(sha => sha.StudentId == student.ID
                                       && sha.Homework.ClassId == currentClassId.Value);

                        if (totalHomeworks > 0)
                            homeworkRate = Math.Round(((decimal)submittedHomeworks / totalHomeworks) * 100, 1);
                    }

                    // --- KPI 4: Upcoming MultiChoiceExam Count ---
                    int upcomingExams = 0;
                    if (currentClassId.HasValue)
                    {
                        //upcomingExams = _context.ClassTimeTable
                        //    .Count(e => e.ClassId == currentClassId.Value
                        //  د           && e.ActualDate > DateTime.Now);
                    }

                    // --- Overall Average (weighted: 50% grades + 25% attendance + 25% homework) ---
                    decimal overallAverage = Math.Round(
                        (gradesRate * 0.5m) + (attendanceRate * 0.25m) + (homeworkRate * 0.25m), 1);

                    // --- Recent Stages (last 5) ---
                    var recentGrades = _context.StudentExamDegrees
                        .Include(sed => sed.Exam)
                            .ThenInclude(e => e.Subject)
                        .Where(sed => sed.StudentId == student.ID)
                        .OrderByDescending(sed => sed.Exam.ActualDate)
                        .Take(5)
                        .ToList()
                        .Select(sed => new RecentGradeInfo
                        {
                            SubjectName = sed.Exam.Subject?.Name ?? "",
                            ExamName = sed.Exam.Name,
                            Score = $"{sed.Degree}/{sed.Exam.TotalDegree}",
                            Date = sed.Exam.ActualDate.ToString("yyyy-MM-dd")
                        })
                        .ToList();

                    string studentFullName = string.Join(" ",
                        new[] { student.User.FirstName, student.User.SecondName, student.User.LastName }
                        .Where(n => !string.IsNullOrEmpty(n)));

                    viewModel.Children.Add(new PerformanceChildInfo
                    {
                        StudentId = student.ID,
                        FullName = studentFullName,
                        GradeName = gradeName,
                        OverallAverage = overallAverage,
                        AttendanceRate = attendanceRate,
                        HomeworkSubmissionRate = homeworkRate,
                        UpcomingExamsCount = upcomingExams,
                        RecentGrades = recentGrades
                    });
                }
            }

            return viewModel;
        }

        public ParentGradesViewModel GetGradesData(int userId)
        {
            // 1. Find the Parent record and include User + Students
            var parent = _context.Parents
                .Include(p => p.User)
                .Include(p => p.Students)
                    .ThenInclude(s => s.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (parent == null)
                return null;

            var parentUser = parent.User;
            string fullName = string.Join(" ",
                new[] { parentUser.FirstName, parentUser.SecondName, parentUser.ThirdName, parentUser.LastName }
                .Where(n => !string.IsNullOrEmpty(n)));

            string firstLetter = !string.IsNullOrEmpty(parentUser.FirstName)
                ? parentUser.FirstName.Substring(0, 1)
                : "و";

            var viewModel = new ParentGradesViewModel
            {
                ParentFullName = fullName,
                ParentFirstLetter = firstLetter,
                Children = new List<GradeChildInfo>()
            };

            // 2. For each child, compute subject grades
            if (parent.Students != null)
            {
                foreach (var student in parent.Students)
                {
                    // Get current class enrollment
                    var latestEnrollment = _context.StudentClassEnrollments
                        .Include(e => e.Class)
                            .ThenInclude(c => c.Level)
                        .Where(e => e.StudentId == student.ID)
                        .OrderByDescending(e => e.AcademicTermId)
                        .FirstOrDefault();

                    string gradeName = latestEnrollment?.Class?.Level?.Name ?? "";

                    // Get all exams for this student
                    var allGrades = _context.StudentExamDegrees
                        .Include(sed => sed.Exam)
                            .ThenInclude(e => e.Subject)
                        .Where(sed => sed.StudentId == student.ID)
                        .OrderByDescending(sed => sed.Exam.ActualDate)
                        .ToList()
                        .Select(sed => new GradeDetailInfo
                        {
                            SubjectName = sed.Exam.Subject?.Name ?? "بدون مادة",
                            ExamName = sed.Exam.Name,
                            Score = $"{sed.Degree}/{sed.Exam.TotalDegree}",
                            Date = sed.Exam.ActualDate.ToString("yyyy-MM-dd")
                        })
                        .ToList();

                    string studentFullName = string.Join(" ",
                        new[] { student.User.FirstName, student.User.SecondName, student.User.LastName }
                        .Where(n => !string.IsNullOrEmpty(n)));

                    viewModel.Children.Add(new GradeChildInfo
                    {
                        StudentId = student.ID,
                        FullName = studentFullName,
                        GradeName = gradeName,
                        AllGrades = allGrades
                    });
                }
            }

            return viewModel;
        }

        public ParentScheduleViewModel GetScheduleData(int userId)
        {
            // 1. Find the Parent record and include User + Students
            var parent = _context.Parents
                .Include(p => p.User)
                .Include(p => p.Students)
                    .ThenInclude(s => s.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (parent == null)
                return null;

            var parentUser = parent.User;
            string fullName = string.Join(" ",
                new[] { parentUser.FirstName, parentUser.SecondName, parentUser.ThirdName, parentUser.LastName }
                .Where(n => !string.IsNullOrEmpty(n)));

            string firstLetter = !string.IsNullOrEmpty(parentUser.FirstName)
                ? parentUser.FirstName.Substring(0, 1)
                : "و";

            var viewModel = new ParentScheduleViewModel
            {
                ParentFullName = fullName,
                ParentFirstLetter = firstLetter,
                Children = new List<ChildScheduleInfo>()
            };

            // 2. For each child, compute schedule data
            if (parent.Students != null)
            {
                foreach (var student in parent.Students)
                {
                    var latestEnrollment = _context.StudentClassEnrollments
                        .Include(e => e.Class)
                            .ThenInclude(c => c.Level)
                        .Where(e => e.StudentId == student.ID)
                        .OrderByDescending(e => e.AcademicTermId)
                        .FirstOrDefault();

                    string gradeName = latestEnrollment?.Class?.Level?.Name ?? "";
                    int? currentClassId = latestEnrollment?.ClassId;

                    string schedulePhotoUrl = "";

                    if (currentClassId.HasValue)
                    {
                        var timeTable = _context.ClassTimeTable.FirstOrDefault(t => t.ClassId == currentClassId.Value);
                        if (timeTable != null)
                        {
                            schedulePhotoUrl = timeTable.PhotoLink;
                        }
                    }

                    string studentFullName = string.Join(" ",
                        new[] { student.User.FirstName, student.User.SecondName, student.User.LastName }
                        .Where(n => !string.IsNullOrEmpty(n)));

                    viewModel.Children.Add(new ChildScheduleInfo
                    {
                        StudentId = student.ID,
                        FullName = studentFullName,
                        GradeName = gradeName,
                        SchedulePhotoUrl = schedulePhotoUrl
                    });
                }
            }

            return viewModel;
        }


        public ChildrenAssignmentsViewModel GetChildrenAssignments(int ParentID)
        {
            var childrenUsersID = _context.Students.Where(s => s.parentId == ParentID).Select(s => s.UserId).ToList();
            ChildrenAssignmentsViewModel childrenAssignmentsViewModel = new ChildrenAssignmentsViewModel
            {
                ParentId = ParentID,
                ParentFullName = _userService.GetUserFullName(_context.Parents.FirstOrDefault(p => p.ID == ParentID).UserId),
                ChildrenAssignments = new List<ChildAssignmentsInfo>()
            };
            foreach (var userID in childrenUsersID)
            {
                int StudentId = _userService.GetStudentIDByUserID(userID);
                ChildAssignmentsInfo childAssignmentsInfo = new ChildAssignmentsInfo
                {
                    FullName = _userService.GetUserFullName(userID),
                    StudentId = StudentId,
                    Assignments = _studentService.GetAllAssignmentsOfCurrentTerm(StudentId)
                };

                childrenAssignmentsViewModel.ChildrenAssignments.Add(childAssignmentsInfo);

            }

            return childrenAssignmentsViewModel;
        }



        public List<SelectListItem> GetChildrenForDropdown(int parentId)
        {
            var result = _context.Students
                .Include(s => s.User)
                .Where(s => s.parentId == parentId)
                .ToList()
                .Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.User.FullName
                })
                .ToList();

            return result;
        }

        public ChildrenAbsenceViewModel GetChildrenAbsences(int ParentID)
        {

            var childrenUsersID = _context.Students.Where(s => s.parentId == ParentID).Select(s => s.UserId).ToList();

            // Get the last academic term to compute total school days
            var lastTerm = _context.AcademicTerms
                .OrderByDescending(t => t.StartDate)
                .FirstOrDefault();

            int totalSchoolDays = 0;
            if (lastTerm != null)
            {
                var termStart = lastTerm.StartDate;
                var endDate = lastTerm.EndDate < DateTime.Now.Date ? lastTerm.EndDate : DateTime.Now.Date;
                for (var d = termStart; d <= endDate; d = d.AddDays(1))
                {
                    if (d.DayOfWeek != DayOfWeek.Friday && d.DayOfWeek != DayOfWeek.Saturday)
                        totalSchoolDays++;
                }
            }

            ChildrenAbsenceViewModel childrenAbsenceViewModel = new ChildrenAbsenceViewModel
            {
                ParentId = ParentID,
                ParentFullName = _userService.GetUserFullName(_context.Parents.FirstOrDefault(p => p.ID == ParentID).UserId),
                ChildrenAbsences = new List<ChildAbsenceInfo>()
            };
            foreach (var userID in childrenUsersID)
            {
                ChildAbsenceInfo childAbsenceInfo = new ChildAbsenceInfo
                {
                    FullName = _userService.GetUserFullName(userID),
                    Absences = _userService.GetUserAbsencesForLastTerm(userID),
                    StudentId = _userService.GetStudentIDByUserID(userID),
                    TotalSchoolDays = totalSchoolDays
                };

                childrenAbsenceViewModel.ChildrenAbsences.Add(childAbsenceInfo);

            }
            return childrenAbsenceViewModel;
        }
    }
}