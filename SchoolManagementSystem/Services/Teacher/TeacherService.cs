using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.Teacher;

namespace SchoolManagementSystem.Services.Teacher
{
    public class TeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }



        public DateOnly GetDate(DateTime dateTime) => DateOnly.FromDateTime(dateTime);

        public TimeOnly GetTime(DateTime dateTime) => TimeOnly.FromDateTime(dateTime);

        public string GetExamStatus(DateTime dateTime, int Duration)
        {
            DateOnly ExamDate = GetDate(dateTime);
            TimeOnly ExamTime = GetTime(dateTime);
            var examDateTime = ExamDate.ToDateTime(ExamTime);

            if (examDateTime > DateTime.Now)
                return "قادم";
            else if (examDateTime.AddMinutes(Duration) >= DateTime.Now)
                return "جاري";
            else
                return "منتهي";
        }

        public async Task<NoteDisplayViewModel> GetNoteToTeacherAsync(int teacheruserid)
        {
            var navigation = await GetNavigationDataAsync(teacheruserid);

            var notesList = await _context.Notes
                .Where(e => e.TargetUserId == teacheruserid)
                .Select(e => new NoteItemViewModel
                {
                    WriterName = e.WriterUser.FullName,
                    AddedDate = e.AddedDate,
                    NoteDetails = e.NoteDetails
                })
                .OrderByDescending(e => e.AddedDate)
                .ToListAsync();

            return new NoteDisplayViewModel
            {
                NavigationInfo = navigation,
                Notes = notesList
            };
        }

        public async Task< DashboardViewModel >GetTeacherDashboard(int teacheruserid)
        {
            var navigation = await GetNavigationDataAsync(teacheruserid);
            var teacher =await _context.Users
                .Where(u => u.ID == teacheruserid)
                .Select( u => new DashboardViewModel
                {
                    NavigationInfo=navigation,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    Phone = u.Phone,
                    HireDate = u.Teacher != null ? u.Teacher.HireDate : default,
                    Subjects = u.Teacher != null
                        ? u.Teacher.TeacherSubjects.Select(ts => ts.Subject.Name).Distinct().ToList()
                        : new List<string>()
                }).FirstOrDefaultAsync();

            return teacher;
        }

        public async Task<GradesViewModel> GetTeacherGrades(int teacheruserid, int? subjectid, int? levelid, int? stageid, int? classid)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();
            var techerid = await GetTeacherIdAsync(teacheruserid);
            var studentData = _context.StudentExamDegrees
                .Where(e => e.Exam.TeacherId == techerid && lastTermStartDate <= e.Exam.ActualDate)
                .Where(e => !subjectid.HasValue || e.Exam.SubjectId == subjectid)
                .Where(e => !classid.HasValue || e.Exam.ClassId == classid)
                .Where(e => !levelid.HasValue || e.Student.StudentClassEnrollments.Any(s => s.LevelID == levelid))
                .Where(e => !stageid.HasValue || e.Student.StudentClassEnrollments.Any(s => s.Level.StageID == stageid))
                .Select(e => new StudentGradeViewModel
                {
                    fullName = e.Student.User.FullName,
                    Subject = e.Exam.Subject.Name,
                    Grade = e.Degree,
                    Data = e.Exam.ActualDate,
                    ExamPercentage = e.Exam.TotalDegree > 0
                        ? (decimal)e.Degree / e.Exam.TotalDegree * 100
                        : 0
                })
                .ToList();

            var maxDegree = studentData.Any() ? studentData.Max(x => x.Grade) : 0;
            var averageDegree = studentData.Any() ? studentData.Average(x => x.Grade) : 0;
            var studentCount = studentData.Count;

            var subjects = await GetTeacherSubjectsAsync(techerid);
            var classes = await GetTeacherClassesAsync(techerid);
            var levels = await GetTeacherLevelsAsync(techerid);
            var stages = await GetTeacherStagesAsync(techerid);

            var exams = new GradesViewModel
            {
                StudentGrades = studentData,
                maxDegree = maxDegree,
                AverageGrade = averageDegree,
                studentCount = studentCount,
                Stages = stages,
                Levels = levels,
                Classes = classes,
                Subjects = subjects,
                NavigationInfo = await GetNavigationDataAsync(teacheruserid)
            };

            return exams;
        }

     

        public async Task<DateTime> GetLastAcademicTermDate()
        {
            return await _context.AcademicTerms
                .MaxAsync(a => a.StartDate);
        }
       


        public async Task<List<Level>> GetTeacherGradesAsync(int teacheruserid)
        {
            var teacherId = await GetTeacherIdAsync(teacheruserid);
            var lastTermStartDate = await GetLastAcademicTermDate();
            return await _context.StudentsSubjectsEnrollments
                .Where(x => x.TeacherId == teacherId && x.EnrolledDate >= lastTermStartDate)
                .SelectMany(x => x.Student.StudentClassEnrollments)
                .Select(e => e.Level)
                .Distinct()
                .ToListAsync();
        }

        public async Task<TeacherStudentsPageViewModel> GetTeacherStudentsAsync(int teacheruserId, int? levelId, int? stageId, int? classId)
        {
            var teacherId = await GetTeacherIdAsync(teacheruserId);
            var lastTermStartDate = await GetLastAcademicTermDate();

            var query = _context.StudentsSubjectsEnrollments
                .Where(x => x.TeacherId == teacherId && x.EnrolledDate >= lastTermStartDate);

            if (levelId.HasValue && levelId > 0)
            {
                query = query.Where(x => x.Student.StudentClassEnrollments.Any(a => a.LevelID == levelId));
            }

            if (classId.HasValue && classId > 0)
            {
                query = query.Where(x => x.Student.StudentClassEnrollments.Any(s => s.ClassId == classId));
            }

            if (stageId.HasValue && stageId > 0)
            {
                query = query.Where(x => x.Student.StudentClassEnrollments.Any(s => s.Level.StageID == stageId));
            }

            var homeworksCountByClass = await _context.Homeworks
                .Where(h => h.TeacherId == teacherId && h.LastDate >= DateTime.Now)
                .GroupBy(h => h.ClassId)
                .Select(g => new { ClassId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ClassId, x => x.Count);

            var examsCountByClass = await _context.MultiChoiceExam
                .Where(e => e.TeacherId == teacherId && e.AddedDate >= lastTermStartDate && e.TotalDegree > 0)
                .GroupBy(e => e.ClassId)
                .Select(g => new { ClassId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ClassId, x => x.Count);

            var studentsData = await query
                .Select(x => x.Student)
                .Distinct()
                .Select(s => new
                {
                    s.ID,
                    s.UserId,
                    FullName = s.User.FullName ?? "",
                    Phone = s.User.Phone,
                    ParentName = s.Parent != null && s.Parent.User != null ? s.Parent.User.FullName : "غير مسجل",
                    ParentPhone = s.Parent != null && s.Parent.User != null ? s.Parent.User.Phone : "غير مسجل",

                    CurrentClassId = s.StudentClassEnrollments.Select(c => c.ClassId).FirstOrDefault(),

                    AbsencesCount = _context.Absences.Count(a => a.UserId == s.UserId),

                    TotalStudentDegrees = s.StudentExamDegrees
                        .Where(e => e.Exam.TotalDegree > 0 && e.Exam.AddedDate >= lastTermStartDate)
                        .Sum(e => (decimal?)e.Degree) ?? 0,

                    TotalExamMaxDegrees = s.StudentExamDegrees
                        .Where(e => e.Exam.TotalDegree > 0 && e.Exam.AddedDate >= lastTermStartDate)
                        .Sum(e => (decimal?)e.Exam.TotalDegree) ?? 0,

                    CompletedHomeworks = s.StudentHomeworkAnswers
                        .Count(a => a.AssignDate >= lastTermStartDate),
                })
                .ToListAsync();

            var studentList = studentsData.Select(s =>
            {
                int totalHomeworksForThisStudent = homeworksCountByClass.TryGetValue(s.CurrentClassId, out var count) ? count : 0;
                int totalExamsForThisStudent = examsCountByClass.TryGetValue(s.CurrentClassId, out var eCount) ? eCount : 0;

                bool hasExams = totalExamsForThisStudent > 0;
                bool hasHomeworks = totalHomeworksForThisStudent > 0;
                bool hasData = hasExams || hasHomeworks; 

                decimal homeworkPercentage = hasHomeworks
                    ? ((decimal)s.CompletedHomeworks / totalHomeworksForThisStudent) * 100
                    : 0;

                decimal examPercentage = s.TotalExamMaxDegrees > 0
                    ? (s.TotalStudentDegrees / s.TotalExamMaxDegrees) * 100
                    : 0;

                decimal totalPercentage = 0;

                if (!hasExams && !hasHomeworks)
                {
                    totalPercentage = 0;
                }
                else if (!hasExams)
                {
                    totalPercentage = homeworkPercentage;
                }
                else if (!hasHomeworks)
                {
                    totalPercentage = examPercentage; 
                }
                else
                {
                    totalPercentage = (examPercentage * 0.7m) + (homeworkPercentage * 0.3m);
                }

                return new TeacherStudentViewModel
                {
                    StudentId = s.ID,
                    FullName = s.FullName,
                    StudentPhone = s.Phone,
                    AbsencesCount = s.AbsencesCount,
                    ParentName = s.ParentName,
                    ParentPhone = s.ParentPhone,
                    ExamPercentage = hasExams ? $"{(int)examPercentage}%" : "لا يوجد امتحانات",
                    HomworkPerecentage = homeworkPercentage,
                    TotalPercentage = totalPercentage,
                    Performance = GetPerformance(totalPercentage, hasData) 
                };
            }).ToList();

            var result = new TeacherStudentsPageViewModel
            {
                Students = studentList,
                Classes = await GetTeacherClassesAsync(teacherId),
                Levels = await GetTeacherLevelsAsync(teacherId),
                Stages = await GetTeacherStagesAsync(teacherId),
                NavigationInfo = await GetNavigationDataAsync(teacheruserId)
            };

            return result;
        }

        private string GetPerformance(decimal totalPercentage, bool hasData)
        {
            if (!hasData) return "لا يوجد تقييم";

            if (totalPercentage >= 90) return "ممتاز";
            if (totalPercentage >= 80) return "جيد جداً";
            if (totalPercentage >= 70) return "جيد";
            if (totalPercentage >= 60) return "مقبول";

            return "ضعيف";
        }
        public async Task<int> GetStudentUserId(int studentId)
        {
            return await _context.Students
                .Where(s => s.ID == studentId)
                .Select(s => s.UserId)
                .FirstOrDefaultAsync();

        }

        public async Task<List<string>> GetStudentAbsenceDates(int studentId)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();
            return await _context.Absences
                .Where(a => a.UserId == studentId && a.AbsenceDate >= lastTermStartDate)
                .OrderByDescending(a => a.AbsenceDate)
                .Select(a => a.AbsenceDate.ToString("yyyy-MM-dd"))
                .ToListAsync();
        }

        public async Task<int> CreateExamAsync(AddExamViewModel model, int teacherId, AppDbContext context)
        {
            var exam = new MultiChoiceExam
            {
                Name = model.Name,
                SubjectId = model.SelectedSubjectId,
                ClassId = model.SelectedClassId,
                TeacherId = teacherId,
                ActualDate = model.Date.Add(model.Time),
                DurationInMinutes = model.Duration,
                ExamType = model.Type,
                AddedDate = DateTime.Now,
                TotalDegree = model.Questions.Sum(q => q.QuestionDegree)

            };

            context.MultiChoiceExam.Add(exam);
            await context.SaveChangesAsync();
            return exam.ID;
        }

        public async Task AddExamWithQuestions(AddExamViewModel model, int teaheruserid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var teacherId = await GetTeacherIdAsync(teaheruserid);
                var examId = await CreateExamAsync(model, teacherId, _context);

                foreach (var que in model.Questions)
                {
                    var questionId = await AddQuestionAsync(examId, que, _context);
                    AddChoices(questionId, que.Choices, _context);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> AddQuestionAsync(int examId, QuestionViewModel que, AppDbContext context)
        {
            var question = new Question
            {
                Title = que.QuestionText,
                Degree = que.QuestionDegree,
                ExamId = examId
            };

            context.Questions.Add(question);
            await context.SaveChangesAsync();
            return question.ID;
        }

        public void AddChoices(int questionId, OptionViewModel cho, AppDbContext context)
        {
            if (cho.CorrectOptionIndex < 0 || cho.CorrectOptionIndex >= cho.Options.Count)
                throw new Exception("الإجابة الصحيحة غير صحيحة");

            for (int i = 0; i < cho.Options.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(cho.Options[i]))
                    throw new Exception("الاختيارات لا يمكن أن تكون فارغة");

                var answer = new QuestionAnswer
                {
                    AnswerValue = cho.Options[i],
                    IsCorrect = (i == cho.CorrectOptionIndex),
                    QuestionId = questionId
                };

                context.QuestionAnswers.Add(answer);
            }
        }

        public async Task<bool> UpdateExamAsync(EditExamViewModel model)
        {
            var exam = await _context.MultiChoiceExam
                .Include(e => e.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(e => e.ID == model.Id);

            if (exam == null)
            {
                return false;
            }

            exam.Name = model.Name;
            exam.SubjectId = model.SelectedSubjectId;
            exam.ClassId = model.SelectedClassId;
            exam.ExamType = model.Type;
            exam.ActualDate = model.Date;
            exam.DurationInMinutes = model.Duration;
            exam.TotalDegree = model.Questions.Sum(q => q.Mark);

            var oldAnswers = exam.Questions.SelectMany(q => q.QuestionAnswers).ToList();
            if (oldAnswers.Any())
            {
                _context.QuestionAnswers.RemoveRange(oldAnswers);
            }

            var oldQuestions = exam.Questions.ToList();
            if (oldQuestions.Any())
            {
                _context.Questions.RemoveRange(oldQuestions);
            }

            exam.Questions = model.Questions.Select(q => new Question
            {
                Title = q.Text,
                Degree = q.Mark,
                QuestionAnswers = q.Choices.Select(c => new QuestionAnswer
                {
                    AnswerValue = c.ChoiceText,
                    IsCorrect = c.IsCorrect
                }).ToList()
            }).ToList();

            _context.MultiChoiceExam.Update(exam);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }
        public async Task<EditExamViewModel?> GetExamForEditAsync(int id)
        {
            var exam = await _context.MultiChoiceExam
                .Include(e => e.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (exam == null)
            {
                return null;
            }

            var viewModel = new EditExamViewModel
            {
                Id = exam.ID,
                Name = exam.Name,
                SelectedSubjectId = exam.SubjectId,
                SelectedClassId = exam.ClassId,
                Type = exam.ExamType,
                Date = exam.ActualDate,
                Duration = exam.DurationInMinutes,

                Subjects = await GetTeacherSubjectsAsync(exam.TeacherId),
                Classes = await GetTeacherClassesAsync(exam.TeacherId),

                Questions = exam.Questions.Select(q => new EditQuestionViewModel
                {
                    Id = q.ID,
                    Text = q.Title,
                    Mark = q.Degree,
                    Choices = q.QuestionAnswers.Select(c => new EditChoiceViewModel
                    {
                        Id = c.ID,
                        ChoiceText = c.AnswerValue,
                        IsCorrect = c.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return viewModel;
        }

        public async Task<(bool Success, string Error)> AddAssignmentAsync(AddAssignmentViewModel model, int teacheruserid)
        {
            if (model.file == null || model.file.Length == 0)
                return (false, "يجب رفع ملف");

            var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".png" };
            var extension = Path.GetExtension(model.file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return (false, "نوع الملف غير مسموح");

            if (model.startDate > model.endDate)
                return (false, "تاريخ البداية يجب أن يكون قبل تاريخ النهاية");

            var filePath = await AddFileAsync(model.file, extension);

            var homework = new Homework
            {
                Title = model.Name,
                ClassId = model.ClassId,
                SubjectID = model.SubjectId,
                AddedDate = model.startDate,
                LastDate = model.endDate,
                Link = filePath,
                TeacherId = await GetTeacherIdAsync(teacheruserid)
            };

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<TeacherTimeTableViewModel> GetTimeTableAsync(int teacheruserid)
        {
            var navigation = await GetNavigationDataAsync(teacheruserid);

            var timetable = await _context.TeacherTimeTables
                .Where(t => t.Teacher.UserId == teacheruserid)
                .Select(t => new TeacherTimeTableViewModel
                {
                    PhotoLink = t.PhotoLink
                })
                .FirstOrDefaultAsync();

            if (timetable == null)
            {
                return new TeacherTimeTableViewModel
                {
                    PhotoLink = null,
                    NavigationInfo = navigation
                };
            }

            timetable.NavigationInfo = navigation;
            return timetable;
        }


        public async Task<AddExamViewModel> GetAddExamViewModel(int teacheruserid)
        {
            
            var teacherId = await GetTeacherIdAsync(teacheruserid);
            var subjects = await GetTeacherSubjectsAsync(teacherId);
            var classes = await GetTeacherClassesAsync(teacherId);
            return new AddExamViewModel
            {
                Subjects = subjects,
                Classes = classes
            };
        }

        public async Task<bool> DeleteExamAsync(int id)
        {
            try
            {
                var exam = await _context.MultiChoiceExam.FindAsync(id);

                if (exam == null)
                    return false;

                var questionAnswers = await _context.QuestionAnswers
                    .Where(q => q.Question.ExamId == id)
                    .ToListAsync();

                var questions = await _context.Questions
                    .Where(q => q.ExamId == id)
                    .ToListAsync();

                var examDegrees = await _context.StudentExamDegrees
                    .Where(e => e.ExamId == id)
                    .ToListAsync();

                _context.QuestionAnswers.RemoveRange(questionAnswers);
                _context.Questions.RemoveRange(questions);
                _context.StudentExamDegrees.RemoveRange(examDegrees);
                _context.MultiChoiceExam.Remove(exam);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<EditAssignmentViewModel> GetAssignmentAsync(int id)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();


            var query = _context.Homeworks
                .Where(h => h.ID == id && h.AddedDate >= lastTermStartDate);
            var link = query.Select(e => e.Link).FirstOrDefault();

            var assignment = await query.Select(h => new EditAssignmentViewModel
            {
                ID = h.ID,
                Name = h.Title,
                ClassName = h.Class.Name,
                SubjectName = h.Subject.Name,
                startDate = h.AddedDate,
                endDate = h.LastDate,
                ExistingFilePath = h.Link

            })
              .FirstOrDefaultAsync();
            return assignment;
        }

        public async Task<string> AddFileAsync(IFormFile file, string extension)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Homework");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + extension;
            var path = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/Uploads/Homework/" + fileName;
        }

        public async Task<bool> UpdateAssignmentAsync(EditAssignmentViewModel editAssignmentView)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var assignment = await _context.Homeworks.FindAsync(editAssignmentView.ID);
                    if (assignment == null)
                        return false;

                    assignment.Title = editAssignmentView.Name;
                    assignment.AddedDate = editAssignmentView.startDate;
                    assignment.LastDate = editAssignmentView.endDate;

                    string oldFilePath = null;
                    string newFilePath = null;

                    if (editAssignmentView.File != null)
                    {
                        oldFilePath = assignment.Link;

                        var extension = Path.GetExtension(editAssignmentView.File.FileName).ToLower();
                        newFilePath = await AddFileAsync(editAssignmentView.File, extension);

                        assignment.Link = newFilePath;
                    }

                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(oldFilePath))
                    {
                        await DeleteHomeworkFileAsync(oldFilePath);
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return false;
                }
            }
        }

        public async Task<bool> DeleteAssignmentAsync(int assignmentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var assignment = await _context.Homeworks.FindAsync(assignmentId);
                if (assignment == null)
                    return false;

                string assignmentFileName = assignment.Link;

                var answers = _context.StudentHomeworkAnswers.Where(a => a.HomeworkId == assignmentId).ToList();

                foreach (var answer in answers)
                {
                    if (!string.IsNullOrEmpty(answer.Link))
                    {
                        var answerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/HomeworkAnswers", answer.Link);
                        if (File.Exists(answerPath))
                        {
                            File.Delete(answerPath);
                        }
                    }
                }

                _context.StudentHomeworkAnswers.RemoveRange(answers);
                _context.Homeworks.Remove(assignment);

                var result = await _context.SaveChangesAsync() > 0;

                if (result)
                {

                    await DeleteHomeworkFileAsync(assignmentFileName);
                    await transaction.CommitAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task DeleteHomeworkFileAsync(string fileName)
        {

            if (!string.IsNullOrEmpty(fileName))
            {
                var fileNameOnly = Path.GetFileName(fileName);
                var assignmentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Homework", fileNameOnly);
                if (File.Exists(assignmentPath))
                {
                    File.Delete(assignmentPath);
                }
            }
        }
        public async Task<NavigationViewModel?> GetNavigationDataAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.ID == userId)
                .Select(u => new NavigationViewModel
                {
                    FullName = u.FirstName+" "+u.LastName,
                    ProfilePhotoUrl = u.ProfilPhotoURL?? "default.png"
                })
                .FirstOrDefaultAsync();
        }
        
        public async Task<TeacherExamsDashboardViewModel> GetTeacherExamsDashboard(int teacheruserId,int? levelid, int? stageid, int? classid, int? subjectid, string? status, string? type)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();
            var teacherId = await GetTeacherIdAsync(teacheruserId);
            var query = _context.MultiChoiceExam
                .Where(e => e.TeacherId == teacherId && e.ActualDate >= lastTermStartDate)
                .Where(e => !subjectid.HasValue || e.SubjectId == subjectid)
                .Where(e => !classid.HasValue || e.ClassId == classid)
                .Where(e => !levelid.HasValue || e.Class.LevelID == levelid)
                .Where(e => !stageid.HasValue || e.Class.Level.StageID == stageid)
                .Select(e => new
                {
                    e.ID,
                    e.Name,
                    Subject = e.Subject != null ? e.Subject.Name : "غير محدد",
                    ExamType = e.ExamType.ToString(),
                    e.ActualDate,
                    e.DurationInMinutes,
                    QuestionsCount = e.Questions.Count,
                    TotalScore = e.TotalDegree
                })
                .ToList();

            var examsData = query
                .Select(e => new ExamViewModel
                {
                    Id = e.ID,
                    ExamName = e.Name,
                    Subject = e.Subject,
                    ExamType = e.ExamType,
                    ExamDate = GetDate(e.ActualDate),
                    ExamTime = GetTime(e.ActualDate),
                    Duration = e.DurationInMinutes,
                    QuestionsCount = e.QuestionsCount,
                    TotalScore = e.TotalScore,
                    Status = GetExamStatus(e.ActualDate, e.DurationInMinutes)
                })
                .Where(e => string.IsNullOrEmpty(status) || e.Status == status)
                .Where(e => string.IsNullOrEmpty(type) || e.ExamType.Equals(type, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var classes = await GetTeacherClassesAsync(teacherId);
            var levels = await GetTeacherLevelsAsync(teacherId);
            var subjects = await GetTeacherSubjectsAsync(teacherId);
            var stages = await GetTeacherStagesAsync(teacherId);
            var navigation=await GetNavigationDataAsync(teacheruserId);
            return new TeacherExamsDashboardViewModel
            {
                Classes = classes,
                Levels = levels,
                Subjects = subjects,
                Stages = stages,
                Exams = examsData,
                NavigationInfo=navigation,
                totalExams = examsData.Count(),
                ongoingExams = examsData.Count(e => e.Status == "جاري"),
                upcomingExams = examsData.Count(e => e.Status == "قادم"),
                finishedExams = examsData.Count(e => e.Status == "منتهي")

            };
        }

        public async Task<int>GetTeacherIdAsync(int teacherUserId)
        {
            return await _context.Teachers
                .Where(t => t.UserId == teacherUserId)
                .Select(t => t.ID)
                .FirstOrDefaultAsync();
        }
        public async Task<ExamGradesViewModel>GetExamGradesAsync(int examid)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();

            var examGrades = await _context.StudentExamDegrees.Where(e => e.ExamId == examid && e.Exam.ActualDate >= lastTermStartDate)
                .Select(e => new StudentGrades
                {
                    Code = e.StudentId,
                    Name = e.Student.User.FullName,
                    grade = e.Degree,
                    status = e.Exam.TotalDegree / 2 <= e.Degree ? "ناجح" : "راسب",
                }).ToListAsync();
            var exam = await _context.MultiChoiceExam
                .Include(e => e.Subject)
                .Where(e => e.ID == examid)
                .FirstOrDefaultAsync();
            return new ExamGradesViewModel
            {
                StudentGrades = examGrades,
                Subject = exam.Subject.Name,
                Date = exam.ActualDate,
                TotalDegree = exam.TotalDegree,
                StudentCount = examGrades.Count(),
                SuccessDegree = exam.TotalDegree / 2

            };

        }
        public async Task<ExamDetailsViewModel?> GetExamDetailsAsync(int examid)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();

            var examDetails = await _context.MultiChoiceExam
                .Where(e => e.ID == examid && e.AddedDate >= lastTermStartDate) 
                .Select(e => new ExamDetailsViewModel
                {
                    Id = e.ID,
                    Name = e.Name,
                    SubjectName = e.Subject != null ? e.Subject.Name : "غير محدد", 
                    ClassName = e.Class != null ? e.Class.Name : "غير محدد",
                    Type = e.ExamType,
                    Date = e.ActualDate,
                    Time = e.ActualDate.TimeOfDay, 
                    Duration = e.DurationInMinutes,
                    TotalMarks = e.TotalDegree,
                    Questions = e.Questions.Select(q => new QuestionDetailsViewModel
                    {
                        Id = q.ID,
                        Text = q.Title,
                        Mark = q.Degree,
                        Choices = q.QuestionAnswers.Select(c => new ChoiceViewModel
                        {
                            ChoiceText = c.AnswerValue,
                            IsCorrect = c.IsCorrect
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return examDetails;
        }

        public async Task<List<IdNameViewModel>> GetTeacherSubjectsAsync(int teacheruserid)
        {
            var teacherId = await GetTeacherIdAsync(teacheruserid);
            return await _context.TeacherSubjects
                .Where(s => s.TeacherId == teacherId)
                .Select(s => new IdNameViewModel
                {
                    Id = s.SubjectId,
                    Name = s.Subject.Name
                })
                .Distinct()
                .ToListAsync();
        }
        
        public async Task<List<IdNameViewModel>> GetTeacherClassesAsync(int teacherid)
        {

            var lastTermStartDate = await GetLastAcademicTermDate();

            return await _context.StudentsSubjectsEnrollments
                .Where(s => s.TeacherId == teacherid && s.EnrolledDate >= lastTermStartDate)
                .SelectMany(s => s.Student.StudentClassEnrollments)
                .Select(c => new IdNameViewModel
                {
                    Id = c.ClassId,
                    Name = c.Class.Name
                })
                .Distinct()
                .ToListAsync();
        }
        
        public async Task<List<Stage>> GetTeacherClassesAsync(int teacherId, int? levelid)
        {

            var lastTermStartDate = await GetLastAcademicTermDate();
            var query = _context.StudentsSubjectsEnrollments
                .Where(x => x.TeacherId == teacherId && x.EnrolledDate >= lastTermStartDate)
                .SelectMany(x => x.Student.StudentClassEnrollments);

            if (levelid.HasValue && levelid > 0)
            {
                query = query.Where(e => e.LevelID == levelid);
            }

            return await query
                .Select(e => e.Level.Stage)
                .Distinct()
                .ToListAsync();
        }


      
        public async Task<List<IdNameViewModel>> GetTeacherLevelsAsync(int teacherId)
        {
           

            var lastTermStartDate = await GetLastAcademicTermDate();

            return await _context.StudentsSubjectsEnrollments
                .Where(t => t.TeacherId == teacherId && t.EnrolledDate >= lastTermStartDate)
                .SelectMany(s => s.Student.StudentClassEnrollments)
                .Select(s => new IdNameViewModel
                {
                    Id = s.LevelID,
                    Name = s.Level.Name,
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<IdNameViewModel>> GetTeacherStagesAsync(int teacherId)
        {

            var lastTermStartDate = await GetLastAcademicTermDate();

            return await _context.StudentsSubjectsEnrollments
                .Where(t => t.TeacherId == teacherId && t.EnrolledDate >= lastTermStartDate)
                .SelectMany(s => s.Student.StudentClassEnrollments)
                .Select(s => new IdNameViewModel
                {
                    Id = s.Level.Stage.ID,
                    Name = s.Level.Stage.Name,
                })
                .Distinct()
                .ToListAsync();


        }
        public async Task<AssignmentDashbordViewModel> GetTeacherAssignmentsDashboard(int teacheruserid, int? levelid, int? stadgeid, int? classid, int? subjectid, string? status)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();
            var teacherid = await GetTeacherIdAsync(teacheruserid);

            var query = _context.Homeworks
                .Where(h => h.TeacherId == teacheruserid && h.AddedDate >= lastTermStartDate);

            if (subjectid.HasValue)
            {
                query = query.Where(h => h.SubjectID == subjectid.Value);
            }

            if (classid.HasValue)
            {
                query = query.Where(h => h.ClassId == classid.Value);
            }

            if (levelid.HasValue)
            {
                query = query.Where(h => h.Class != null && h.Class.LevelID == levelid.Value);
            }

            if (stadgeid.HasValue)
            {
                query = query.Where(h => h.Class != null && h.Class.Level != null && h.Class.Level.Stage.ID == stadgeid.Value);
            }

            var data = query.Select(h => new AssignmentViewModel
            {
                Id = h.ID,
                subjectId = h.SubjectID,
                classId = h.ClassId,
                levelId = h.Class != null ? h.Class.LevelID : null,
                stageId = (h.Class != null && h.Class.Level != null) ? h.Class.Level.StageID : null,
                Name = h.Title,
                Subject = h.Subject != null ? h.Subject.Name : "غير محدد",
                StartDate = h.AddedDate,
                EndDate = h.LastDate,
                FileName = h.Link,
            }).ToList();

            if (!string.IsNullOrEmpty(status))
            {
                data = data.Where(a => a.Status == status).ToList();
            }

            var classes = await GetTeacherClassesAsync(teacherid);
            var levels = await GetTeacherLevelsAsync(teacherid);
            var subjects = await GetTeacherSubjectsAsync(teacherid);
            var stages = await GetTeacherStagesAsync(teacherid);

            return new AssignmentDashbordViewModel
            {
                Assignments = data,
                Classes = classes,
                Levels = levels,
                Subjects = subjects,
                Stages = stages,
                totalAssignments = data.Count,
                completedAssignments = data.Count(a => a.Status == "منتهي"),
                lateAssignments = data.Count(a => a.Status == "جاري" || a.Status == "لم يبدأ بعد"),
                NavigationInfo = await GetNavigationDataAsync(teacheruserid)
            };
        }
        public async Task<NotesPageViewModel> GetTeacherNotesAsync(int teacheruserId, int? levelid, int? stageid, int? classid)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();
            var teacherId = await GetTeacherIdAsync(teacheruserId);

            var classes = await GetTeacherClassesAsync(teacherId);
            var levels = await GetTeacherLevelsAsync(teacherId);
            var stages = await GetTeacherStagesAsync(teacherId);
            var teaherId = await GetTeacherIdAsync(teacherId);

            var query = _context.StudentsSubjectsEnrollments
                .Where(n => n.TeacherId == teaherId && n.EnrolledDate >= lastTermStartDate);

            if (levelid.HasValue && levelid > 0)
            {
                query = query.Where(n => n.Student.StudentClassEnrollments.Any(s => s.LevelID == levelid));
            }
            if (stageid.HasValue && stageid > 0)
            {
                query = query.Where(n => n.Student.StudentClassEnrollments.Any(s => s.Level.StageID == stageid));
            }
            if (classid.HasValue && classid > 0)
            {
                query = query.Where(n => n.Student.StudentClassEnrollments.Any(s => s.ClassId == classid));
            }

            var studentsList = await query
                .Select(n => new StudentNoteItemViewModel
                {
                    ID = n.StudentId,
                    Name = n.Student.User.FullName,
                    Note = n.Student.User.ReceivedNotes.Any()
                        ? string.Join(" , ", n.Student.User.ReceivedNotes.Select(s => s.NoteDetails))
                        : "لا يوجد ملاحظات",
                    Level = n.Student.StudentClassEnrollments.Where(s => s.StudentId == n.StudentId).Select(s => s.Level.Name).FirstOrDefault(),
                    Class = n.Student.StudentClassEnrollments.Where(s => s.StudentId == n.StudentId).Select(s => s.Level.Stage.Name).FirstOrDefault(), // الصف (Stage)
                    Section = n.Student.StudentClassEnrollments.Where(s => s.StudentId == n.StudentId).Select(s => s.Class.Name).FirstOrDefault() // الفصل (Class)
                })
                .ToListAsync();

            var finalStudentsList = studentsList
            .GroupBy(s => s.ID)
            .Select(g => g.First())
            .ToList();
            var viewModel = new NotesPageViewModel
            {
                Students = finalStudentsList,
                Levels = levels,
                Stages = stages,
                Classes = classes,

                NavigationInfo = await GetNavigationDataAsync(teacheruserId)
            };

            return viewModel;
        }

        public async Task<bool> AddNoteAsync(int teacheruserid, AddNoteViewModel viewModel)
        {
            try
            {
                var studentUserId = await _context.Students
                .Where(s => s.ID == viewModel.StudentId)
                .Select(s => s.UserId)
                .FirstOrDefaultAsync();




                if (studentUserId == 0) return false;
                var note = new Note
                {
                    WriterUserId = teacheruserid,
                    TargetUserId = studentUserId,
                    NoteDetails = viewModel.Note,
                    AddedDate = DateTime.Now,
                };
                await _context.Notes.AddAsync(note);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public async Task<AllNotesForStudent> GetNoteAsync(int teacheruserid, int studentId)
        {
            try
            {
                var targetuserId = await _context.Students
                    .Where(s => s.ID == studentId)
                    .Select(s => s.UserId)
                    .FirstOrDefaultAsync();


                if (targetuserId == 0) return null;

                var studentName = await _context.Students
                    .Where(s => s.ID == studentId)
                    .Select(s => s.User.FullName)
                    .FirstOrDefaultAsync() ?? "طالب غير معروف";

                var notes = await _context.Notes
                    .Where(n => n.TargetUserId == targetuserId && n.WriterUserId == teacheruserid)
                    .Select(n => new NoteItem
                    {
                        Id = n.ID,
                        Note = n.NoteDetails
                    })
                    .ToListAsync();

                return new AllNotesForStudent
                {
                    StudentName = studentName,
                    Notes = notes ?? new List<NoteItem>()
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            try
            {
                var note = await _context.Notes.FindAsync(noteId);
                if (note == null) return false;
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<List<HomeworkAnswerViewModel>> GetAllSubmitionForHomewoekAsync(int homeworkId)
        {
            var lastTermStartDate = await GetLastAcademicTermDate();

            var submissionsRaw = await _context.StudentHomeworkAnswers
                .Where(h => h.HomeworkId == homeworkId && h.AssignDate >= lastTermStartDate)
                .Select(h => new
                {
                    h.ID,
                    StudentName = h.Student.User.FullName,
                    h.AssignDate,
                    h.Link
                })
                .ToListAsync();

            return submissionsRaw.Select(h => new HomeworkAnswerViewModel
            {
                Id = h.ID,
                FullName = h.StudentName,
                Date = GetDate(h.AssignDate),
                FileName = h.Link
            }).ToList();
        }
    }
}