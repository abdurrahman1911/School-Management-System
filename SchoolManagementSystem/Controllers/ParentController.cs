using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.Parent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Controllers
{
  //  [Authorize(Roles = "Parent")]
    public class ParentController : Controller
    {
        private readonly ParentService _parentService;
        private readonly AppDbContext _context;
        private readonly UserService _userService;

        public ParentController(ParentService parentService, AppDbContext context, UserService userService)
        {
            _parentService = parentService;
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _parentService.GetParentIndexData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Performance()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _parentService.GetPerformanceData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Schedule()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _parentService.GetScheduleData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Grades()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var viewModel = _parentService.GetGradesData(userId);

            if (viewModel == null)
                return RedirectToAction("Login", "Home");

            return View(viewModel);
        }

        public IActionResult Attendance()
        {

            // 1. Get the currently logged-in parent's ID 
            // (You might be getting this from session, claims, or a parameter)
            var ParentUserID =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // Example ID

            var loggedInParentObj = _context.Parents.FirstOrDefault(p=> p.UserId==ParentUserID); // Assuming you have a way to get the parent ID from the user

            if(loggedInParentObj == null)
            {
                // Handle the case where the parent ID is not found (e.g., redirect to an error page or show a message)
                return RedirectToAction("Index", "Home");
            }

            // Query the database to get the children id and name of the logged-in parent
            var children = _parentService.GetChildrenForDropdown(loggedInParentObj.ID);

            // Pass it to the ViewBag
            ViewBag.ChildrenList = children;

            // the absences of children
            ChildrenAbsenceViewModel childrenAbsenceViewModel = _parentService.GetChildrenAbsences(loggedInParentObj.ID);

            return View(childrenAbsenceViewModel);
        }

        public IActionResult Assignments()
        {
            // 1. Get the currently logged-in parent's ID 
            // (You might be getting this from session, claims, or a parameter)
            var ParentUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // Example ID

            var loggedInParentObj = _context.Parents.FirstOrDefault(p => p.UserId == ParentUserID); // Assuming you have a way to get the parent ID from the user

            if (loggedInParentObj == null)
            {
                // Handle the case where the parent ID is not found (e.g., redirect to an error page or show a message)
                return RedirectToAction("Index", "Home");
            }

            // Query the database to get the children id and name of the logged-in parent
            var children = _parentService.GetChildrenForDropdown(loggedInParentObj.ID);

            // Pass it to the ViewBag
            ViewBag.ChildrenList = children;

            // the assignments of children
           
            ChildrenAssignmentsViewModel childrenAssignmentsViewModel = _parentService.GetChildrenAssignments(loggedInParentObj.ID);
            return View(childrenAssignmentsViewModel);
        }

        public IActionResult Edit()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var indexData = _parentService.GetParentIndexData(userId);

            if (indexData != null)
            {
                ViewBag.ParentFullName = indexData.ParentFullName;
                ViewBag.ParentFirstLetter = indexData.ParentFirstLetter;
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SettingViewModel model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var indexData = _parentService.GetParentIndexData(userId);

            if (indexData != null)
            {
                ViewBag.ParentFullName = indexData.ParentFullName;
                ViewBag.ParentFirstLetter = indexData.ParentFirstLetter;
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "الرجاء التأكد من صحة البيانات المدخلة");
                return View(model);
            }

            var user = _context.Users.Find(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
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
                return RedirectToAction("Index", "Parent");
            }
        }

    }
}
