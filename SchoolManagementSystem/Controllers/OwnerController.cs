using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.Owner;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Controllers
{
    [Authorize (Roles ="Owner")] 
    public class OwnerController : Controller
    {

        private readonly OwnerService _ownerService;
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly LogService _logService;


        public OwnerController(OwnerService ownerService, AppDbContext context, UserService userService, LogService logService)
        {

            _ownerService = ownerService;
            _context = context;
            _userService = userService;
            _logService = logService;

        }

        

        public async Task<IActionResult> StudentDegrees(int StudentID)
        {

            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = new OwnerStudentDegreesViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.StudentDegree = await _ownerService.GetStudentDegreeInfosAsync(StudentID);


            return View(model);
        }

        public IActionResult UserAbcense(int UserID)
        {

            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new UserAbcenseViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserID);

            model.UserFullName = _userService.GetUserFullName(UserID);

            model.Absences = _userService.GetUserAbsencesForLastTerm(UserID);

            
            return View(model);
        }

        public IActionResult Dashboard() {
            
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model= _ownerService.GetOwnerDashboardData(userId);

           return  View(model);
        }

        public IActionResult Editdata() {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            OwnerEditDataViewModel model = new OwnerEditDataViewModel();

            model.NavigationViewModel = _userService.GetNavigationData(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editdata(OwnerEditDataViewModel model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                
                model.NavigationViewModel=_userService.GetNavigationData(userId);
                ModelState.AddModelError("", "الباسورد لازم يكون 8 حروف على الأقل");
                return View(model);
            }

            var user = _context.Users.Find(userId);
            bool check =await _userService.Setting(
                user,
                model.CurrentPassword,
                model.NewPassword
                );

            if (!check)
            {
                ModelState.AddModelError("", "كلمة المرور الحالية غير صحيحة");
                return View(model);
            }
            else
            {
                return RedirectToAction("Dashboard", "Owner");
            }
        }

        public async Task<IActionResult> Student() {

            int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new OwnerStudentsViewModel();

            model.NavigationViewModel = _userService.GetNavigationData(UserId);

            model.SchoolStudents = await _ownerService.GetStudentsInfoAsync();

            model.Stages=await _ownerService.GetStagesInSchoolAsync();


            return View(model);
            
                
        }

        public async Task<IActionResult> Successandfailure(int? TermID=null){

            int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model=new OwnerSuccessFailerViewModel();

            model.NavigationInfo = _userService.GetNavigationData(UserId);

            model.termsFiltersInfo=await _ownerService.GetAcademicTermsFilterInfoAsync();

            model.StagesSuccessFailerInfos = await _ownerService.GetStagesSuccessFailerInfosAsync(TermID);


            return View(model);
        }


        public async Task<IActionResult> Supervisors() {
            
            var model = new OwnerSupervisorsViewModel();

            int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            model.NavigationData= _userService.GetNavigationData(UserId);

            model.SchoolSupervisors = await  _ownerService.GetOwnerSupervisorsDataAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(int UserID)
        {
           var model = new UserViewModel();
            int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            model.NavigationViewModel = _userService.GetNavigationData(UserId);
            model.UserDataInfo= await _userService.GetUserDataInfoForPresentationAsync(UserID);

            return View(model);
        }
        public async Task<IActionResult> Teachers()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new OwnerTeachersViewModel();

            model.NavigationInfo = _userService.GetNavigationData(userId);
            model.TeachersInfo = await _ownerService.GetTeachersInfo();

            return View(model);
        }

        public async Task<IActionResult> TeacherSubjects(int TeacherID)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new OwnerTeacherSubjectsViewModel();

            model.NavigationInfo = _userService.GetNavigationData(userId);
            model.TeacherSubjects = await _ownerService.GetTeacherSubjects(TeacherID);

            return View(model);
        }
        public async Task<IActionResult> Notes()
        {

            var LoginUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = new OwnerNotesViewModel();

            model.NavigationInfo = _userService.GetNavigationData(LoginUserId);

            model.OwnerNotesInfo= await _ownerService.GetOwnerActorNotesInfo();

            

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Notifications(int page = 1)
        {
            int pageSize = 20;
            int LoginUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

           
            var (logs, currentPage, totalPages) = await _logService.GetPagedLogsAsync(page, pageSize);

         
            var model = new OwnerNotificationsViewModel
            {
                NavigationInfo = _userService.GetNavigationData(LoginUserID),
                Logs = logs,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };

            return View(model);
        }

    }
}