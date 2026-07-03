using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class TeacherViewModel:BaseUserViewModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime HireDate { get; set; }
        public DateTime? ExiteDate { get; set; }



    }
}
