using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;

using SchoolManagementSystem.ViewModel.Owner;
using SchoolManagementSystem.ViewModel.Teacher;
using System.Runtime.InteropServices;
using System.Transactions;

namespace SchoolManagementSystem.Services
{
    public class OwnerService
    {
        readonly AppDbContext _context;
        readonly UserService _userService;
        readonly UserTypeService _userTypeService;
        public OwnerService(AppDbContext context, UserService userService, UserTypeService userTypeService)
        {
            _context = context;
            _userService = userService;
            _userTypeService = userTypeService;
        }



        public async Task<TeacherSubjects> GetTeacherSubjects(int teacherId)
        {
            int teacherUserId = await _context.Teachers
                .Where(t => t.ID == teacherId)
                .Select(t => t.UserId)
                .FirstOrDefaultAsync();

            var teacherSubjects = new TeacherSubjects();

            teacherSubjects.FullName = _userService.GetUserFullName(teacherUserId);

            teacherSubjects.Subjects = await _context.TeacherSubjects
                .AsNoTracking()
                .Where(ts => ts.TeacherId == teacherId)
                .Select(ts => ts.Subject)
                .Distinct()
                .ToListAsync();

            return teacherSubjects;
        }
        public async Task<List<TeacherInfo>> GetTeachersInfo()
        {

            var lastTermID = await _GetLastAcademicTermIdAsync();

            var StartDate = await _context.AcademicTerms
                .Where(at => at.ID == lastTermID)
                .Select(at => at.StartDate)
                .FirstOrDefaultAsync();

            var EndDate = await _context.AcademicTerms
               .Where(at => at.ID == lastTermID)
               .Select(at => at.EndDate)
               .FirstOrDefaultAsync();

            var teachers = await _context.Teachers
                .AsNoTracking()
                .Select(t => new TeacherInfo
                {
                    TeacherID = t.ID,
                    UserID = t.UserId,
                    JoinDate = t.HireDate,
                    ExitDate = t.ExiteDate,
                    FullName=t.User.FullName,
                    AbcenseDaysCount = t.User.Absences
                    .Where(a => a.AbsenceDate >= StartDate && a.AbsenceDate <= EndDate)
                     .Count(),

                })
                .ToListAsync();

           

            return teachers;
        }

       
        public async Task<List<AcademicTermsFilter>> GetAcademicTermsFilterInfoAsync()
        {
            var AcademicTerms=new List<AcademicTermsFilter>();

            try
            {
                AcademicTerms=await _context.AcademicTerms.Include(at=> at.AcademicYear)
                    .Where(at=> at.EndDate<DateTime.Now)
                    .Select(at=> new AcademicTermsFilter { 
                        TermName=at.Name,
                        YearName=at.AcademicYear.Name,
                        ID= at.ID
                    }).OrderByDescending(at=> at.ID)
                    .ToListAsync() ;
            }
            catch (Exception ex)
            {

            }


            return AcademicTerms;
        }

        private async Task<List<(int LevelID, string LevelName)>> _GetStageLevelsAsync(int StageID)
        {
            var levels = new List<(int LevelID, string LevelName)>();

            try
            {
               
                var dbLevels = await _context.Levels
                    .Where(l => l.StageID == StageID)
                    .Select(l => new { l.ID, l.Name }) 
                    .ToListAsync();

               
                levels = dbLevels.Select(l => (l.ID, l.Name)).ToList();
            }
            catch (Exception ex)
            {
              
            }

            return levels;
        }


        private async Task<int> _GetTotalNumOfStudentsInLevelInSpecificTermAsync(int LevelID,int TermID)
        {
            int totalNumOfStudentsInLevel = 0;
            try
            {
                totalNumOfStudentsInLevel = await _context.StudentClassEnrollments
                    .Where(sce => sce.LevelID == LevelID && sce.AcademicTermId == TermID)
                    .CountAsync();
            }
            catch (Exception ex)
            {

            }

            return totalNumOfStudentsInLevel;

        }

