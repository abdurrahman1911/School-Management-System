using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.ITManager;
using SchoolManagementSystem.ViewModel.Owner;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Controllers
{
    public class ITManagerController : Controller
    {

        private readonly ITManagerService _ITManagerService;
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly ImageService _imageService;

        private readonly LogService _logService;


        public ITManagerController(ITManagerService ITManagerService, AppDbContext context, UserService userService, ImageService imageService, LogService logService)
        {
            _ITManagerService = ITManagerService;
            _context = context;
            _userService = userService;
            _imageService = imageService;
            _logService = logService;
        }
        #region By Deghish

         
       
        public async Task<IActionResult> AcademicYear()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = await _ITManagerService.GetAcademicYearAsync();
            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            return View(model);
        }


        public async Task< IActionResult> AcademicTerm()
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = await _ITManagerService.GetAcademicTermAsync();
            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            return View(model);
        }

        public IActionResult AddAcademicYear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAcademicYear(AddOrEditeAcademicYearViewModel model)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var added = await _ITManagerService.AddAcademicYearAsync(model);

            if (added)
            {
                string logAction = "إضافة";
                string logDetails = $"تم إضافة سنة دراسية جديدة باسم ({model.Name})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                return RedirectToAction("AcademicYear");
            }

            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حفظ البيانات في السيرفر، يرجى المحاولة مرة أخرى.");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditAcademicYear(int id)
        {
            var academicYear = await _context.AcademicYears.FindAsync(id);

            if (academicYear == null)
            {
                return NotFound(); 
            }

            var model = new AddOrEditeAcademicYearViewModel
            {
                Name = academicYear.Name,
                StartDate = academicYear.StartDate,
                EndDate = academicYear.EndDate
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAcademicYear(int id, AddOrEditeAcademicYearViewModel model)
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var updated = await _ITManagerService.UpdateAcademicYearAsync(id, model);

            if (updated)
            {
                string logAction = "تعديل";
                string logDetails = $"تم تعديل بيانات السنة الدراسية ({model.Name})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                return RedirectToAction("AcademicYear"); 
            }

            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث البيانات، يرجى المحاولة مرة أخرى.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAcademicYear(int id)
        {

            var year =await _context.AcademicYears.Where(ay => ay.ID == id).FirstOrDefaultAsync();

            var result = await _ITManagerService.DeleteAcademicYearAsync(id);

            if (result == "success")
            {
                var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


                string logAction = "حذف";
                string logDetails = $"تم حذف السنة الدراسية ({year?.Name})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                return Json(new { success = true, message = "تم حذف السنة الدراسية بنجاح." });
            }

            return Json(new { success = false, message = result });
        }
        public async Task<IActionResult> DeleteAcademicTerm(int id)
        {
            var model = await _context.AcademicTerms.Where(at => at.ID == id).FirstOrDefaultAsync();
            var result = await _ITManagerService.DeleteAcademicTermAsync(id);

            if (result == "success")
            {
                var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));



                string logAction = "حذف";
                string logDetails = $"تم حذف الفصل الدراسي ({model?.Name})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                return Json(new { success = true, message = "تم حذف الفصل الدراسي بنجاح." });
            }

            return Json(new { success = false, message = result });
        }


        public async Task< IActionResult> AddAcademicTerm()
        {
            var model = new AddOrUpdateAcademicTermViewModel();
            model.AcademicYears = await _ITManagerService.GetAcdemiceYearForDropDownAsync();
            return View(model); 
        }
        [HttpPost]
        public async Task< IActionResult> AddAcademicTerm(AddOrUpdateAcademicTermViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AcademicYears = await _ITManagerService.GetAcdemiceYearForDropDownAsync();
                return View(model);
            }

            var added = await _ITManagerService.AddAcademicTermAsync(model);

            if (added)
            {
                var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


                var termName = model.TermNumber == 1 ? "الفصل الدراسي الأول" :
                    model.TermNumber == 2 ? "الفصل الدراسي الثاني" : "الفصل الدراسي الصيفي";
                string logAction = "إضافة";
                string logDetails = $"تم إضافة فصل دراسي جديد: ({termName})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                return RedirectToAction("AcademicTerm");
            }

            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حفظ البيانات في السيرفر، يرجى المحاولة مرة أخرى.");
                model.AcademicYears = await _ITManagerService.GetAcdemiceYearForDropDownAsync();

            return View(model);

        }


        [HttpGet]
        public async Task<IActionResult> EditAcademicTerm(int id)
        {
            var model = await _ITManagerService.GetAcademicTermByIdAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditAcademicTerm(int id, AddOrUpdateAcademicTermViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AcademicYears = await _ITManagerService.GetAcdemiceYearForDropDownAsync();
                return View(model);
            }
            var updated = await _ITManagerService.UpdateAcademicTermAsync(id, model);
            if (updated)
            {
                var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var termName = model.TermNumber == 1 ? "الفصل الدراسي الأول" :
model.TermNumber == 2 ? "الفصل الدراسي الثاني" : "الفصل الدراسي الصيفي";

                string logAction = "تعديل";
                string logDetails = $"تم تعديل بيانات ({termName})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                return RedirectToAction("AcademicTerm");
            }
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث البيانات، يرجى المحاولة مرة أخرى.");
            model.AcademicYears = await _ITManagerService.GetAcdemiceYearForDropDownAsync();

            return View(model);
        }

        public async Task<IActionResult> ManageSuccess()
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new TargetEnrollementData();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();

            model.termsData = await _ITManagerService.GetTermsDataAsync();

            return View(model);
        }

        public async Task<IActionResult> ManageRegisterClassSubjects()
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new TargetEnrollementData();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();

            model.termsData = await _ITManagerService.GetTermsDataAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ManageRegisterClassSubjects(int classID,int LevelID,int StageID,int TermID)
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            //code

            bool isValid =  _ITManagerService.ValidateRegisterClassSubjects( classID, LevelID, StageID, TermID);

            if (!isValid)
            {
               
                var model = new TargetEnrollementData();

                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                model.Stages = await _ITManagerService.GetStagesInSchoolAsync();

                model.termsData = await _ITManagerService.GetTermsDataAsync();

                return View(model);
            }

         

           


            return RedirectToAction("SelectRegisteredClassSubjects","ITManager", new { classID = classID, LevelID=LevelID, StageID =StageID, TermID=TermID });

        }

         public async Task<IActionResult> SelectRegisteredClassSubjects(int classID, int LevelID, int StageID, int TermID)
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new RegisterSubjectsForClassViewModel()
            {
                ClassID = classID,
                LevelID = LevelID,
                StageID = StageID,
                TargetTermID = TermID,
                NavigationInfo = _userService.GetNavigationData(LoginUserID),
                Subjects = await _ITManagerService.GetSubjectsWithTeachersAsync()
            };
            

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> SelectRegisteredClassSubjects(RegisterSubjectsForClassViewModel model)
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            //code
            var isRegistered = await _ITManagerService.RegisterSubjectsForClassAsync(model.ClassID, model.TargetTermID, model.Subjects);
            if (isRegistered) {
                TempData["SuccessMessage"] = "تم تسجيل المواد بنجاح";
                return RedirectToAction("ManageRegisterClassSubjects", "ITManager");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تسجيل المواد، يرجى المحاولة مرة أخرى.");
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
                model.Subjects = await _ITManagerService.GetSubjectsWithTeachersAsync();
                return View(model);
            }


        }

        [HttpPost]
        public async Task<IActionResult> ManageSuccess(int stageid,int levelid ,int classid,int currentTermid)
        {

            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));



            bool isValid = await _ITManagerService.isManageSuccessClassDataRight(stageid, levelid, classid, currentTermid);

            if (!isValid)
            {
                var model = new TargetEnrollementData();

                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                model.Stages = await _ITManagerService.GetStagesInSchoolAsync();

                model.termsData = await _ITManagerService.GetTermsDataAsync();

                ModelState.AddModelError(string.Empty, "لا يوجد اي طالب في هذا الفصل مسجل ف هذا الترم");

                return View(model);
            }




            return RedirectToAction("ManageClassSuccess", "ITManager", new { stageID=stageid, levelID= levelid, classID= classid, currentTermID=currentTermid});
        }

        public async Task<IActionResult> ManageClassSuccess(int stageID, int levelID, int classID, int currentTermID)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new ManageSuccessViewModel
            {
                SelectedStageId = stageID,
                SelectedLevelId = levelID,
                SelectedClassId = classID,
                CurrentAcademicTermId = currentTermID
            };

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.termsData = await _ITManagerService.GetTermsDataAsync();

            model.classStudentsData = await _ITManagerService.GetClassStudentsData(classID, levelID, currentTermID);
            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> ManageClassSuccess(ManageSuccessViewModel model)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

           

           var isUpdated = await _ITManagerService
                .MangeSuccessAsync(model.SelectedClassId,model.CurrentAcademicTermId,model.NextAcademicTermId,model.classStudentsData);
            if (isUpdated)
            {


                
                var SelectedClass = await _context.Classes.Where(c => c.ID == model.SelectedClassId).FirstOrDefaultAsync();
                var selectLevel = await _context.Levels.Where(l => l.ID == model.SelectedLevelId).FirstOrDefaultAsync();
                var selectedTerm = await _context.AcademicTerms.Where(at => at.ID == model.NextAcademicTermId).FirstOrDefaultAsync();

               
                string logAction = "تسجيل نجاح";
                string logDetails = $"تم تسجيل نجاح الطلاب في المرحلة ({selectLevel?.Name}) فصل ({SelectedClass?.Name}) ونقلهم إلى الترم الدراسي ({selectedTerm?.Name})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                return RedirectToAction("ManageSuccess", "ITManager");
            }
            return View(model);

        }


        


        #endregion



        public async Task<IActionResult> UsersChangePassword()
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = new UsersChangePasswordViewModel();
            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UsersChangePassword(UsersChangePasswordViewModel model)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
           
            if (!ModelState.IsValid)
            {
                model.NavigationInfo=_userService.GetNavigationData(LoginUserID);

                return View(model);
            }
            
            if (!_ITManagerService.isUserRequireDataRight(model.RequireInfo))
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
                ModelState.AddModelError(string.Empty, "لا يوجد مستخدم بالمعلومات التي ادخلتها");
                return View(model);
            }


            return RedirectToAction("UserChangePassword", "ITManager", new { SSN = model.RequireInfo.SSN });


        }

        public async Task<IActionResult> UserChangePassword(string SSN)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new UserChangePasswordViewModel();

            

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            model.UserData = await _userService.GetUserDataForChangePasswordAsync(SSN);

            

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserChangePassword(UserChangePasswordViewModel model)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {

                model.NavigationInfo=_userService.GetNavigationData(LoginUserID);
                model.UserData = await _userService.GetUserDataForChangePasswordAsync(model.UserData.SSN);
                return View(model);

            } 

            bool isUpdated=await _ITManagerService.UpdateUserPasswordAsync(model.UserData.SSN, model.Password);

            if (isUpdated)
            {
                string logAction = "تغيير كلمة مرور";
                string logDetails = $"تم تغيير كلمة المرور للمستخدم صاحب الرقم القومي ({model.UserData.SSN})";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ البيانات بنجاح";
                return RedirectToAction("UsersChangePassword", "ITManager");

            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";

                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
                model.UserData = await _userService.GetUserDataForChangePasswordAsync(model.UserData.SSN);
                return View(model);
            }

           
        }

        public IActionResult EditProfile()
        {
            var model = new ITManagerEditProfileViewModel();
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ITManagerEditProfileViewModel model)
        {
            var LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {

                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
                ModelState.AddModelError("", "الباسورد لازم يكون 8 حروف على الأقل");
                return View(model);
            }

            var user = _context.Users.Find(LoginUserID);
            bool check =await _userService.Setting(
                user,
                model.ChangePasswordInfo.CurrentPassword,
                model.ChangePasswordInfo.NewPassword
                );

            if (!check)
            {
                ModelState.AddModelError("", "كلمة المرور الحالية غير صحيحة");
                return View(model);
            }
            else
            {
                string logAction = "تعديل الملف الشخصي";
                string logDetails = "قام المستخدم بتغيير كلمة المرور الخاصة به";

                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                return RedirectToAction("Home", "ITManager");
            }
        }

        public async Task<IActionResult> Home()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = new ITManagerHomeViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.LoginITManagerInfo= await _ITManagerService.GetLoginITManagerHomeInfoAsync(LoginUserID);

            model.someSchoolInfo = await _ITManagerService.GetITManagerSomeSchoolInfoAsync();




            return View(model);
        }

        public IActionResult AddNewUserRole()
        {

            var model = new ITManagerNewUserRoleViewModel();

            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUserRole(ITManagerNewUserRoleViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (model.NewUserSSN==null && model.userType == 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }
            var routeValues = new { ssn = model.NewUserSSN };
            switch (model.userType)
            {
                case UserTypeEnum.Student:
                   if(!_ITManagerService.isStudentExist(model.NewUserSSN))
                    {
                        return RedirectToAction("AddNewStudent", "ITManager", routeValues);
                    }
                    else
                    {
                        return RedirectToAction("EditStudent", "ITManager", routeValues);
                    }

                case UserTypeEnum.Teacher:
                    if (!_ITManagerService.isTeacherExist(model.NewUserSSN))
                    {
                        return RedirectToAction("AddNewTeacher", "ITManager", routeValues);
                    }
                    else
                    {
                        return RedirectToAction("EditTeacher", "ITManager", routeValues);
                    }
                    
                case UserTypeEnum.Supervisor:
                    if (!_ITManagerService.isSupervisorExist(model.NewUserSSN))
                    {
                        return RedirectToAction("AddNewSupervisor", "ITManager", routeValues);
                    }
                    else
                    {
                        return RedirectToAction("EditSupervisor", "ITManager", routeValues);
                    }
                    
                case UserTypeEnum.Headmaster:
                    if (!_ITManagerService.isHeadmasterExist(model.NewUserSSN))
                    {
                        return RedirectToAction("AddNewHeadmaster", "ITManager", routeValues);
                    }
                    else
                    {
                        return RedirectToAction("EditHeadmaster", "ITManager", routeValues);
                    }
                  
                case UserTypeEnum.Parent:
                    if (!_ITManagerService.isParentExist(model.NewUserSSN))
                    {
                        return RedirectToAction("AddNewParent", "ITManager", routeValues);
                    }
                    else
                    {
                        return RedirectToAction("EditParent", "ITManager", routeValues);
                    }
                  
                case UserTypeEnum.IT:
                    if (!_ITManagerService.isITManagerExist(model.NewUserSSN))
                    {
                        return RedirectToAction("AddNewITManager", "ITManager", routeValues);
                    }
                    else
                    {
                        return RedirectToAction("EditITManager", "ITManager", routeValues);
                    }


                default:
                    return RedirectToAction("Home", "ITManager");
            }

            
        }

        [HttpPost]
        public async Task<IActionResult> AddNewStudent(AddEditStudentViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);
            var studentErrors = _ITManagerService.ValidateStudentData(model.StudentDataInfo);
            var studentEnrollementErrors = _ITManagerService.ValidateStudentEnrollmentInfoData(model.StudentEnrollmentInfo);

            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }

            foreach (var error in studentErrors)
            {
                ModelState.AddModelError("", error);
            }


            if (userErrors.Count!=0||studentErrors.Count!=0||studentEnrollementErrors.Count!=0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                model.StudentEnrollmentInfo =
                    await _ITManagerService.GetStudentEntrollmentinfoAsync(
                        model.StudentDataInfo.StudentID);

                model.Stages =
                    await _ITManagerService.GetStagesInSchoolAsync();

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess=await _ITManagerService.SaveUpdatStudent(model.UserInfo, model.StudentDataInfo, model.StudentEnrollmentInfo, LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
         {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string studentFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "إضافة طالب";
                string logDetails = $"تم إضافة طالب جديد باسم ({studentFullName}) ورقم قومي ({model.UserInfo.SSN})";

               
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                TempData["SuccessMessage"] = "تم حفظ بيانات الطالب بنجاح";
                return RedirectToAction("AddSubjectforStudent", "ITManager", new { preFillSSN = model.UserInfo.SSN });
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("StudentManagement", "ITManager");
            }
        }

        public async Task<IActionResult> AddNewStudent(string? SSN,string? ParentSSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditStudentViewModel();

            if(SSN!=null)
                model.SSN= SSN;

            if (ParentSSN != null)
                model.ParentSSN = ParentSSN;

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isStudentExist(SSN))
                {
                    model.StudentDataInfo = await _ITManagerService.GetStudentDataAsync(model.UserInfo.UserID);

                    model.StudentEnrollmentInfo = await _ITManagerService.GetStudentEntrollmentinfoAsync(model.StudentDataInfo.StudentID);

                }
            }

            

            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();




            return View(model);
        }

        public async Task<IActionResult> EditStudent(string SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditStudentViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);



            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isStudentExist(SSN))
                {
                    model.StudentDataInfo = await _ITManagerService.GetStudentDataAsync(model.UserInfo.UserID);

                    model.StudentEnrollmentInfo = await _ITManagerService.GetStudentEntrollmentinfoAsync(model.StudentDataInfo.StudentID);

                }
            }



            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();




            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditStudent(AddEditStudentViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);
            var studentErrors = _ITManagerService.ValidateStudentData(model.StudentDataInfo);
            var studentEnrollementErrors = _ITManagerService.ValidateStudentEnrollmentInfoData(model.StudentEnrollmentInfo);

            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }

            foreach (var error in studentErrors)
            {
                ModelState.AddModelError("", error);
            }


            if (userErrors.Count != 0 || studentErrors.Count != 0 || studentEnrollementErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                model.StudentEnrollmentInfo =
                    await _ITManagerService.GetStudentEntrollmentinfoAsync(
                        model.StudentDataInfo.StudentID);

                model.Stages =
                    await _ITManagerService.GetStagesInSchoolAsync();

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatStudent(model.UserInfo, model.StudentDataInfo, model.StudentEnrollmentInfo, LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string studentFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "تعديل بيانات طالب";
                string logDetails = $"تم تعديل بيانات الطالب ({studentFullName}) صاحب الرقم القومي ({model.UserInfo.SSN})";

              
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات الطالب بنجاح";
                return RedirectToAction("StudentManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("StudentManagement", "ITManager");
            }
        }
        public async Task<IActionResult> AddNewTeacher(string? SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditTeacherViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            if (SSN != null)
                model.SSN = SSN;

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isTeacherExist(SSN))
                {
                    model.TeacherInfo = await _ITManagerService.GetTeacherDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddNewTeacher(AddEditTeacherViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);
          
          
            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }

          


            if (userErrors.Count != 0 )
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatTeacher(model.UserInfo, model.TeacherInfo, LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string teacherFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "إضافة معلم";
                string logDetails = $"تم إضافة معلم جديد باسم ({teacherFullName}) ورقم قومي ({model.UserInfo.SSN})";

                
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات المعلم بنجاح";
                return RedirectToAction("AddBinding", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("TeacherManagement", "ITManager");
            }
        }

        public async Task<IActionResult> EditTeacher(string SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditTeacherViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isTeacherExist(SSN))
                {
                    model.TeacherInfo = await _ITManagerService.GetTeacherDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(AddEditTeacherViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatTeacher(model.UserInfo, model.TeacherInfo, LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string teacherFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

               
                string logAction = "تعديل بيانات معلم";
                string logDetails = $"تم تعديل بيانات المعلم ({teacherFullName}) صاحب الرقم القومي ({model.UserInfo.SSN})";

             
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                TempData["SuccessMessage"] = "تم تعديل بيانات المعلم بنجاح";
                return RedirectToAction("TeacherManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("TeacherManagement", "ITManager");
            }
        }


        public async Task<IActionResult> AddNewParent(string? SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditParentViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if(SSN!=null)
                model.SSN = SSN;


            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isParentExist(SSN))
                {
                    model.ParentInfo = await _ITManagerService.GetParentDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddNewParent(AddEditParentViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatParent(model.UserInfo, model.ParentInfo, LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string parentFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "إضافة ولي أمر";
                string logDetails = $"تم إضافة ولي أمر جديد باسم ({parentFullName}) ورقم قومي ({model.UserInfo.SSN})";

                
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات ولي الامر بنجاح";
                return RedirectToAction("AddNewStudent", "ITManager", new {ParentSSN=model.UserInfo.SSN});
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("ParentManagement", "ITManager");
            }

        }

        public async Task<IActionResult> EditParent(string SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditParentViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isParentExist(SSN))
                {
                    model.ParentInfo = await _ITManagerService.GetParentDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditParent(AddEditParentViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatParent(model.UserInfo, model.ParentInfo, LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string parentFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

               
                string logAction = "تعديل بيانات ولي أمر";
                string logDetails = $"تم تعديل بيانات ولي الأمر ({parentFullName}) صاحب الرقم القومي ({model.UserInfo.SSN})";

                
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                TempData["SuccessMessage"] = "تم تعديل بيانات ولي الامر بنجاح";
                return RedirectToAction("ParentManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("ParentManagement", "ITManager");
            }
        }
        public async Task<IActionResult> AddNewSupervisor(string? SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditSupervisorViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (SSN != null)
                model.SSN = SSN;

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isSupervisorExist(SSN))
                {
                    model.SupervisorInfo = await _ITManagerService.GetSupervisorDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddNewSupervisor(AddEditSupervisorViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatSupervisor(model.UserInfo, model.SupervisorInfo
                , LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
            {
                model.UserInfo.FirstName,
                model.UserInfo.SecondName,
                model.UserInfo.ThirdName,
                model.UserInfo.LastName
            };

                string supervisorFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "إضافة مشرف";
                string logDetails = $"تم إضافة مشرف جديد باسم ({supervisorFullName}) ورقم قومي ({model.UserInfo.SSN})";

              
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات المشرف بنجاح";
                return RedirectToAction("SupervisorsManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("SupervisorsManagement", "ITManager");
            }
        }

        public async Task<IActionResult> EditSupervisor(string SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditSupervisorViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isSupervisorExist(SSN))
                {
                    model.SupervisorInfo = await _ITManagerService.GetSupervisorDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditSupervisor(AddEditSupervisorViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatSupervisor(model.UserInfo, model.SupervisorInfo
                , LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string supervisorFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "تعديل بيانات مشرف";
                string logDetails = $"تم تعديل بيانات المشرف ({supervisorFullName}) صاحب الرقم القومي ({model.UserInfo.SSN})";

              
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم تعديل بيانات المشرف بنجاح";
                return RedirectToAction("SupervisorsManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("SupervisorsManagement", "ITManager");
            }
        }

        public async Task<IActionResult> AddNewHeadmaster(string? SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditHeadmasterViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (SSN != null)
                model.SSN = SSN;

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isHeadmasterExist(SSN))
                {
                    model.HeadmasterInfo = await _ITManagerService.GetHeadmasterDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddNewHeadmaster(AddEditHeadmasterViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatHeadmaster(model.UserInfo, model.HeadmasterInfo
                , LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string headmasterFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "إضافة مدير";
                string logDetails = $"تم إضافة مدير جديد باسم ({headmasterFullName}) ورقم قومي ({model.UserInfo.SSN})";

               
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات المدير بنجاح";
                return RedirectToAction("ManagersManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("ManagersManagement", "ITManager");
            }
        }

        public async Task<IActionResult> EditHeadmaster(string SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditHeadmasterViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isHeadmasterExist(SSN))
                {
                    model.HeadmasterInfo = await _ITManagerService.GetHeadmasterDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditHeadmaster(AddEditHeadmasterViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatHeadmaster(model.UserInfo, model.HeadmasterInfo
                , LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string headmasterFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

              
                string logAction = "تعديل بيانات مدير";
                string logDetails = $"تم تعديل بيانات المدير ({headmasterFullName}) صاحب الرقم القومي ({model.UserInfo.SSN})";

                
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                TempData["SuccessMessage"] = "تم حفظ بيانات المدير بنجاح";
                return RedirectToAction("ManagersManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("ManagersManagement", "ITManager");
            }
        }

        public async Task<IActionResult> AddNewITManager(string? SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditITManagerViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if(SSN!=null)
                model.SSN = SSN;

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isITManagerExist(SSN))
                {
                    model.ITManagerInfo = await _ITManagerService.GetITManagerDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddNewITManager(AddEditITManagerViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatITManager(model.UserInfo, model.ITManagerInfo
                , LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string itManagerFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                string logAction = "إضافة مدير أمن معلومات";
                string logDetails = $"تم إضافة مدير أمن معلومات جديد باسم ({itManagerFullName}) ورقم قومي ({model.UserInfo.SSN})";

              
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات مدير امن المعلومات بنجاح";
                return RedirectToAction("ITManagersManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("ITManagersManagement", "ITManager");
            }
        }

        public async Task<IActionResult> EditITManager(string SSN)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new AddEditITManagerViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            if (_ITManagerService.isUserExist(SSN))
            {
                model.UserInfo = await _userService.GetUserInfoForAddingNewRole(SSN);

                if (_ITManagerService.isITManagerExist(SSN))
                {
                    model.ITManagerInfo = await _ITManagerService.GetITManagerDataAsync(model.UserInfo.UserID);
                }

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditITManager(AddEditITManagerViewModel model)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userErrors = _ITManagerService.ValidateUserData(model.UserInfo);


            foreach (var error in userErrors)
            {
                ModelState.AddModelError("", error);
            }




            if (userErrors.Count != 0)
            {
                model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

                return View(model);
            }


            // save code

            model.UserInfo.ProfilePhotoUrl = await _imageService.SaveUserImageAsync(model.ProfileImageFile);

            bool isSuccess = await _ITManagerService.SaveUpdatITManager(model.UserInfo, model.ITManagerInfo
                , LoginUserID);


            if (isSuccess)
            {
                var nameParts = new[]
        {
            model.UserInfo.FirstName,
            model.UserInfo.SecondName,
            model.UserInfo.ThirdName,
            model.UserInfo.LastName
        };

                string itManagerFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                
                string logAction = "تعديل بيانات مدير أمن معلومات";
                string logDetails = $"تم تعديل بيانات مدير أمن المعلومات ({itManagerFullName}) صاحب الرقم القومي ({model.UserInfo.SSN})";

               
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حفظ بيانات مدير امن المعلومات بنجاح";
                return RedirectToAction("ITManagersManagement", "ITManager");
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البيانات";
                return RedirectToAction("ITManagersManagement", "ITManager");
            }
        }

        public async Task<IActionResult> ITManagersManagement()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var model = new ITManagersManagementViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.ITManagerInfos = await _ITManagerService.GetITManagersInfoAsync();

            return View(model);
        }



        public async Task<IActionResult> ManagersManagement()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var model = new ITHeadmastersManagementViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.HeadmasterInfo = await _ITManagerService.GetHeadmastersInfoAsync();

            return View(model);
        }

        //parent manage
        public async Task<IActionResult> ParentManagement()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new ITParentsManagementViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.parents = await _ITManagerService.GetParentsInfoAsync();

         

            return View(model);
        }

       
        public async Task<IActionResult> StudentManagement()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new ITStudentManagementViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.Students = await _ITManagerService.GetStudentsInfoAsync();

            model.Stages=await _ITManagerService.GetStagesInSchoolAsync();

            return View(model);
        }

        public async Task<IActionResult> SubjectsManagement()
        {
            var subjects = await _ITManagerService.GetAllSubjectsAsync();
            var user = _context.Users.FirstOrDefault(e=>e.ID==int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View(subjects);
        }

        public async Task<IActionResult> SubjectsTeachersManagement()
        {
            var bindings = await _ITManagerService.GetAllTeacherSubjectsAsync();
            return View(bindings);
        }

        public async Task<IActionResult> SupervisorsManagement()
        {

            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var model = new ITSupervisorsManagementViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.Supervisors = await _ITManagerService.GetSupervisorsInfoAsync();

            return View(model);
        }

        public async Task<IActionResult> TeacherManagement()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new ITTeachersManagementViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.Teachers = await _ITManagerService.GetTeachersInfoAsync();

            return View(model);
        }

        public async Task<IActionResult> TeacherSubjects(int TeacherID)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new ITTeacherSubjectsViewModel();

            model.NavigationInfo = _userService.GetNavigationData(userId);
            model.TeacherSubjects = await _ITManagerService.GetTeacherSubjects(TeacherID);

            return View(model);
        }

     

        public async Task<IActionResult> ClassesManagement()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.NavigationInfo = _userService.GetNavigationData(userId);
            var classes = await _ITManagerService.GetAllClassesListAsync();
            return View(classes);
        }

       
        public async Task<IActionResult> SudentClassEnrollment()
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new StudentClassEnrollmentPageViewModel();
            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);
            model.Enrollments = await _ITManagerService.GetAllStudentClassEnrollmentsAsync();

            return View(model);
        }
        public IActionResult AddSubjectforStudent(string preFillSSN = null)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.NavigationInfo = _userService.GetNavigationData(userId);
            ViewBag.PreFillSSN = preFillSSN;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchStudentForEnrollment(string type, string value)
        {
            var data = await _ITManagerService.GetStudentEnrollmentDataAsync(type, value);
            if (data == null) return NotFound();
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubjectforStudent(int StudentId, List<int> Subjects)
        {
            if (StudentId > 0)
            {
                await _ITManagerService.SaveStudentSubjectsAsync(StudentId, Subjects ?? new List<int>());
                TempData["SuccessMessage"] = "تم حفظ المواد بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "يرجى البحث عن الطالب وتحديده أولاً";
            }
            return RedirectToAction("AddSubjectforStudent");
        }



        // ================= إضافة ربط المواد بالمعلمين =================

        private async Task PopulateBindingDropdowns()
        {
            ViewBag.Teachers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _ITManagerService.GetTeachersSelectListAsync(), "Id", "Name");
            ViewBag.Subjects = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _ITManagerService.GetSubjectsSelectListAsync(), "Id", "Name");
            ViewBag.Classes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _ITManagerService.GetClassesSelectListAsync(), "Id", "Name");
        }

        public async Task<IActionResult> AddBinding()
        {
            await PopulateBindingDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBinding(TeacherSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _ITManagerService.AddTeacherSubjectAsync(model);
                return RedirectToAction("SubjectsTeachersManagement");
            }
            await PopulateBindingDropdowns();
            return View(model);
        }

        // ================= تعديل ربط المعلمين بالمواد =================

        public async Task<IActionResult> EditBinding(int id)
        {
            var binding = await _ITManagerService.GetTeacherSubjectByIdAsync(id);
            if (binding == null) return NotFound();

            await PopulateBindingDropdowns();
            return View(binding);
        }

        [HttpPost]
        public async Task<IActionResult> EditBinding(TeacherSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _ITManagerService.UpdateTeacherSubjectAsync(model);
                return RedirectToAction("SubjectsTeachersManagement");
            }
            await PopulateBindingDropdowns();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBinding(int id)
        {
            await _ITManagerService.DeleteTeacherSubjectAsync(id);
            return RedirectToAction("SubjectsTeachersManagement");
        }



        // ================= إضافة ماده =================

        public IActionResult AddSubject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _ITManagerService.AddSubjectAsync(model);
                return RedirectToAction("SubjectsManagement");
            }
            return View(model);
        }

        // ================= تعديل ماده =================

        public async Task<IActionResult> EditSubject(int id)
        {
            var subject = await _ITManagerService.GetSubjectByIdAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubject(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _ITManagerService.UpdateSubjectAsync(model);
                return RedirectToAction("SubjectsManagement");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            await _ITManagerService.DeleteSubjectAsync(id);
            return RedirectToAction("SubjectsManagement");
        }




        // ================= إضافة فصل =================//

        public async Task<IActionResult> AddClass()
        {
            var model = new AddEditClassViewModel();
            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddClass(AddEditClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _ITManagerService.IsClassExistsAsync(model.LevelID, model.ClassName))
                {
                    ModelState.AddModelError("ClassName", "اسم الفصل موجود بالفعل في هذا الصف.");
                }
                else
                {
                    bool isSuccess = await _ITManagerService.SaveNewClassAsync(model);
                    if (isSuccess)
                    {
                        int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                        string logAction = "إضافة فصل";
                        string logDetails = $"تم إضافة فصل جديد باسم ({model.ClassName})";

                        // 2. Save the log
                        bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                        TempData["SuccessMessage"] = "تم إضافة الفصل بنجاح";
                        return RedirectToAction("ClassesManagement");
                    }
                    else
                    {
                        ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات.");
                    }
                }
            }
            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();
            return View(model);
        }

        // ================= تعديل فصل    =================//
        public async Task<IActionResult> EditClass(int id)
        {
            var model = await _ITManagerService.GetClassForEditAsync(id);
            if (model == null) return NotFound();

            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditClass(AddEditClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _ITManagerService.IsClassExistsAsync(model.LevelID, model.ClassName, model.ID))
                {
                    ModelState.AddModelError("ClassName", "اسم الفصل موجود بالفعل في هذا الصف.");
                }
                else
                {
                    bool isSuccess = await _ITManagerService.UpdateClassAsync(model);
                    if (isSuccess)
                    {
                        int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                        string logAction = "تعديل فصل";
                        string logDetails = $"تم تعديل بيانات الفصل ({model.ClassName})";

                        
                        bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                        TempData["SuccessMessage"] = "تم تعديل الفصل بنجاح";
                        return RedirectToAction("ClassesManagement");
                    }
                    else
                    {
                        ModelState.AddModelError("", "حدث خطأ أثناء تعديل البيانات.");
                    }
                }
            }
            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClass(int id)
        {
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var classToDelete = await _context.Classes.FirstOrDefaultAsync(c => c.ID == id);

            bool isSuccess = await _ITManagerService.DeleteClassAsync(id);

            if (isSuccess)
            {

                string deletedClassName = classToDelete != null ? classToDelete.Name : $"بمعرف {id}";

               
                string logAction = "حذف فصل";
                string logDetails = $"تم حذف الفصل ({deletedClassName})";

                
                bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);
                TempData["SuccessMessage"] = "تم حذف الفصل بنجاح";

            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الفصل. قد يكون مرتبطاً ببيانات أخرى.";
            }
            return RedirectToAction("ClassesManagement");
        }




        // ================= إضافة تسجيل الطلاب في فصل =================//

        private async Task PopulateEnrollmentDropdowns(AddEditStudentClassEnrollmentViewModel model)
        {
            model.Stages = await _ITManagerService.GetStagesInSchoolAsync();
            model.Students = await _ITManagerService.GetStudentsSelectListForEnrollmentAsync();
            model.AcademicTerms = await _ITManagerService.GetAcademicTermsSelectListAsync();
        }

        public async Task<IActionResult> Addsudentclass()
        {
            var model = new AddEditStudentClassEnrollmentViewModel();
            await PopulateEnrollmentDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentClass(AddEditStudentClassEnrollmentViewModel model)
        {

            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                // Check duplicate enrollment
                if (await _ITManagerService.IsStudentAlreadyEnrolledInTermAsync(model.StudentId, model.AcademicTermId))
                {
                    ModelState.AddModelError("", "هذا الطالب مسجل بالفعل في هذا الترم الدراسي");
                    await PopulateEnrollmentDropdowns(model);
                    return View("Addsudentclass", model);
                }

                bool isSuccess = await _ITManagerService.SaveStudentClassEnrollmentAsync(model);
                if (isSuccess)
                {
                    var student = await _context.Users.FirstAsync(u => u.ID == model.StudentId);

                    var enrolledClass = await _context.Classes
                                                      .Include(c => c.Level)
                                                      .FirstAsync(c => c.ID == model.ClassId);

                    
                    var nameParts = new[]
                    {
                        student.FirstName,
                        student.SecondName,
                        student.ThirdName,
                        student.LastName
                    };

                    string studentFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                    string logAction = "تسجيل طالب";
                    string logDetails = $"تم تسجيل الطالب ({studentFullName}) في المرحلة ({enrolledClass.Level.Name}) فصل ({enrolledClass.Name})";


                    bool LogAdded = await _logService.CreateLogAsync(LoginUserID, logAction, logDetails);

                    TempData["SuccessMessage"] = "تم تسجيل الطالب في الفصل بنجاح";
                    return RedirectToAction("SudentClassEnrollment");
                }
                else
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات");
                }
            }

            await PopulateEnrollmentDropdowns(model);
            return View("Addsudentclass", model);
        }

        // ================= تعديل تسجيل الطلاب في فصل =================//

        public async Task<IActionResult> EditSudentclass(int id)
        {
            var model = await _ITManagerService.GetStudentClassEnrollmentByIdAsync(id);
            if (model == null) return NotFound();

            await PopulateEnrollmentDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudentClass(AddEditStudentClassEnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check duplicate enrollment (exclude current)
                if (await _ITManagerService.IsStudentAlreadyEnrolledInTermAsync(model.StudentId, model.AcademicTermId, model.ID))
                {
                    ModelState.AddModelError("", "هذا الطالب مسجل بالفعل في هذا الترم الدراسي");
                    await PopulateEnrollmentDropdowns(model);
                    return View("Editsudentclass", model);
                }

                bool isSuccess = await _ITManagerService.UpdateStudentClassEnrollmentAsync(model);
                if (isSuccess)
                {
                    var student = await _context.Users.FirstAsync(u => u.ID == model.StudentId);

                    var enrolledClass = await _context.Classes
                                                      .Include(c => c.Level)
                                                      .FirstAsync(c => c.ID == model.ClassId);

                   
                    var nameParts = new[]
                    {
                student.FirstName,
                student.SecondName,
                student.ThirdName,
                student.LastName
            };
                    string studentFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                    string logAction = "تعديل تسجيل طالب";
                    string logDetails = $"تم تعديل تسكين الطالب ({studentFullName}) ليصبح في المرحلة ({enrolledClass.Level.Name}) فصل ({enrolledClass.Name})";
                    TempData["SuccessMessage"] = "تم تعديل بيانات التسجيل بنجاح";
                    return RedirectToAction("SudentClassEnrollment");
                }
                else
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء تعديل البيانات");
                }
            }

            await PopulateEnrollmentDropdowns(model);
            return View("Editsudentclass", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudentClassEnrollment(int id)
        {
            bool isSuccess = await _ITManagerService.DeleteStudentClassEnrollmentAsync(id);
            if (isSuccess)
            {
                TempData["SuccessMessage"] = "تم حذف التسجيل بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف التسجيل";
            }
            return RedirectToAction("SudentClassEnrollment");
        }










    }
}
