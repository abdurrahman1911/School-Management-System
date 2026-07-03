using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class UsersChangePasswordViewModel
    {
        public NavigationViewModel? NavigationInfo { get; set; }

        public RequireInfo RequireInfo { get; set; }

    }

    public class RequireInfo
    {

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        public string SSN { get; set; }
    }
}