        private async Task<int> _GetTotalNumOfStudentsPassedInLevelInSpecificTermAsync(int LevelID, int TermID)
        {
            int NumOfPassedStudentsInLevel = 0;
            try
            {
                NumOfPassedStudentsInLevel = await _context.StudentClassEnrollments
                    .Where(sce => sce.LevelID == LevelID && sce.AcademicTermId == TermID && sce.IsPassed==true)
                    .CountAsync();
            }
            catch (Exception ex)
            {

            }

            return NumOfPassedStudentsInLevel;

        }

        private async Task<int> _GetLastEndedTermIDAsync()
        {
            int termId = 0;

            try
            {
                termId = await _context.AcademicTerms
                    .Where(at => at.EndDate < DateTime.Now)
                    .OrderByDescending(at => at.EndDate) 
                    .Select(at => at.ID)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                
            }

            return termId;
        }



        private async Task<List<(int StageID, string StageName)>> _GetStagesAsync()
        {
            var stages = new List<(int StageID, string StageName)>();

            try
            {
                
                var dbStages = await _context.Stages
                    .Select(s => new { s.ID, s.Name }) 
                    .ToListAsync();

                
                stages = dbStages.Select(s => (s.ID, s.Name)).OrderBy(s=> s.ID).ToList();
            }
            catch (Exception ex)
            {
                
            }

            return stages;
        }

        public async Task<List<StageSuccessFailerInfo>> GetStagesSuccessFailerInfosAsync(int? AcademicTermID)
        {
           
            int termId = AcademicTermID ?? await _GetLastEndedTermIDAsync();

         
            var levelStatsDictionary = await _context.StudentClassEnrollments
                .Where(sce => sce.AcademicTermId == termId)
                .GroupBy(sce => sce.LevelID)
                .Select(g => new
                {
                    LevelID = g.Key,
                    TotalStudents = g.Count(),
                    PassedStudents = g.Count(sce => sce.IsPassed == true)
                })
                .ToDictionaryAsync(k => k.LevelID, v => v);

            var stagesSuccessFailer = new List<StageSuccessFailerInfo>();

           
            var stages = await _GetStagesAsync();

            
            foreach (var stage in stages)
            {
                var stageInfo = new StageSuccessFailerInfo
                {
                    StageID = stage.StageID,       
                    StageName = stage.StageName,   
                    LevelsSuccessFailerInfos = new List<LevelSuccessFailerInfo>()
                };

                
                var levels = await _GetStageLevelsAsync(stage.StageID);

                
                foreach (var level in levels)
                {
                    var levelInfo = new LevelSuccessFailerInfo
                    {
                        LevelID = level.LevelID,
                        LevelName = level.LevelName
                    };

                  
                    if (levelStatsDictionary.TryGetValue(level.LevelID, out var stats))
                    {
                        levelInfo.TotalStudents = stats.TotalStudents;
                        levelInfo.PassedStudentsNumber = stats.PassedStudents;
                        levelInfo.FailedStudentsNumber = stats.TotalStudents - stats.PassedStudents;

                        if (stats.TotalStudents > 0)
                        {
                            levelInfo.SuccessPercentage = (decimal)stats.PassedStudents / stats.TotalStudents * 100m;
                            levelInfo.FailurePercentage = 100m - levelInfo.SuccessPercentage;
                        }
                    }
                    else
                    {
                       
                        levelInfo.TotalStudents = 0;
                        levelInfo.PassedStudentsNumber = 0;
                        levelInfo.FailedStudentsNumber = 0;
                        levelInfo.SuccessPercentage = 0m;
                        levelInfo.FailurePercentage = 0m;
                    }

                    stageInfo.LevelsSuccessFailerInfos.Add(levelInfo);
                }

                stagesSuccessFailer.Add(stageInfo);
            }

            return stagesSuccessFailer;
        }
        public async Task<StudentDegreeInfo> GetStudentDegreeInfosAsync(int studentId)
        {
            var studentDegreeInfo = new StudentDegreeInfo();

            var StudentUserID = await _context.Students
                .Where(s => s.ID == studentId)
                .Select(s => s.UserId)
                .FirstOrDefaultAsync();

            if(StudentUserID == 0)
            {
                return studentDegreeInfo;
            }

            studentDegreeInfo.StudentFullName=_userService.GetUserFullName(StudentUserID);

            int? lastTermId = await _GetLastAcademicTermIdAsync();
            if (lastTermId == null)
            {
                return studentDegreeInfo;
            }
            var LastTermStartDate = await _GetTermStartDateAsync(lastTermId.Value);
            studentDegreeInfo.ExamsDegree = await _context.StudentExamDegrees.Include(sed => sed.Exam)
                .Where(sed => sed.StudentId == studentId && sed.Exam.ActualDate >= LastTermStartDate)
                .Select(sed => new ExamDegreeInfo
                {
                    SubjectName = sed.Exam.Subject.Name,
                    ExamName = sed.Exam.Name,
                    ExamType = sed.Exam.ExamType.ToString(),
                    ExamDate = sed.Exam.ActualDate,
                    TotalDegree = sed.Exam.TotalDegree,
                    ExamDurationMinutes = sed.Exam.DurationInMinutes,
                    StudentDegree = sed.Degree
                })
                .ToListAsync();


            return studentDegreeInfo;
        }


