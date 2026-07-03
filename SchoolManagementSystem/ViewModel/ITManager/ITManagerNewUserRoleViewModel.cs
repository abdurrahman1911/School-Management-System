using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITManagerNewUserRoleViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string NewUserSSN { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public Services.UserTypeEnum userType { get; set; }

    }

    
}
