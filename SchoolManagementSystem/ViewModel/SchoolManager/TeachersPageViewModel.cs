using System.Collections.Generic;

namespace SchoolManagement.Models.ViewModels
{
    // الـ ViewModel الرئيسي الذي يتم تمريره للصفحة
    public class TeachersPageViewModel
    {
        public string ManagerName { get; set; }
        public string ManagerPhotoUrl { get; set; }
        public int TotalTeachersCount { get; set; }
        public List<TeacherCardViewModel> Teachers { get; set; }
    }

    public class TeacherCardViewModel
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; } // 🌟 تعديل النوع هنا ليصبح int
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public string HireDate { get; set; }
        public string TeacherScheduleUrl { get; set; }
        public List<string> Subjects { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
    }
}