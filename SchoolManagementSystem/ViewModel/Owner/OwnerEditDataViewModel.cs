using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerEditDataViewModel
    {
        
        public NavigationViewModel? NavigationViewModel { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "الباسورد لازم يكون 8 حروف على الأقل")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }
    }
}
