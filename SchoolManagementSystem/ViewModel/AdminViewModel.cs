using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel
{
    public class AdminViewModel:BaseUserViewModel
    {
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        
        public int UserId { get; set; }

    }
}
