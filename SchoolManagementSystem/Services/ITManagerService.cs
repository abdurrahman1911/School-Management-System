using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.ITManager;
using SchoolManagementSystem.ViewModel.Teacher;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;

namespace SchoolManagementSystem.Services
{
    public class ITManagerService
    {
        readonly AppDbContext _context;
        readonly UserService _userService;
        readonly UserTypeService _userTypeService;
        readonly LogService _logService;
        
        public ITManagerService(AppDbContext context, UserService userService, UserTypeService userTypeService)
        {
            _context = context;
            _userService = userService;
            _userTypeService = userTypeService;
            
        }

        #region By Deghish

        public async Task<List<int>> GetStudentByClassId(int classId, int academictermId)
        {
            return await _context.StudentClassEnrollments
                .Where(sce => sce.ClassId == classId && sce.AcademicTermId == academictermId)
                .Select(sce => sce.StudentId)
                .ToListAsync();
        }

        
        public async Task<bool> RegisterSubjectsForClassAsync(int classId, int academicTermId, List<SubjectsData> subjects)
        {
            var studentsInClass = await GetStudentByClassId(classId, academicTermId);
            var selectedSubjects = subjects.Where(s => s.isSelected).ToList();

            if (!studentsInClass.Any() || !selectedSubjects.Any()) return false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var enrollments = studentsInClass.SelectMany(studentId => selectedSubjects.Select(subject => new StudentsSubjectsEnrollment
                    {
                        StudentId = studentId,
                        SubjectId = subject.SubjectID,
                        TeacherId = subject.selectedTeacherID,
                        AcademicTermId = academicTermId
                    }));

                    await _context.StudentsSubjectsEnrollments.AddRangeAsync(enrollments);
                    await _context.SaveChangesAsync();
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
        public async Task<AcademicYearViewModel>GetAcademicYearAsync()
        {
            try
            {
                var academicYears = await _context.AcademicYears.AsNoTracking().ToListAsync();
                return  new AcademicYearViewModel
                {
                    AcadimicYearList = academicYears.Select(ay => new AcdimicYearItem
                    {
                        Id = ay.ID,
                        Year =ay.Name,
                        StartDate = ay.StartDate,
                        EndDate = ay.EndDate,
                        Status = ay.EndDate < DateTime.Now ? "مغلق" : "مفتوح"

                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                //error page
                throw;
            }
           
        }

        public async Task<AcademicTearmViewModel> GetAcademicTermAsync()
        {
            try
            {
               var academicTerms = await _context.AcademicTerms.Include(at => at.AcademicYear).AsNoTracking().ToListAsync();
                return new AcademicTearmViewModel
                {
                    AcademicTermList = academicTerms.Select(at => new AcdimicTermItem
                    {
                        Id = at.ID,
                        TermName = at.Name,
                        StartDate = at.StartDate,
                        EndDate = at.EndDate,
                        Year = at.AcademicYear.Name,
                        Status = at.EndDate<DateTime.Now ? "مغلق" : "مفتوح"
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                //error page
                throw;
            }
        }
        public async Task<bool> AddAcademicYearAsync(AddOrEditeAcademicYearViewModel model)
        {
            try
            {
                var acadimicYear = new AcademicYear
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                await _context.AcademicYears.AddAsync(acadimicYear);
                await _context.SaveChangesAsync();
              
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddAcademicTermAsync(AddOrUpdateAcademicTermViewModel model)
        {
            try
            {
                var acadimicterm = new AcademicTerm
                {
                    AcademicYearId = model.SelectedAcademicYearId,
                    Name = model.TermNumber == 1 ? "الفصل الدراسي الأول" :
                    model.TermNumber == 2 ? "الفصل الدراسي الثاني" : "الفصل الدراسي الصيفي",
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    TermNumber = model.TermNumber

                };
                await _context.AcademicTerms.AddAsync(acadimicterm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateAcademicYearAsync(int id, AddOrEditeAcademicYearViewModel model)
        {
            try
            {
                var acadimicYear = await _context.AcademicYears.FindAsync(id);
                if (acadimicYear == null)
                    return false;
                acadimicYear.Name = model.Name;
                acadimicYear.StartDate = model.StartDate;
                acadimicYear.EndDate = model.EndDate;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<string> DeleteAcademicYearAsync(int id)
        {
            try
            {
                var acadimicYear = await _context.AcademicYears.FindAsync(id);

                if (acadimicYear == null)
                    return "السنة الدراسية غير موجودة بالفعل.";

                var hasLinkedTerms = _context.AcademicTerms.Any(t => t.AcademicYearId == id);

                if (hasLinkedTerms)
                {
                    return "لا يمكن حذف هذه السنة الدراسية لوجود تيرم دراسي مرتبط بها. برجاء حذف التيرمات المرتبطة أولاً.";
                }

                _context.AcademicYears.Remove(acadimicYear);
                await _context.SaveChangesAsync();

                return "success";
            }
            catch (Exception ex)
            {
                return "حدث خطأ غير متوقع أثناء محاولة الحذف من قاعدة البيانات.";
            }
        }
        public async Task<string> DeleteAcademicTermAsync(int id)
        {
            try
            {
                var acadimicterm = await _context.AcademicTerms.FindAsync(id);

                if (acadimicterm == null)
                    return "الفصل الدراسي غير موجود بالفعل.";

                _context.AcademicTerms.Remove(acadimicterm);
                await _context.SaveChangesAsync();

                return "success";
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                if (ex.InnerException != null &&
                   (ex.InnerException.Message.Contains("REFERENCE constraint") ||
                    ex.InnerException.Message.Contains("foreign key constraint") ||
                    ex.InnerException.Message.Contains("FK_")))
                {
                    return "لا يمكن حذف هذا الترم الدراسي لأنه مرتبط ببيانات أخرى (مثل الفصول أو الطلاب أو المواد). يرجى حذف الارتباطات أولاً ثم المحاولة مرة أخرى.";
                }

                return "حدث خطأ أثناء تحديث قاعدة البيانات.";
            }
            catch (Exception ex)
            {
                return "حدث خطأ غير متوقع أثناء محاولة الحذف من قاعدة البيانات.";
            }
        }
        public async Task<List<IdNameViewModel>> GetAcdemiceYearForDropDownAsync()
        {
            try
            {
                return await _context.AcademicYears
                    .AsNoTracking()
                    .Select(ay => new IdNameViewModel
                    {
                        Id = ay.ID,
                        Name = ay.Name
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                //error page
                throw;
            }
        }
        public async Task<AddOrUpdateAcademicTermViewModel> GetAcademicTermByIdAsync(int id)
        {
            try
            {
                var term = await _context.AcademicTerms.FindAsync(id);
                if (term == null)
                    return null;
                return new AddOrUpdateAcademicTermViewModel
                {
                    SelectedAcademicYearId = term.AcademicYearId,
                    TermNumber = term.TermNumber,
                    StartDate = term.StartDate,
                    EndDate = term.EndDate,
                    AcademicYears = await GetAcdemiceYearForDropDownAsync()

                };

            }
            catch (Exception ex)
            {
                //error page
                throw;
            }
        }
        public async Task<bool> UpdateAcademicTermAsync(int id, AddOrUpdateAcademicTermViewModel model)
        {
            try
            {
                var term = await _context.AcademicTerms.FindAsync(id);
                if (term == null)
                    return false;
                term.AcademicYearId = model.SelectedAcademicYearId;
                term.Name = model.TermNumber == 1 ? "الفصل الدراسي الأول" :
                    model.TermNumber == 2 ? "الفصل الدراسي الثاني": "الفصل الدراسي الصيفي";
                term.TermNumber = model.TermNumber;
                term.StartDate = model.StartDate;
                term.EndDate = model.EndDate;
                await _context.SaveChangesAsync();
                return true;
            } 
            catch
            {
                return false;
            }
        }
        public async Task<bool> MangeSuccessAsync(int classid, int currentTermId,int nextTermId,List< ClassStudentsData> model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var successStudentIds = model.Where(m => m.IsSuccess == true).Select(s => s.StudentId).ToList();
                    var failedStudentIds = model.Where(m => m.IsSuccess == false).Select(s => s.StudentId).ToList();

                    // 1. Update the IsPassed status for students in the current term
                    var enrollmentsToUpdate = await _context.StudentClassEnrollments
                        .Where(s => s.AcademicTermId == currentTermId && successStudentIds.Contains(s.StudentId)).ToListAsync();
                    foreach (var enrollment in enrollmentsToUpdate) { enrollment.IsPassed = true; }

                    
                    int termNumber = await _context.AcademicTerms.Where(at => at.ID == currentTermId).Select(at => at.TermNumber).FirstOrDefaultAsync();
                    // 2. Conditional logic based on termNumber
                    if (termNumber == 1)
                    {
                        // If term is 1: Students stay in the same class and level
                        var allEnrollments = await _context.StudentClassEnrollments
                            .Where(s => s.AcademicTermId == currentTermId && successStudentIds.Concat(failedStudentIds).Contains(s.StudentId)).ToListAsync();

                        foreach (var old in allEnrollments)
                        {
                            _context.StudentClassEnrollments.Add(new StudentClassEnrollment
                            {
                                StudentId = old.StudentId,
                                ClassId = old.ClassId,
                                LevelID = old.LevelID,
                                AcademicTermId = nextTermId,
                                IsPassed = false
                            });
                        }
                    }
                    else
                    {
                        // If term is not 1: Apply promotion logic for successful students and re-enroll failed students

                        // Re-enroll failed students in their current class/level
                        var failedEnrollments = await _context.StudentClassEnrollments
                            .Where(s => s.AcademicTermId == currentTermId && failedStudentIds.Contains(s.StudentId)).ToListAsync();
                        foreach (var old in failedEnrollments)
                        {
                            _context.StudentClassEnrollments.Add(new StudentClassEnrollment
                            {
                                StudentId = old.StudentId,
                                ClassId = old.ClassId,
                                LevelID = old.LevelID,
                                AcademicTermId = nextTermId,
                                IsPassed = false
                            });
                        }

                        // Enroll successful students into the next level with randomized class distribution
                        var studentsToEnroll = await _context.StudentClassEnrollments
                            .Include(s => s.Level)
                            .Where(s => s.AcademicTermId == currentTermId && successStudentIds.Contains(s.StudentId))
                            .ToListAsync();

                        var currentLevelOrder = studentsToEnroll.Select(s => s.Level.Order).FirstOrDefault();
                        var newLevelId = await _context.Levels.Where(l => l.Order == currentLevelOrder + 1)
                            .Select(s => s.ID).FirstOrDefaultAsync();
                        var newClasses = await _context.Classes.Where(c => c.LevelID == newLevelId).ToListAsync();

                        if (newClasses.Any())
                        {
                            int classCount = newClasses.Count;
                            for (int i = 0; i < studentsToEnroll.Count; i++)
                            {
                                _context.StudentClassEnrollments.Add(new StudentClassEnrollment
                                {
                                    StudentId = studentsToEnroll[i].StudentId,
                                    ClassId = newClasses[i % classCount].ID,
                                    LevelID = newLevelId,
                                    AcademicTermId = nextTermId,
                                    IsPassed = false
                                });
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    // Rollback changes in case of any error
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        #endregion

        public List<string> ValidateUserData(UserInfo model)
        {
            List<string> errors = new();

            if (string.IsNullOrEmpty(model.FirstName))
                errors.Add("الاسم الأول مطلوب");

            if (string.IsNullOrEmpty(model.LastName))
                errors.Add("الاسم الأخير مطلوب");

            if (string.IsNullOrEmpty(model.Phone))
                errors.Add("رقم الهاتف مطلوب");

            if (string.IsNullOrEmpty(model.SSN))
                errors.Add("الرقم القومي مطلوب");

            if (string.IsNullOrEmpty(model.Governorate))
                errors.Add("المحافظة مطلوبة");

            if (string.IsNullOrEmpty(model.City))
                errors.Add("المدينة مطلوبة");

            if (string.IsNullOrEmpty(model.Nationality))
                errors.Add("الجنسية مطلوبة");

            return errors;
        }

        public bool isUserRequireDataRight(RequireInfo requireInfo)
        {
            return _context.Users.Any(u => u.SSN == requireInfo.SSN && u.Phone== requireInfo.PhoneNumber);
        }
        public bool isStudentExist(string SSN)
        {
          

            return _context.Students.Include(s=> s.User).Any(s=> s.User.SSN==SSN);
        }

        public bool isTeacherExist(string SSN)
        {
           

            return _context.Teachers.Include(t => t.User).Any(t => t.User.SSN == SSN);
        }

        public bool isParentExist(string SSN)
        {
           

            return _context.Parents.Include(t => t.User).Any(t => t.User.SSN == SSN);
        }

        public bool isSupervisorExist(string SSN)
        {
           

            return _context.Supervisors.Include(t => t.User).Any(t => t.User.SSN == SSN);
        }

        public bool isHeadmasterExist(string SSN)
        {
           

            return _context.Headmasters.Include(t => t.User).Any(t => t.User.SSN == SSN);
        }

        public bool isITManagerExist(string SSN)
        {
           

            return _context.Admins.Include(t => t.User).Any(t => t.User.SSN == SSN);
        }
        public bool isUserExist(string SSN)
        {
            return _context.Users.Any(u => u.SSN == SSN);
        }

        private async Task<int> _SaveNewUser(UserInfo userData)
        {
            


            try
            {
                var userModel = new User
                {
                    FirstName = userData.FirstName,
                    SecondName = userData.SecondName,
                    ThirdName = userData.ThirdName,
                    LastName = userData.LastName,
                    Phone = userData.Phone,
                    Email = userData.Email,
                    Password = clsBCrypt.GetHash( userData.SSN),
                    SSN = userData.SSN,
                    BirthDate = userData.BirthDate,
                    AddedDate = DateTime.Now,
                    Governorate = userData.Governorate,
                    City = userData.City,
                    Street = userData.Street,
                    Area = userData.Area,
                    Gender = userData.Gender,
                    Nationality = userData.Nationality,
                    ProfilPhotoURL = userData.ProfilePhotoUrl
                };

                await _context.Users.AddAsync(userModel);

                await _context.SaveChangesAsync();

                return userModel.ID;

            }
            catch (Exception)
            {
                throw;
            }


           
        }


        private async Task<bool> _UpdateUserAsync(int userId, UserInfo userData)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);

                if (user == null)
                    return false;

                user.FirstName = userData.FirstName;
                user.SecondName = userData.SecondName;
                user.ThirdName = userData.ThirdName;
                user.LastName = userData.LastName;
                user.Phone = userData.Phone;
                user.Email = userData.Email;
                user.SSN = userData.SSN;
                user.BirthDate = userData.BirthDate;
                user.Governorate = userData.Governorate;
                user.City = userData.City;
                user.Street = userData.Street;
                user.Area = userData.Area;
                user.Gender = userData.Gender;
                user.Nationality = userData.Nationality;
                if (!string.IsNullOrEmpty(userData.ProfilePhotoUrl))
                    user.ProfilPhotoURL = userData.ProfilePhotoUrl;



                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<StudentInfo> _GetStudentInfo(int StudentID)
        {
            var lastTermID = await _GetLastTermIDAsync();

            return await _context.Students
                .AsNoTracking()
                .Where(s => s.ID == StudentID)
                .Select(s => new StudentInfo
                {
                    ID = s.ID,
                    StudentName = s.User.FullName,
                    PhoneNumber = s.User.Phone,
                    SSN = s.User.SSN,
                    JoinDate = s.JoinDate,

                    ParentID = s.parentId,
                    ParentName = s.Parent.User.FullName,

                    ClassID = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.ClassId)
                        .FirstOrDefault(),

                    ClassName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Name)
                        .FirstOrDefault(),

                    LevelId = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.LevelID)
                        .FirstOrDefault(),

                    LevelName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.Name)
                        .FirstOrDefault(),

                    StageId = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.StageID)
                        .FirstOrDefault(),

                    StageName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.Stage.Name)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        }

        private async Task<ParentInfo> _GetParentInfo(int ParentID)
        {
            var ParentInfo = new ParentInfo();

            try
            {

                ParentInfo = await _context.Parents
                    .Include(s => s.User)
                    .Where(s => s.ID == ParentID)
                    .Select(s => new ParentInfo
                    {
                        Name = s.User.FullName,
                        ID = s.ID,
                        Phone = s.User.Phone,
                        SSN = s.User.SSN,
                        
                    })
                    .FirstOrDefaultAsync();


                if (ParentInfo != null)
                {
                    ParentInfo.SonsNumber = await _context.Students
                        .Where(s => s.parentId == ParentID)
                        .CountAsync();
                }

                return ParentInfo;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        private async Task<List<int>> _GetSchoolStudentsIDs()
        {
            

            try
            {
                var studentsIDs = new List<int>();

                studentsIDs = await _context.Students
                    .Select(s => s.ID)
                    .ToListAsync();

                return studentsIDs;

            }
            catch (Exception ex)
            {
                throw;
            }

            
        }

        private async Task<List<int>> _GetSchoolParentsIDs()
        {


            try
            {
                var parentsIDs = new List<int>();

                parentsIDs = await _context.Parents
                    .Select(s => s.ID)
                    .ToListAsync();

                return parentsIDs;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public async Task<List<StudentInfo>> GetStudentsInfoAsync()
        {
            var lastTermID = await _GetLastTermIDAsync();

            return await _context.Students
                .AsNoTracking()
                .Select(s => new StudentInfo
                {
                    ID = s.ID,
                    StudentName = s.User.FullName,
                    PhoneNumber = s.User.Phone,
                    SSN = s.User.SSN,
                    JoinDate = s.JoinDate,         
                    ParentID = s.parentId,
                    ParentName = s.Parent.User.FullName,

                    ClassID = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.ClassId)
                        .FirstOrDefault(),

                    ClassName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Name)
                        .FirstOrDefault(),

                    LevelId = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.LevelID)
                        .FirstOrDefault(),

                    LevelName = s.StudentClassEnrollments
                        .Where(e => e.AcademicTermId == lastTermID)
                        .Select(e => e.Class.Level.Name)
                        .FirstOrDefault(),

                    StageId = s.StudentClassEnrollments
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


        public async Task<List<SupervisorInfo>> GetSupervisorsInfoAsync()
        {

            return await _context.Supervisors
                .AsNoTracking()
                .Select(s => new SupervisorInfo
                {
                    ID = s.ID,
                    Name = s.User.FullName,
                    PhoneNumber = s.User.Phone,
                    SSN = s.User.SSN,
                    HireDate = s.HireDate,
                    ExitDate=s.ExiteDate 
                 
                })
                .ToListAsync();
        }

        public async Task<List<HeadmasterInfo>> GetHeadmastersInfoAsync()
        {

            return await _context.Headmasters
                .AsNoTracking()
                .Select(s => new HeadmasterInfo
                {
                    ID = s.ID,
                    Name = s.User.FullName,
                    PhoneNumber = s.User.Phone,
                    SSN = s.User.SSN,
                    HireDate = s.HireDate,
                    ExitDate = s.ExiteDate

                })
                .ToListAsync();
        }

        public async Task<List<ITManagerInfo>> GetITManagersInfoAsync()
        {

            return await _context.Admins
                .AsNoTracking()
                .Select(s => new ITManagerInfo
                {
                    ID = s.ID,
                    Name = s.User.FullName,
                    PhoneNumber = s.User.Phone,
                    SSN = s.User.SSN,
                    HireDate = s.HireDate,
                    ExitDate = s.ExiteDate

                })
                .ToListAsync();
        }

        public async Task<List<ParentInfo>> GetParentsInfoAsync()
        {

            return await _context.Parents
               .AsNoTracking()
               .Select(s => new ParentInfo
               {
                   Name = s.User.FullName,
                   ID = s.ID,
                   Phone = s.User.Phone,
                   SSN = s.User.SSN,
                   SonsNumber = s.Students.Count()
                 

               })
               .ToListAsync();
       
        }

        public async Task<bool> isManageSuccessClassDataRight(int stageid, int levelid, int classid, int currentTermID)
        {

            return _context.StudentClassEnrollments.Any(sce => sce.AcademicTermId == currentTermID && sce.LevelID == levelid && sce.ClassId == classid);
        }
        private async Task<int> _GetAdminIDFromUserID(int userID)
        {
            var adminId = await _context.Admins
                .Where(a => a.UserId == userID)
                .Select(a => a.ID)
                .FirstOrDefaultAsync();

            if (adminId == 0)
            {
                // Fallback: If current user is not an Admin (e.g., Owner or IT), assign to the first available Admin
                adminId = await _context.Admins.Select(a => a.ID).FirstOrDefaultAsync();
            }

            return adminId;
        }

        private async Task<int> _SaveNewStudent(StudentData studentData,int AdminID, StudentEnrollmentInfo enrollmentData)
        {
           

            try
            {
                var model = new Student
                {
                    UserId = studentData.StudentUserID,
                    AdminId = AdminID,
                    IsGraduated = false,
                    ExiteDate = null,
                    parentId = studentData.ParentID,
                    ParentRelation = studentData.ParentRelation,
                    JoinDate = studentData.JoinDate,


                };

                await _context.Students.AddAsync(model);
                await _context.SaveChangesAsync();

                //add to the userUserTypes table
                if (!_context.UserUserTypes.Any(x =>
                        x.UserId == model.UserId &&
                        x.UserTypeId == (byte)UserTypeEnum.Student))
                {
                    await _context.UserUserTypes.AddAsync(new UserUserType
                    {
                        UserId = model.UserId,
                        UserTypeId = (byte)UserTypeEnum.Student
                    });
                }


                var enrollmentModel = new StudentClassEnrollment
                {
                    StudentId = model.ID,
                    LevelID = enrollmentData.LevelID,
                    ClassId = enrollmentData.ClassID,
                    AcademicTermId = await _GetLastTermIDAsync()
                };

                await _context.StudentClassEnrollments.AddAsync(enrollmentModel);

                await _context.SaveChangesAsync();

                return model.ID;

            }
            catch (Exception)
            {
                throw;
            }


            

        }

        private async Task<bool> _UpdateStudent(StudentData studentData, int adminID, StudentEnrollmentInfo enrollmentData)
        {
            try
            {
                var model = await _context.Students
                    .FirstOrDefaultAsync(s => s.ID == studentData.StudentID);

                if (model == null)
                    return false;

                model.UserId = studentData.StudentUserID;
                model.AdminId = adminID;
                model.parentId = studentData.ParentID;
                model.ParentRelation = studentData.ParentRelation;
                model.JoinDate = studentData.JoinDate;
                model.ExiteDate = studentData.ExitDate;
                model.IsGraduated = studentData.isGraduated;

               


                var oldEnrollmentData = await GetStudentEntrollmentinfoAsync(model.ID);

                if(oldEnrollmentData != enrollmentData)
                {
                    int LastTermID=await _GetLastTermIDAsync();
                    var oldmodel = await _context.StudentClassEnrollments
                        .Where(sce => sce.StudentId == studentData.StudentID && sce.AcademicTermId == LastTermID)
                        .FirstOrDefaultAsync();

                    if (oldmodel != null)
                    {
                        oldmodel.LevelID = enrollmentData.LevelID;
                        oldmodel.ClassId = enrollmentData.ClassID;
                        oldmodel.StudentId = studentData.StudentID;
                        oldmodel.AcademicTermId = LastTermID;
                    }
                    else
                    {
                        var enrollmentModel = new StudentClassEnrollment
                        {
                            StudentId = model.ID,
                            LevelID = enrollmentData.LevelID,
                            ClassId = enrollmentData.ClassID,
                            AcademicTermId = await _GetLastTermIDAsync()
                        };

                        await _context.StudentClassEnrollments.AddAsync(enrollmentModel);
                    }


                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<int> _GetUserIDFromSSN(string SSN)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.SSN == SSN)
                    .Select(u => u.ID)
                    .FirstOrDefaultAsync();
            } catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<bool> SaveUpdatStudent(
     UserInfo userData,
     StudentData studentData,
     StudentEnrollmentInfo enrollmentData,
     int LoginUserID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int studentUserID;

               
                if (isUserExist(userData.SSN))
                {
                    studentUserID = await _GetUserIDFromSSN(userData.SSN);
                    await _UpdateUserAsync(studentUserID, userData);
                }
                else
                {
                    studentUserID = await _SaveNewUser(userData);
                }

                int adminID = await _GetAdminIDFromUserID(LoginUserID);
                int studentID;

                studentData.ParentID = await _context.Parents
                     .Include(p => p.User)
                     .Where(p => p.User.SSN == studentData.ParentSSN)
                     .Select(p => p.ID)
                     .FirstOrDefaultAsync();

              

                studentData.StudentUserID=studentUserID;

                if (isStudentExist(userData.SSN))
                {
                    studentData.StudentID = await _context.Students.Include(s => s.User)
                        .Where(s => s.User.SSN == userData.SSN)
                        .Select(s => s.ID)
                        .FirstOrDefaultAsync();

                    bool isDone=await _UpdateStudent(studentData, adminID,enrollmentData);
                    studentID = studentData.StudentID;
                }
                else
                {
                    studentID = await _SaveNewStudent(studentData, adminID, enrollmentData);
                }
               
                

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }

           
        }


        public List<string> ValidateStudentData(StudentData model)
        {
            List<string> errors = new();

            if (!isParentExist(model.ParentSSN))
                errors.Add("ولي الأمر المدخل غير موجود");

            if (model.ParentRelation == null)
                errors.Add("علاقة ولي الأمر بالطالب مطلوبة");



            return errors;
        }

       

        public List<string> ValidateStudentEnrollmentInfoData(StudentEnrollmentInfo model)
        {

            List<string> errors = new();

            if (model.StageID == 0)
                errors.Add("المرحلة الدراسية مطلوبة");

            if (model.LevelID == 0)
                errors.Add("المستوى الدراسي مطلوب");

            if (model.ClassID == 0)
                errors.Add("الفصل الدراسي مطلوب");


            return errors;
        }
        private async Task<int> _SaveNewTeacher(TeacherData teacherData, int AdminID)
        {


            try
            {
                var model = new Models.Teacher
                {
                    UserId = teacherData.TeacherUserID,
                    AdminId = AdminID,
                    ExiteDate = null,
                    HireDate = teacherData.HireDate
                };

                await _context.Teachers.AddAsync(model);
                await _context.SaveChangesAsync();

                //add to the userUserTypes table
                if (!_context.UserUserTypes.Any(x =>
                        x.UserId == model.UserId &&
                        x.UserTypeId == (byte)UserTypeEnum.Teacher))
                {
                    await _context.UserUserTypes.AddAsync(new UserUserType
                    {
                        UserId = model.UserId,
                        UserTypeId = (byte)UserTypeEnum.Teacher
                    });
                }


                await _context.SaveChangesAsync();

                return model.ID;

            }
            catch (Exception)
            {
                throw;
            }




        }
        private async Task<bool> _UpdateTeacher(TeacherData teacherData, int adminID)
        {
            try
            {
                var model = await _context.Teachers
                    .FirstOrDefaultAsync(t => t.ID == teacherData.TeacherID);

                if (model == null)
                    return false;

                model.UserId = teacherData.TeacherUserID;
                model.AdminId = adminID;
                model.HireDate = teacherData.HireDate;
                model.ExiteDate = teacherData.ExitDate;
             

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> SaveUpdatTeacher(
   UserInfo userData,
   TeacherData teacherData, 
   int LoginUserID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int teacherUserID;


                if (isUserExist(userData.SSN))
                {
                    teacherUserID = await _GetUserIDFromSSN(userData.SSN);
                    await _UpdateUserAsync(teacherUserID, userData);
                }
                else
                {
                    teacherUserID = await _SaveNewUser(userData);
                }

                int adminID = await _GetAdminIDFromUserID(LoginUserID);
                int teacherID;




                teacherData.TeacherUserID = teacherUserID;

                if (isTeacherExist(userData.SSN))
                {
                    teacherData.TeacherID = await _context.Teachers.Include(t => t.User)
                        .Where(t => t.User.SSN == userData.SSN)
                        .Select(t => t.ID)
                        .FirstOrDefaultAsync();

                    bool isDone = await _UpdateTeacher(teacherData, adminID);
                    teacherID = teacherData.TeacherID;
                }
                else
                {
                    teacherID = await _SaveNewTeacher(teacherData, adminID);
                }



                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }


        }

        private async Task<bool> _UpdateParent(ParentData parentData, int adminID)
        {
            try
            {
                var model = await _context.Parents
                    .FirstOrDefaultAsync(t => t.ID == parentData.ParentID);

                if (model == null)
                    return false;

                model.UserId = parentData.ParentUserID;
                model.AdminId = adminID;
              


                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> _UpdateSupervisor(SupervisorData supervisorData, int adminID)
        {
            try
            {
                var model = await _context.Supervisors
                    .FirstOrDefaultAsync(t => t.ID == supervisorData.ID);

                if (model == null)
                    return false;

                model.UserId = supervisorData.UserID;
                model.HireDate= supervisorData.HireDate;
                model.ExiteDate = supervisorData.ExitDate;
                model.AdminId = adminID;



                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }


        private async Task<bool> _UpdateHeadmaster(HeadmasterData headmasterData, int adminID)
        {
            try
            {
                var model = await _context.Headmasters
                    .FirstOrDefaultAsync(t => t.ID == headmasterData.ID);

                if (model == null)
                    return false;

                model.UserId = headmasterData.UserID;
                model.HireDate = headmasterData.HireDate;
                model.ExiteDate = headmasterData.ExitDate;
                model.AdminId = adminID;



                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> _UpdateITManager(ITManagerData ITManagerData, int adminID)
        {
            try
            {
                var model = await _context.Admins
                    .FirstOrDefaultAsync(t => t.ID == ITManagerData.ID);

                if (model == null)
                    return false;

                model.UserId = ITManagerData.UserID;
                model.HireDate = ITManagerData.HireDate;
                model.ExiteDate = ITManagerData.ExitDate;
               



                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<int> _SaveNewParent(ParentData parentData, int AdminID)
        {


            try
            {
                var model = new Models.Parent
                {
                    UserId = parentData.ParentUserID,
                    AdminId = AdminID,
                  
                };

                await _context.Parents.AddAsync(model);
                await _context.SaveChangesAsync();

                //add to the userUserTypes table
                if (!_context.UserUserTypes.Any(x =>
                        x.UserId == model.UserId &&
                        x.UserTypeId == (byte)UserTypeEnum.Parent))
                {
                    await _context.UserUserTypes.AddAsync(new UserUserType
                    {
                        UserId = model.UserId,
                        UserTypeId = (byte)UserTypeEnum.Parent
                    });
                }


                await _context.SaveChangesAsync();

                return model.ID;

            }
            catch (Exception)
            {
                throw;
            }




        }

        private async Task<int> _SaveNewSupervisor(SupervisorData supervisorData, int AdminID)
        {


            try
            {
                var model = new Models.Supervisor
                {
                    UserId = supervisorData.UserID,
                    HireDate=supervisorData.HireDate,
                    AdminId = AdminID,

                };

                await _context.Supervisors.AddAsync(model);
                await _context.SaveChangesAsync();

                //add to the userUserTypes table
                if (!_context.UserUserTypes.Any(x =>
                        x.UserId == model.UserId &&
                        x.UserTypeId == (byte)UserTypeEnum.Supervisor))
                {
                    await _context.UserUserTypes.AddAsync(new UserUserType
                    {
                        UserId = model.UserId,
                        UserTypeId = (byte)UserTypeEnum.Supervisor
                    });
                }


                await _context.SaveChangesAsync();

                return model.ID;

            }
            catch (Exception)
            {
                throw;
            }




        }


        private async Task<int> _SaveNewHeadmaster(HeadmasterData HeadmasterData, int AdminID)
        {


            try
            {
                var model = new Models.Headmaster
                {
                    UserId = HeadmasterData.UserID,
                    HireDate = HeadmasterData.HireDate,
                    AdminId = AdminID,

                };

                await _context.Headmasters.AddAsync(model);
                await _context.SaveChangesAsync();

                //add to the userUserTypes table
                if (!_context.UserUserTypes.Any(x =>
                        x.UserId == model.UserId &&
                        x.UserTypeId == (byte)UserTypeEnum.Headmaster))
                {
                    await _context.UserUserTypes.AddAsync(new UserUserType
                    {
                        UserId = model.UserId,
                        UserTypeId = (byte)UserTypeEnum.Headmaster
                    });
                }


                await _context.SaveChangesAsync();

                return model.ID;

            }
            catch (Exception)
            {
                throw;
            }




        }

        private async Task<int> _SaveNewITManager(ITManagerData ITManagerData, int AdminID)
        {


            try
            {
                var model = new Models.Admin
                {
                    UserId = ITManagerData.UserID,
                    HireDate = ITManagerData.HireDate,
                  

                };

                await _context.Admins.AddAsync(model);
                await _context.SaveChangesAsync();

                //add to the userUserTypes table
                if (!_context.UserUserTypes.Any(x =>
                        x.UserId == model.UserId &&
                        x.UserTypeId == (byte)UserTypeEnum.IT))
                {
                    await _context.UserUserTypes.AddAsync(new UserUserType
                    {
                        UserId = model.UserId,
                        UserTypeId = (byte)UserTypeEnum.IT
                    });
                }


                await _context.SaveChangesAsync();

                return model.ID;

            }
            catch (Exception)
            {
                throw;
            }




        }
        public async Task<bool> SaveUpdatParent(
   UserInfo userData,
   ParentData ParentData,
   int LoginUserID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int ParentUserID;


                if (isUserExist(userData.SSN))
                {
                    ParentUserID = await _GetUserIDFromSSN(userData.SSN);
                    await _UpdateUserAsync(ParentUserID, userData);
                }
                else
                {
                    ParentUserID = await _SaveNewUser(userData);
                }

                int adminID = await _GetAdminIDFromUserID(LoginUserID);
                int ParentID;


                if(ParentData==null)
                    ParentData=new ParentData();

                ParentData.ParentUserID = ParentUserID;

                if (isParentExist(userData.SSN))
                {
                    ParentData.ParentID = await _context.Parents.Include(t => t.User)
                        .Where(t => t.User.SSN == userData.SSN)
                        .Select(t => t.ID)
                        .FirstOrDefaultAsync();

                    bool isDone = await _UpdateParent(ParentData, adminID);
                    ParentID = ParentData.ParentID;
                }
                else
                {
                    ParentID = await _SaveNewParent(ParentData, adminID);
                }



                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }


        }

        public async Task<bool> SaveUpdatSupervisor(
   UserInfo userData,
   SupervisorData SupervisorData,
   int LoginUserID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int SupervisorUserID;


                if (isUserExist(userData.SSN))
                {
                    SupervisorUserID = await _GetUserIDFromSSN(userData.SSN);
                    await _UpdateUserAsync(SupervisorUserID, userData);
                }
                else
                {
                    SupervisorUserID = await _SaveNewUser(userData);
                }

                int adminID = await _GetAdminIDFromUserID(LoginUserID);
                int SupervisorID;


                if (SupervisorData == null)
                    SupervisorData = new SupervisorData();

                SupervisorData.UserID = SupervisorUserID;

                if (isSupervisorExist(userData.SSN))
                {
                    SupervisorData.ID = await _context.Supervisors.Include(t => t.User)
                        .Where(t => t.User.SSN == userData.SSN)
                        .Select(t => t.ID)
                        .FirstOrDefaultAsync();

                    bool isDone = await _UpdateSupervisor(SupervisorData, adminID);
                    SupervisorID = SupervisorData.ID;
                }
                else
                {
                    SupervisorID = await _SaveNewSupervisor(SupervisorData, adminID);
                }



                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }


        }

        public async Task<bool> SaveUpdatHeadmaster(
   UserInfo userData,
   HeadmasterData HeadmasterData,
   int LoginUserID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int HeadmasterUserID;


                if (isUserExist(userData.SSN))
                {
                    HeadmasterUserID = await _GetUserIDFromSSN(userData.SSN);
                    await _UpdateUserAsync(HeadmasterUserID, userData);
                }
                else
                {
                    HeadmasterUserID = await _SaveNewUser(userData);
                }

                int adminID = await _GetAdminIDFromUserID(LoginUserID);
                int SupervisorID;


                if (HeadmasterData == null)
                    HeadmasterData = new HeadmasterData();

                HeadmasterData.UserID = HeadmasterUserID;

                if (isHeadmasterExist(userData.SSN))
                {
                    HeadmasterData.ID = await _context.Headmasters.Include(t => t.User)
                        .Where(t => t.User.SSN == userData.SSN)
                        .Select(t => t.ID)
                        .FirstOrDefaultAsync();

                    bool isDone = await _UpdateHeadmaster(HeadmasterData, adminID);
                    SupervisorID = HeadmasterData.ID;
                }
                else
                {
                    SupervisorID = await _SaveNewHeadmaster(HeadmasterData, adminID);
                }



                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }


        }

        public async Task<bool>  SaveUpdatITManager(
 UserInfo userData,
 ITManagerData ITManagerData,
 int LoginUserID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int ITManagerUserID;


                if (isUserExist(userData.SSN))
                {
                    ITManagerUserID = await _GetUserIDFromSSN(userData.SSN);
                    await _UpdateUserAsync(ITManagerUserID, userData);
                }
                else
                {
                    ITManagerUserID = await _SaveNewUser(userData);
                }

                int adminID = await _GetAdminIDFromUserID(LoginUserID);
                int ITManagerID;


                if (ITManagerData == null)
                    ITManagerData = new ITManagerData();

                ITManagerData.UserID = ITManagerUserID;

                if (isITManagerExist(userData.SSN))
                {
                    ITManagerData.ID = await _context.Admins.Include(t => t.User)
                        .Where(t => t.User.SSN == userData.SSN)
                        .Select(t => t.ID)
                        .FirstOrDefaultAsync();

                    bool isDone = await _UpdateITManager(ITManagerData, adminID);
                    ITManagerID = ITManagerData.ID;
                }
                else
                {
                    ITManagerID = await _SaveNewITManager(ITManagerData, adminID);
                }



                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                return false;
            }


        }

        public async Task<List<TeacherInfo>> GetTeachersInfoAsync()
        {
            var teachers = await _context.Teachers
                .Include(t=> t.User)
                .Select(t => new TeacherInfo
                {
                    TeacherID = t.ID,
                    TeacherName=t.User.FullName,
                    HireDate=t.HireDate,
                    TeacherPhone=t.User.Phone,
                    TeacherSSN=t.User.SSN,
                    
                 
                })
                .ToListAsync();

            
            return teachers;
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
                .Where(ts => ts.TeacherId == teacherId)
                .Include(ts => ts.Subject)
                .Select(ts => ts.Subject)
                .Distinct()
                .ToListAsync();

            return teacherSubjects;
        }
        public async Task<LoginITManagerInfo> GetLoginITManagerHomeInfoAsync(int LoginUserID)
        {
           

            var UserData= await _userService.GetUserDataInfoForPresentationAsync(LoginUserID);

            var LoginITManagerInfo = new LoginITManagerInfo
            { 
                Email = UserData.Email,
                Phone = UserData.Phone,
                JoinDate = UserData.AddedDate
            };

            return LoginITManagerInfo;
        }

        private async Task<int> _GetSatgeIDOfSpecificLevelID(int LevelID)
        {
            int StageID = 0;

            try
            {

                StageID= await _context.Levels
                    .Where(l=> l.ID == LevelID)
                    .Select(l=> l.StageID)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

            }

            return StageID;
        }
        //update this method to return the full data of the class.

        private async Task<string> _GetParentSSNAsync(int ParentID)
        {
            string SSN ="";

            try
            {
                SSN = await _context.Parents
                    .Include(p=> p.User)
                    .Where(p=> p.ID==ParentID )
                    .Select(p => p.User.SSN)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

            }

            return SSN;
        }

        private async Task<string> _GetParentPhoneAsync(int ParentID)
        {
            string Phone = "";

            try
            {
                Phone = await _context.Parents
                    .Include(p => p.User)
                    .Where(p => p.ID == ParentID)
                    .Select(p => p.User.Phone)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

            }

            return Phone;
        }

        private async Task<int> _GetStudentClassIDAsync(int StudentID)
        {
            int StudentClassID = 0;

            int LastTermID = await _GetLastTermIDAsync();
            try
            {


                StudentClassID=await _context.StudentClassEnrollments
                    .Where(sce=> sce.StudentId==StudentID && sce.AcademicTermId==LastTermID)
                    .Select(sce=> sce.ClassId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return StudentClassID;
        }

        private async Task<int> _GetStudentLevelIDAsync(int StudentID)
        {
            int StudentLevelID = 0;

            int LastTermID = await _GetLastTermIDAsync();
            try
            {


                StudentLevelID = await _context.StudentClassEnrollments
                    .Where(sce => sce.StudentId == StudentID && sce.AcademicTermId == LastTermID)
                    .Select(sce => sce.LevelID)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return StudentLevelID;
        }

        private async Task<int> _GetStudentStageIDAsync(int StudentID)
        {
            int StudentStageID = 0;


            int LevelID = await _GetStudentLevelIDAsync(StudentID);


            try
            {


                StudentStageID = await _context.Levels
                    .Where(l=> l.ID==LevelID)
                    .Select(l => l.StageID)
                    .FirstOrDefaultAsync();


            }
            catch (Exception ex)
            {

            }

            return StudentStageID;
        }
        public async Task<StudentData> GetStudentDataAsync(int StudentUserID)
        {
            var StudentData = new StudentData();

           

            try
            {
                StudentData = await _context.Students
                    .Where(s => s.UserId == StudentUserID)
                    .Select(s => new StudentData
                    {
                        StudentUserID=StudentUserID,           
                        ParentRelation=s.ParentRelation,
                        JoinDate=s.JoinDate,
                        StudentID=s.ID,
                        ParentID=s.parentId,
                        ExitDate=s.ExiteDate,
                        isGraduated=s.IsGraduated
                        
                       
                        

                    }).FirstOrDefaultAsync();

                if (StudentData != null)
                {

                    StudentData.ParentSSN = await _GetParentSSNAsync(StudentData.ParentID);
                  

                }


            }
            catch (Exception ex)
            {
            }

            if (StudentData == null)
            {
                StudentData = new StudentData
                {
                    StudentUserID = StudentUserID,
                    JoinDate=DateTime.Now

                };
            }

            return StudentData;
        }

        public async Task<TeacherData> GetTeacherDataAsync(int TeacherUserID)
        {
            var TeacherData = new TeacherData();



            try
            {
                TeacherData = await _context.Teachers
                    .Where(t => t.UserId == TeacherUserID)
                    .Select(t => new TeacherData
                    {
                        TeacherUserID = TeacherUserID,
                        HireDate = t.HireDate,
                        ExitDate=t.ExiteDate,
                        TeacherID = t.ID,
                        



                    }).FirstOrDefaultAsync();

               


            }
            catch (Exception ex)
            {
            }

            if (TeacherData == null)
            {
                TeacherData = new TeacherData
                {
                    TeacherUserID = TeacherUserID,
                    HireDate = DateTime.Now

                };
            }

            return TeacherData;
        }

        public async Task<ParentData> GetParentDataAsync(int ParentUserID)
        {
            var ParentData = new ParentData();



            try
            {
                ParentData = await _context.Parents
                    .Where(t => t.UserId == ParentUserID)
                    .Select(t => new ParentData
                    {
                        ParentUserID = ParentUserID,
                        ParentID = t.ID

                    }).FirstOrDefaultAsync();




            }
            catch (Exception ex)
            {
            }

            if (ParentData == null)
            {
                ParentData = new ParentData
                {
                    ParentUserID = ParentUserID,
                };
            }

            return ParentData;
        }

        public async Task<SupervisorData> GetSupervisorDataAsync(int SupervisorUserID)
        {
            var SupervisorData = new SupervisorData();



            try
            {
                SupervisorData = await _context.Supervisors
                    .Where(t => t.UserId == SupervisorUserID)
                    .Select(t => new SupervisorData
                    {
                        UserID = SupervisorUserID,
                        ID = t.ID,
                        HireDate=t.HireDate,
                        ExitDate=t.ExiteDate

                    }).FirstOrDefaultAsync();




            }
            catch (Exception ex)
            {
            }

            if (SupervisorData == null)
            {
                SupervisorData = new SupervisorData
                {
                    UserID = SupervisorUserID
                };
            }

            return SupervisorData;
        }

        public async Task<HeadmasterData> GetHeadmasterDataAsync(int HeadmasterUserID)
        {
            var HeadmasterData = new HeadmasterData();



            try
            {
                HeadmasterData = await _context.Headmasters
                    .Where(t => t.UserId == HeadmasterUserID)
                    .Select(t => new HeadmasterData
                    {
                        UserID = HeadmasterUserID,
                        ID = t.ID,
                        HireDate = t.HireDate,
                        ExitDate = t.ExiteDate

                    }).FirstOrDefaultAsync();




            }
            catch (Exception ex)
            {
            }

            if (HeadmasterData == null)
            {
                HeadmasterData = new HeadmasterData
                {
                    UserID = HeadmasterUserID
                };
            }

            return HeadmasterData;
        }

        public async Task<ITManagerData> GetITManagerDataAsync(int ITManagerUserID)
        {
            var ITManagerData = new ITManagerData();



            try
            {
                ITManagerData = await _context.Admins
                    .Where(t => t.UserId == ITManagerUserID)
                    .Select(t => new ITManagerData
                    {
                        UserID = ITManagerUserID,
                        ID = t.ID,
                        HireDate = t.HireDate,
                        ExitDate = t.ExiteDate

                    }).FirstOrDefaultAsync();




            }
            catch (Exception ex)
            {
            }

            if (ITManagerData == null)
            {
                ITManagerData = new ITManagerData
                {
                    UserID = ITManagerUserID
                };
            }

            return ITManagerData;
        }

        private async Task<int> _GetLastTermIDAsync()
        {
            int lastTerm = 0;
            try
            {
                 lastTerm = await _context.AcademicTerms
                   .OrderByDescending(t => t.StartDate)
                   .Select(t => t.ID)
                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return lastTerm;
        }
        
        public async Task<StudentEnrollmentInfo> GetStudentEntrollmentinfoAsync(int StudentID)
        {
            var StudentEnrollementInfo = new StudentEnrollmentInfo();

            var lastTermID= await _GetLastTermIDAsync();

            try
            {
                StudentEnrollementInfo = await _context.StudentClassEnrollments
                    .Where(sce => sce.StudentId == StudentID&& sce.AcademicTermId==lastTermID)
                    .Select(sce => new StudentEnrollmentInfo
                    {
                        ClassID = sce.ClassId,
                        LevelID = sce.LevelID

                    })
                    .FirstOrDefaultAsync();

                if (StudentEnrollementInfo != null)
                {

                    StudentEnrollementInfo.StageID = await _GetSatgeIDOfSpecificLevelID(StudentEnrollementInfo.LevelID);
                }

            }
            catch(Exception ex)
            {

            }



            return StudentEnrollementInfo;
        }
        public async Task<SomeSchoolInfo> GetITManagerSomeSchoolInfoAsync()
        {
            

            var StudentsNumber = await _context.Students
                .Where(s => s.IsGraduated == false || s.ExiteDate == null)
                .CountAsync();


            var TeachersNumber = await _context.Teachers
                .Where(t=> t.ExiteDate==null)
                .CountAsync();

            var ClassesNumber = await _context.Classes
                .CountAsync();

            var SubjectsNumber = await _context.Subjects
                .CountAsync();

           



            return new SomeSchoolInfo
            {
                ClassesNum  = ClassesNumber,
                StudentsNum =StudentsNumber,
                TeachersNum =TeachersNumber,
                SubjectsNum =SubjectsNumber
            };
           
        }

        private async Task<List<int>> _GetStagesIDsInSchoolAsync()
        {
            List<int> IDs=new List<int>();

            try
            {
                IDs = await _context.Stages.
                    Select(s => s.ID)
                    .ToListAsync();

            }catch(Exception ex)
            {

            }


            return IDs;
        }

        private async Task<List<int>> _GetLevelsIDsIForSpecificStageAsync(int StageID)
        {
            List<int> IDs = new List<int>();

            try
            {
                IDs = await _context.Levels
                    .Where(l=> l.StageID==StageID)
                    .Select(s => s.ID)
                    .ToListAsync();

            }
            catch (Exception ex)
            {

            }


            return IDs;
        }

        private async Task<List<Class>> _GetClassesForSpecificLevelAsync(int LevelID)
        {
            List<Class> Classes = new List<Class>();

            try
            {
                Classes = await _context.Classes
                    .Where(c=> c.LevelID==LevelID)
                    .ToListAsync();

            }
            catch (Exception ex)
            {

            }


            return Classes;
        }

        private async Task<Level> _GetLevelAsync(int LevelID)
        {
            var Level= new Level();

            try
            {

                Level = await _context.Levels
                    .Where(l => l.ID == LevelID)
                    .FirstOrDefaultAsync();

            }catch(Exception ex)
            {

            }

            return Level;
        }

        private async Task<Stage> _GetStageAsync(int StageID)
        {

            var Stage= new Stage();

            try
            {

                Stage =await  _context.Stages
                    .Where(s => s.ID == StageID)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

            }

            return Stage;


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
        // ================= SUBJECT CRUD =================

        public async Task<List<SubjectViewModel>> GetAllSubjectsAsync()
        {
            return await _context.Subjects
                .Select(s => new SubjectViewModel
                {
                    Id = s.ID,
                    Name = s.Name,
                    Code = s.Code,
                    WeeklyHours = s.WeeklyHours
                }).ToListAsync();
        }

        public async Task<SubjectViewModel?> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            return new SubjectViewModel
            {
                Id = subject.ID,
                Name = subject.Name,
                Code = subject.Code,
                WeeklyHours = subject.WeeklyHours
            };
        }

        public async Task AddSubjectAsync(SubjectViewModel model)
        {
            var subject = new Subject
            {
                Name = model.Name,
                Code = model.Code,
                WeeklyHours = model.WeeklyHours
            };

            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(SubjectViewModel model)
        {
            var subject = await _context.Subjects.FindAsync(model.Id);
            if (subject != null)
            {
                subject.Name = model.Name;
                subject.Code = model.Code;
                subject.WeeklyHours = model.WeeklyHours;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        // ================= TEACHER SUBJECT BINDING CRUD =================

        public async Task<List<TeacherSubjectViewModel>> GetAllTeacherSubjectsAsync()
        {
            return await _context.TeacherSubjects
                .Include(ts => ts.Teacher).ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                .Include(ts => ts.Class).ThenInclude(c => c.Level).ThenInclude(l => l.Stage)
                .Select(ts => new TeacherSubjectViewModel
                {
                    Id = ts.ID,
                    TeacherId = ts.TeacherId,
                    TeacherName = ts.Teacher.User.FullName,
                    SubjectId = ts.SubjectId,
                    SubjectName = ts.Subject.Name,
                    SubjectCode = ts.Subject.Code,
                    ClassId = ts.ClassId,
                    ClassName = ts.Class != null ? ts.Class.Name : null,
                    LevelName = ts.Class != null ? ts.Class.Level.Name : null,
                    StageName = ts.Class != null ? ts.Class.Level.Stage.Name : null
                }).ToListAsync();
        }

        public async Task<TeacherSubjectViewModel?> GetTeacherSubjectByIdAsync(int id)
        {
            return await _context.TeacherSubjects
                .Where(ts => ts.ID == id)
                .Select(ts => new TeacherSubjectViewModel
                {
                    Id = ts.ID,
                    TeacherId = ts.TeacherId,
                    SubjectId = ts.SubjectId,
                    ClassId = ts.ClassId
                }).FirstOrDefaultAsync();
        }

        public async Task AddTeacherSubjectAsync(TeacherSubjectViewModel model)
        {
            var ts = new TeacherSubject
            {
                TeacherId = model.TeacherId,
                SubjectId = model.SubjectId,
                ClassId = model.ClassId
            };

            await _context.TeacherSubjects.AddAsync(ts);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeacherSubjectAsync(TeacherSubjectViewModel model)
        {
            var ts = await _context.TeacherSubjects.FindAsync(model.Id);
            if (ts != null)
            {
                ts.TeacherId = model.TeacherId;
                ts.SubjectId = model.SubjectId;
                ts.ClassId = model.ClassId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTeacherSubjectAsync(int id)
        {
            var ts = await _context.TeacherSubjects.FindAsync(id);
            if (ts != null)
            {
                _context.TeacherSubjects.Remove(ts);
                await _context.SaveChangesAsync();
            }
        }

        // Dropdown helpers
        public async Task<List<object>> GetTeachersSelectListAsync()
        {
            var teachers = await _context.Teachers.Include(t => t.User).ToListAsync();
            return teachers.Select(t => new { Id = t.ID, Name = t.User.FullName } as object).ToList();
        }

        public async Task<List<object>> GetSubjectsSelectListAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return subjects.Select(s => new { Id = s.ID, Name = s.Name } as object).ToList();
        }

        public async Task<List<object>> GetClassesSelectListAsync()
        {
            var classes = await _context.Classes.Include(c => c.Level).ThenInclude(l => l.Stage).ToListAsync();
            return classes.Select(c => new { 
                Id = c.ID, 
                Name = $"{c.Level.Stage.Name} - الصف {c.Level.Name} - فصل {c.Name}" 
            } as object).ToList();
        }        // ================= CLASSES MANAGEMENT =================

        public async Task<List<ClassViewModel>> GetAllClassesListAsync()
        {
            return await _context.Classes
                .Include(c => c.Level)
                .ThenInclude(l => l.Stage)
                .Select(c => new ClassViewModel
                {
                    ID = c.ID,
                    ClassName = c.Name,
                    LevelName = c.Level != null ? c.Level.Name : "",
                    StageName = c.Level != null && c.Level.Stage != null ? c.Level.Stage.Name : ""
                }).ToListAsync();
        }

        public async Task<AddEditClassViewModel> GetClassForEditAsync(int id)
        {
            var cls = await _context.Classes
                .Include(c => c.Level)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (cls == null) return null;

            return new AddEditClassViewModel
            {
                ID = cls.ID,
                ClassName = cls.Name,
                LevelID = cls.LevelID,
                StageID = cls.Level != null ? cls.Level.StageID : 0
            };
        }

        public async Task<bool> IsClassExistsAsync(int levelId, string className, int? excludeClassId = null)
        {
            var query = _context.Classes.Where(c => c.LevelID == levelId && c.Name.ToLower() == className.ToLower());
            if (excludeClassId.HasValue)
            {
                query = query.Where(c => c.ID != excludeClassId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task<bool> SaveNewClassAsync(AddEditClassViewModel model)
        {
            try
            {
                var newClass = new Class
                {
                    Name = model.ClassName,
                    LevelID = model.LevelID
                };
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateClassAsync(AddEditClassViewModel model)
        {
            try
            {
                var cls = await _context.Classes.FindAsync(model.ID);
                if (cls == null) return false;

                cls.Name = model.ClassName;
                cls.LevelID = model.LevelID;

                _context.Classes.Update(cls);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            try
            {
                var cls = await _context.Classes.FindAsync(id);
                if (cls == null) return false;

                _context.Classes.Remove(cls);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ================= STUDENT SUBJECT ENROLLMENT =================

        public async Task<StudentEnrollmentDto?> GetStudentEnrollmentDataAsync(string searchType, string searchValue)
        {
            var query = _context.Students
                .Include(s => s.User)
                .Include(s => s.StudentClassEnrollments).ThenInclude(sce => sce.Class).ThenInclude(c => c.Level).ThenInclude(l => l.Stage)
                .Include(s => s.StudentClassEnrollments).ThenInclude(sce => sce.AcademicTerm)
                .Include(s => s.StudentsSubjectsEnrollments).ThenInclude(sse => sse.Subject)
                .AsQueryable();

            if (searchType == "phone")
            {
                query = query.Where(s => s.User.Phone == searchValue);
            }
            else if (searchType == "nationalid")
            {
                query = query.Where(s => s.User.SSN == searchValue);
            }
            else
            {
                return null;
            }

            var student = await query.FirstOrDefaultAsync();
            if (student == null) return null;

            var activeEnrollment = student.StudentClassEnrollments.OrderByDescending(e => e.ID).FirstOrDefault();
            if (activeEnrollment == null) return null; // Must be enrolled in a class first

            var dto = new StudentEnrollmentDto
            {
                StudentId = student.ID,
                Name = student.User.FullName,
                Stage = activeEnrollment.Class?.Level?.Stage?.Name ?? "",
                Level = activeEnrollment.Class?.Level?.Name ?? "",
                Class = activeEnrollment.Class?.Name ?? "",
                Term = activeEnrollment.AcademicTerm?.Name ?? ""
            };

            // Get subjects available for this class
            var classSubjects = await _context.TeacherSubjects
                .Include(ts => ts.Subject)
                .Where(ts => ts.ClassId == activeEnrollment.ClassId)
                .Select(ts => ts.Subject)
                .Distinct()
                .ToListAsync();

            var enrolledSubjectIds = student.StudentsSubjectsEnrollments
                .Where(sse => sse.AcademicTermId == activeEnrollment.AcademicTermId)
                .Select(sse => sse.SubjectId)
                .ToList();

            foreach (var subject in classSubjects)
            {
                if (subject != null)
                {
                    dto.AvailableSubjects.Add(new SubjectCheckboxDto
                    {
                        SubjectId = subject.ID,
                        SubjectName = subject.Name,
                        IsEnrolled = enrolledSubjectIds.Contains(subject.ID)
                    });
                }
            }

            return dto;
        }

        public async Task SaveStudentSubjectsAsync(int studentId, List<int> subjectIds)
        {
            var student = await _context.Students
                .Include(s => s.StudentClassEnrollments)
                .Include(s => s.StudentsSubjectsEnrollments)
                .FirstOrDefaultAsync(s => s.ID == studentId);

            if (student == null) return;

            var activeEnrollment = student.StudentClassEnrollments.OrderByDescending(e => e.ID).FirstOrDefault();
            if (activeEnrollment == null) return;

            var currentTermId = activeEnrollment.AcademicTermId;

            // Existing enrollments for the current term
            var existingEnrollments = student.StudentsSubjectsEnrollments
                .Where(sse => sse.AcademicTermId == currentTermId)
                .ToList();

            // Remove unselected ones
            var toRemove = existingEnrollments.Where(e => !subjectIds.Contains(e.SubjectId)).ToList();
            _context.StudentsSubjectsEnrollments.RemoveRange(toRemove);

            // Add new ones
            var existingSubjectIds = existingEnrollments.Select(e => e.SubjectId).ToList();
            var newSubjectIds = subjectIds.Where(id => !existingSubjectIds.Contains(id)).ToList();

            if (newSubjectIds.Any())
            {
                // Find teacher bindings for this class
                var classBindings = await _context.TeacherSubjects
                    .Where(ts => ts.ClassId == activeEnrollment.ClassId && newSubjectIds.Contains(ts.SubjectId))
                    .ToListAsync();

                foreach (var subId in newSubjectIds)
                {
                    var binding = classBindings.FirstOrDefault(ts => ts.SubjectId == subId);
                    if (binding != null)
                    {
                        var newEnrollment = new StudentsSubjectsEnrollment
                        {
                            StudentId = studentId,
                            SubjectId = subId,
                            TeacherId = binding.TeacherId,
                            AcademicTermId = currentTermId,
                            EnrolledDate = DateTime.Now,
                            IsPassed = false
                        };
                        _context.StudentsSubjectsEnrollments.Add(newEnrollment);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        // ================= STUDENT CLASS ENROLLMENT CRUD =================

        public async Task<List<StudentClassEnrollmentListItem>> GetAllStudentClassEnrollmentsAsync()
        {
            return await _context.StudentClassEnrollments
                .Include(e => e.Student).ThenInclude(s => s.User)
                .Include(e => e.Class)
                .Include(e => e.Level).ThenInclude(l => l.Stage)
                .Include(e => e.AcademicTerm).ThenInclude(t => t.AcademicYear)
                .OrderByDescending(e => e.ID)
                .Select(e => new StudentClassEnrollmentListItem
                {
                    EnrollmentID = e.ID,
                    StudentId = e.StudentId,
                    StudentName = e.Student.User.FirstName + " " +
                                  (e.Student.User.SecondName ?? "") + " " +
                                  (e.Student.User.ThirdName ?? "") + " " +
                                  e.Student.User.LastName,
                    SSN = e.Student.User.SSN,
                    Phone = e.Student.User.Phone,
                    StageName = e.Level != null && e.Level.Stage != null ? e.Level.Stage.Name : "",
                    LevelName = e.Level != null ? e.Level.Name : "",
                    ClassName = e.Class != null ? e.Class.Name : "",
                    AcademicTermName = e.AcademicTerm != null
                        ? (e.AcademicTerm.AcademicYear != null
                            ? e.AcademicTerm.AcademicYear.Name + " - " + e.AcademicTerm.Name
                            : e.AcademicTerm.Name)
                        : "",
                    IsPassed = e.IsPassed
                }).ToListAsync();
        }

        public async Task<AddEditStudentClassEnrollmentViewModel?> GetStudentClassEnrollmentByIdAsync(int id)
        {
            var enrollment = await _context.StudentClassEnrollments
                .Include(e => e.Class).ThenInclude(c => c.Level)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (enrollment == null) return null;

            return new AddEditStudentClassEnrollmentViewModel
            {
                ID = enrollment.ID,
                StudentId = enrollment.StudentId,
                ClassId = enrollment.ClassId,
                LevelID = enrollment.LevelID,
                AcademicTermId = enrollment.AcademicTermId,
                IsPassed = enrollment.IsPassed,
                StageID = enrollment.Class?.Level?.StageID ?? 0
            };
        }

        public async Task<List<StudentSelectItem>> GetStudentsSelectListForEnrollmentAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Select(s => new StudentSelectItem
                {
                    Id = s.ID,
                    Name = s.User.FirstName + " " +
                           (s.User.SecondName ?? "") + " " +
                           (s.User.ThirdName ?? "") + " " +
                           s.User.LastName
                }).ToListAsync();
        }

        public async Task<List<AcademicTermSelectItem>> GetAcademicTermsSelectListAsync()
        {
            return await _context.AcademicTerms
                .Include(t => t.AcademicYear)
                .Select(t => new AcademicTermSelectItem
                {
                    Id = t.ID,
                    Name = t.AcademicYear != null
                        ? t.AcademicYear.Name + " - " + t.Name
                        : t.Name
                }).ToListAsync();
        }

        public async Task<bool> IsStudentAlreadyEnrolledInTermAsync(int studentId, int academicTermId, int? excludeId = null)
        {
            var query = _context.StudentClassEnrollments
                .Where(e => e.StudentId == studentId && e.AcademicTermId == academicTermId);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.ID != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> SaveStudentClassEnrollmentAsync(AddEditStudentClassEnrollmentViewModel model)
        {
            try
            {
                var enrollment = new StudentClassEnrollment
                {
                    StudentId = model.StudentId,
                    ClassId = model.ClassId,
                    LevelID = model.LevelID,
                    AcademicTermId = model.AcademicTermId,
                    IsPassed = model.IsPassed
                };

                _context.StudentClassEnrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStudentClassEnrollmentAsync(AddEditStudentClassEnrollmentViewModel model)
        {
            try
            {
                var enrollment = await _context.StudentClassEnrollments.FindAsync(model.ID);
                if (enrollment == null) return false;

                enrollment.StudentId = model.StudentId;
                enrollment.ClassId = model.ClassId;
                enrollment.LevelID = model.LevelID;
                enrollment.AcademicTermId = model.AcademicTermId;
                enrollment.IsPassed = model.IsPassed;

                _context.StudentClassEnrollments.Update(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentClassEnrollmentAsync(int id)
        {
            try
            {
                var enrollment = await _context.StudentClassEnrollments.FindAsync(id);
                if (enrollment == null) return false;

                _context.StudentClassEnrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal async Task<bool> UpdateUserPasswordAsync(string SSN, string NewPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.SSN == SSN);

                if (user == null) return false;

                user.Password = clsBCrypt.GetHash(NewPassword);

                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        internal async Task<List<TermData>> GetTermsDataAsync()
        {
            return await _context.AcademicTerms.Select(
                t => new TermData
                {
                    TermID = t.ID,
                    AcademicYearID = t.AcademicYearId,
                    TermName = t.Name,
                    YearName=t.AcademicYear.Name
                })
                .OrderByDescending(t=> t.TermID)
                .ToListAsync();
        }

        internal async Task<List<ClassStudentsData>> GetClassStudentsData(int classid, int levelid, int currentTermID)
        {
            return await _context.StudentClassEnrollments
                .Where(sce => sce.ClassId == classid && sce.LevelID == levelid && sce.AcademicTermId == currentTermID)
                .Select(sce => new ClassStudentsData
                {
                    IsSuccess = sce.IsPassed,
                    StudentId = sce.StudentId,
                    StudentName = sce.Student.User.FullName
                })
                .ToListAsync();
        }

        internal  bool ValidateRegisterClassSubjects(int classID, int levelID, int stageID, int termID)
        {
            bool isClassExist =  _context.Classes.Any(c => c.ID == classID);

            bool isLevelExist = _context.Levels.Any(c => c.ID == levelID);

            bool isStageExist = _context.Stages.Any(c => c.ID == stageID);

            bool isTermExist = _context.AcademicTerms.Any(c => c.ID == termID);




            return isClassExist && isLevelExist && isStageExist && isTermExist;
        }

        public async Task<List<SubjectsData>> GetSubjectsWithTeachersAsync()
        {
            var subjectsList = new List<SubjectsData>();

            try
            {
                subjectsList = await _context.Subjects
                    .Select(subject => new SubjectsData
                    {
                        SubjectID = subject.ID,
                        SubjectName = subject.Name,
                        isSelected = false, 
                        SubjectTeachers = subject.TeacherSubjects
                            .Select(ts => new SubjectTeacherData
                            {
                                TeacherID = ts.TeacherId,
                              
                                TeacherName = ts.Teacher.User.FullName
                            }).Distinct().ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                
            }

            return subjectsList;
        }
    }
}
