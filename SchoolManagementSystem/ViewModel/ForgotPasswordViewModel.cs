using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }
    }
}
