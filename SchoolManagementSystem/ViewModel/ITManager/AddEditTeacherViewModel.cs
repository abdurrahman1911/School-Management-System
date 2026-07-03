using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AddEditTeacherViewModel
    {
        public NavigationViewModel? NavigationInfo { get; set; }

        public string? SSN { get; set; }
        public UserInfo UserInfo { get; set; }

        public TeacherData TeacherInfo { get; set; }
        public IFormFile? ProfileImageFile { get; set; }


    }

   


    public class TeacherData
    {
        public int TeacherUserID { get; set; }

        public int TeacherID { get; set; }


        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }

        public DateTime? ExitDate { get; set; }
    }


}
