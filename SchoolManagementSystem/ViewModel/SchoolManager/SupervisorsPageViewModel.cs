using System.Collections.Generic;

namespace SchoolManagementSystem.ViewModel.SchoolManager
{
    // 1️⃣ الموديل الرئيسي للصفحة
    public class SupervisorsPageViewModel
    {
        public string ManagerName { get; set; }
        public string ManagerPhotoUrl { get; set; }
        public int TotalSupervisorsCount { get; set; }

        // قائمة كروت المشرفين تعتمد على الكلاس الموجود بالأسفل
        public List<SupervisorCardViewModel> Supervisors { get; set; } = new List<SupervisorCardViewModel>();
    }

    // 2️⃣ كلاس كارت المشرف (تم وضعه في نفس الملف لتبسيط الاستدعاء)
    public class SupervisorCardViewModel
    {
        public int SupervisorId { get; set; }
        public int UserId { get; set; } // المعرف الرقمي لربط الملاحظات
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public string HireDate { get; set; }
    }
}