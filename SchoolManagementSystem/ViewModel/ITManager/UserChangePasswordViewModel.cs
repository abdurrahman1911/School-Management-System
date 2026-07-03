using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class UserChangePasswordViewModel
    {
        [ValidateNever]
        public NavigationViewModel NavigationInfo { get; set; }

        [ValidateNever]
        public UserChangePasswordData UserData { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوب")]
        public string Password { get; set; }
        [Required(ErrorMessage = "تأكيد كلمة السر مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة السر و تأكيد كلمة السر يجب ان يكونوا متطابقين")]
        public string ConfirmPassword {  get; set; }
    }

    public class UserChangePasswordData
    {
        public string SSN {  get; set; }
        public string Name { get; set; }

        public string Phone {  get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public string PhotoURL {  get; set; }
    }

   
}