        public async Task<List<SupervisorInfo>> GetOwnerSupervisorsDataAsync()
        {
            var supervisors = new List<SupervisorInfo>();

            var lastTermID = await _GetLastAcademicTermIdAsync();

            var StartDate = await _context.AcademicTerms
                .Where(at => at.ID == lastTermID)
                .Select(at => at.StartDate)
                .FirstOrDefaultAsync();

            var EndDate = await _context.AcademicTerms
               .Where(at => at.ID == lastTermID)
               .Select(at => at.EndDate)
               .FirstOrDefaultAsync();



            supervisors = await _context.Supervisors
                .AsNoTracking()
                .Select(s => new SupervisorInfo
                {
                 
                    ID=s.ID,
                    UserID=s.UserId,
                    PerformanceRating = 90,
                    HireDate = s.HireDate,
                    ExiteDate = s.ExiteDate,
                    FullName=s.User.FullName,
                    AbcenseCount = s.User.Absences
  .Where(a => a.AbsenceDate >= StartDate && a.AbsenceDate <= EndDate)
   .Count(),



                })
                .ToListAsync();

           

            return supervisors;

        }
        public OwnerDashboardViewModel GetOwnerDashboardData(int userId)
        {
           
            var user = _context.Users.Find(userId);
            if (user == null) return null;

            return new OwnerDashboardViewModel
            {
                NavigationViewModel=_userService.GetNavigationData(userId),
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber=user.Phone,
                AddedDate = user.AddedDate
            };
        }

      

        
        
       private async Task<int> _GetStudentLevelOrder(int StudentID, int lastTermId)
        {
            var levelOrder = await _context.StudentClassEnrollments
                .Where(sce => sce.StudentId == StudentID && sce.AcademicTermId == lastTermId)
                .Select(sce => sce.Class.Level.Order)
                .FirstOrDefaultAsync();
            return levelOrder;
       }

        private async Task<int> _GetStudentStageOrder(int StudentID, int lastTermId)
        {
            var stageOrder = await _context.StudentClassEnrollments
                .Where(sce => sce.StudentId == StudentID && sce.AcademicTermId == lastTermId)
                .Select(sce => sce.Class.Level.Stage.Order)
                .FirstOrDefaultAsync();
            return stageOrder;
        }

