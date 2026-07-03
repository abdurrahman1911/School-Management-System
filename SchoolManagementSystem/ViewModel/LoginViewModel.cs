using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "الرقم القومي او جواز السفر مطلوب")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "من فضلك اختر الدور الوظيفي")]
        [Range(1, 8, ErrorMessage = "من فضلك اختر الدور الوظيفي")]
        public byte? UserType { get; set; }

        public bool RememberMe { get; set; }
    }
}