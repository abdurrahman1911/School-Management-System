using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel
{
    public class ParentViewModel:BaseUserViewModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int UserId { get; set; }
    }
}