        private async Task<bool> _IsActiveLastTerm(int studentId)
        {
            int? lastTermId = await _GetLastAcademicTermIdAsync();
            if(lastTermId == null) return false;

            var isActive = _context.StudentClassEnrollments
                .Any(sce => sce.StudentId == studentId && sce.AcademicTermId == lastTermId);
            return isActive;
        }

        private async Task<List<DegreesInfo>> _GetDegreesInfoAsync(int studentId, int lastTermId)
        {

            var LastTermStartDate = await _GetTermStartDateAsync(lastTermId);
            var degrees = await _context.StudentExamDegrees
                .Where(sed => sed.StudentId == studentId && sed.Exam.ActualDate >= LastTermStartDate)
                .Select(sed => new DegreesInfo
                {
                    SubjectName = sed.Exam.Subject.Name,
                    ExamName = sed.Exam.Name,
                    Degree = sed.Degree,
                    totalDegree = sed.Exam.TotalDegree
                })
                .ToListAsync();

            return degrees;
        }

        
        private async Task<int> _GetUserIDForParent(int parentId)
        {
            var userId = await _context.Parents
                .Where(p => p.ID == parentId)
                .Select(p => p.UserId)
                .FirstOrDefaultAsync();

            return userId;
        }

        private async Task<Parent> _GetParentInfo(int parentId)
        {
            var parent = await _context.Parents
                .Where(p => p.ID == parentId)
                .Include(p=>p.User)
                .FirstOrDefaultAsync();
            return parent;
        }
        private async Task<int> _GetAbsencesCount(int UserID)
        {
            return _context.Absences.Count(a => a.UserId == UserID);
        }

        public async Task<List<StudentInfo>> GetStudentsInfoAsync()
        {
            var lastTermID = await _GetLastAcademicTermIdAsync();

            var StartDate = await _context.AcademicTerms
                .Where(at => at.ID == lastTermID)
                .Select(at => at.StartDate)
                .FirstOrDefaultAsync();

            var EndDate = await _context.AcademicTerms
               .Where(at => at.ID == lastTermID)
               .Select(at => at.EndDate)
               .FirstOrDefaultAsync();

            return await _context.Students
                .AsNoTracking()
                .Select(s => new StudentInfo
                {
                    StudentID = s.ID,
                    StudentName = s.User.FullName,
                    StudentPhone = s.User.Phone,
                    ParentPhone = s.Parent.User.Phone,
                    StudentSSN = s.User.SSN,
                    ParentUserID = s.Parent.UserId,
                    ParentRelation = s.ParentRelation,
                    StudentUserID = s.UserId,
                    ExitDate = s.ExiteDate,
                    JoinDate = s.JoinDate,
                  
                    AbsencesCount = s.User.Absences
                    .Where(a => a.AbsenceDate >= StartDate && a.AbsenceDate <= EndDate)
                     .Count(),

                    ParentName = s.Parent.User.FullName,
                    ParentSSN=s.Parent.User.SSN,

                    ClassID = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.ClassId)
                        .FirstOrDefault(),

                    ClassName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Name)
                        .FirstOrDefault(),

                    LevelID = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.LevelID)
                        .FirstOrDefault(),

                    LevelName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.Name)
                        .FirstOrDefault(),

                    StageID = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.StageID)
                        .FirstOrDefault(),

                    StageName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.Stage.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

      

        private async Task<int> _GetStudentClassIdAsync(int studentId, int lastTermId)
        {
            var classId = await _context.StudentClassEnrollments
                .Where(sce => sce.StudentId == studentId && sce.AcademicTermId == lastTermId)
                .Select(sce => sce.ClassId)
                .FirstOrDefaultAsync(); // Tells EF to grab the first match and stop looking

            return classId;
        }

        private async Task<int?> _GetLastAcademicTermIdAsync()
        {
            var lastTermId = await _context.AcademicTerms
                .OrderByDescending(at => at.StartDate)
                .Select(at => (int?)at.ID) 
                .FirstOrDefaultAsync();    

            return lastTermId;
        }

        private async Task<DateTime?> _GetTermStartDateAsync(int termId)
        {
            var startDate = await _context.AcademicTerms
                .Where(at => at.ID == termId)
                .Select(at => (DateTime?)at.StartDate) 
                .FirstOrDefaultAsync();                

            return startDate;
        }

        private async Task<decimal> _GetDegreePercentageOfStudentOfLastTermAsync(int StudentID)
        {
            decimal Percentage = 0;
            int? LastTermID = await _GetLastAcademicTermIdAsync();

            if (LastTermID == null) {
                return 0;
            }

            DateTime? StartTermDate = await _GetTermStartDateAsync(LastTermID.Value);

            if (StartTermDate == null) {
                return 0;
            }

            var studentClassID = await _GetStudentClassIdAsync(StudentID, LastTermID.Value);

            decimal TotalExamsDegree = await _context.MultiChoiceExam
                .Where(mce => mce.ClassId == studentClassID && mce.ActualDate >= StartTermDate)
                .SumAsync(mce => mce.TotalDegree);

            if (TotalExamsDegree == 0)
            {
                return 0;
            }

            decimal totalStudentDegrees = await _context.StudentExamDegrees
            .Where(sed => sed.StudentId == StudentID && sed.Exam.ActualDate >= StartTermDate)
            .SumAsync(sed => sed.Degree);

           

            Percentage= totalStudentDegrees/ TotalExamsDegree * 100;

            return Percentage;
        }

        private async Task<decimal> _GetDoneHomeworkPercentageOfStudentOfLastTermAsync(int StudentID)
        {
            decimal Percentage = 0;
            int? LastTermID = await _GetLastAcademicTermIdAsync();
            if (LastTermID == null)
            {
                return 0;
            }
            DateTime? StartTermDate = await _GetTermStartDateAsync(LastTermID.Value);
            if (StartTermDate == null)
            {
                return 0;
            }
            
            int ClassID=await _GetStudentClassIdAsync(StudentID, LastTermID.Value);
             
            int totalHomeworks = await _context.Homeworks
                .Where(h => h.ClassId == ClassID && h.AddedDate >= StartTermDate)
                .CountAsync();

            if (totalHomeworks == 0)
            {
                return 0;
            }

            int doneHomeworks= await _context.StudentHomeworkAnswers
                .Where(sha => sha.StudentId == StudentID && sha.Homework.ClassId == ClassID && sha.Homework.AddedDate >= StartTermDate)
                .CountAsync();
                
            
            Percentage= doneHomeworks / totalHomeworks *100;

            return Percentage;
        }

        /// <summary>
        /// this function is for calculating the performance of the student in the last term
        /// as 70% for the degree percentage and 30% for the done homework percentage
        /// </summary>
        /// <param name="StudentID"></param>
        /// <returns> return the percentage of performance for the student in the last term</returns>
        private async Task<decimal> _GetPerformanceOfStudentOfLastTermAsync(int StudentID)
        {
           decimal DegreePercentage= await _GetDegreePercentageOfStudentOfLastTermAsync(StudentID);

           decimal DoneHomeworkPercentage= await _GetDoneHomeworkPercentageOfStudentOfLastTermAsync(StudentID);

           decimal Performance = DegreePercentage * 0.7m + DoneHomeworkPercentage * 0.3m;


            return Performance;
        }
        private int _AddAdmin(AdminViewModel model, int userID)
        {
            
            Owner admin = new Owner
            {
                UserId = userID,
               
            };

            _context.Owners.Add(admin);
            _context.SaveChanges();
            return admin.ID;

        }

        private async Task<List<NoteInfo>> _GetStudentsNotes()
        {
           
            var notes = new List<NoteInfo>();

            try
            {
                notes = await _context.Notes
                    .Join(
                        _context.Students,
                        note => note.TargetUserId,
                        student => student.UserId,
                        (note, student) => new NoteInfo 
                        {
                           
                            WriterUserID = note.WriterUserId,
                            TargetUserID = note.TargetUserId,
                            Date = note.AddedDate,
                            Detail = note.NoteDetails
                        }
                    ).ToListAsync();

                foreach (var note in notes)
                {
                    note.WriterName = _userService.GetUserFullName(note.WriterUserID);
                    note.TargetName = _userService.GetUserFullName(note.TargetUserID);

                }
            }
            catch (Exception ex)
            {
                
            }

           
            return notes;
        }

        private async Task<List<NoteInfo>> _GetSupervisorsNotes()
        {

            var notes = new List<NoteInfo>();

            try
            {
                notes = await _context.Notes
                    .Join(
                        _context.Supervisors,
                        note => note.TargetUserId,
                        supervisor => supervisor.UserId,
                        (note, supervisor) => new NoteInfo
                        {

                            WriterUserID = note.WriterUserId,
                            TargetUserID = note.TargetUserId,
                            Date = note.AddedDate,
                            Detail = note.NoteDetails
                        }
                    ).ToListAsync();

                foreach (var note in notes)
                {
                    note.WriterName = _userService.GetUserFullName(note.WriterUserID);
                    note.TargetName = _userService.GetUserFullName(note.TargetUserID);

                }
            }
            catch (Exception ex)
            {

            }


            return notes;
        }

        private async Task<List<NoteInfo>> _GetTeachersNotes()
        {

            var notes = new List<NoteInfo>();

            try
            {
                notes = await _context.Notes
                    .Join(
                        _context.Teachers,
                        note => note.TargetUserId,
                        Teacher => Teacher.UserId,
                        (note, Teacher) => new NoteInfo
                        {

                            WriterUserID = note.WriterUserId,
                            TargetUserID = note.TargetUserId,
                            Date = note.AddedDate,
                            Detail = note.NoteDetails
                        }
                    ).ToListAsync();

                foreach (var note in notes)
                {
                    note.WriterName = _userService.GetUserFullName(note.WriterUserID);
                    note.TargetName = _userService.GetUserFullName(note.TargetUserID);

                }
            }
            catch (Exception ex)
            {
               
            }


            return notes;
        }

        public async Task<OwnerNotesInfo> GetOwnerActorNotesInfo()
        {
            var notes= new OwnerNotesInfo();

            notes.StudentsNotes = await _GetStudentsNotes();
            notes.TeachersNotes = await _GetTeachersNotes();
            notes.SupervisorsNotes = await _GetSupervisorsNotes();


            return notes;

        }
        public bool AddNewAdmin(AdminViewModel model)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    int userID =_userService.AddBaseUser(model, (byte)UserTypeEnum.Owner);
                   // _context.SaveChanges();
                    _AddAdmin(model, userID);
                    _userTypeService.AddUserType(userID, (byte)UserTypeEnum.Owner);
                   // _context.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                return false;
            }



            return true;

        }

        public async Task<List<StageInSchool>> GetStagesInSchoolAsync()
        {
            var stagesInSchool = new List<StageInSchool>();

            try
            {
                // 1. Fetch the raw data from the database using Eager Loading
                // This brings the Stages, their Classes, and their Classes all at once.
                var stagesFromDb = await _context.Stages
                    .Include(s => s.Levels)
                        .ThenInclude(l => l.Classes)
                    .ToListAsync();

                // 2. Map the database entities into your custom ViewModels
                stagesInSchool = stagesFromDb.Select(stage => new StageInSchool
                {
                    Stage = stage,
                    Levels = stage.Levels.Select(level => new LevelInStage
                    {
                        Level = level,
                        // Ensure Classes is converted to a List so the View can read it
                        Classes = level.Classes.ToList()
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {


            }

            return stagesInSchool;
        }
    }
}
