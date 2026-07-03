using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class AddNoteViewModel
    {
        [Required(ErrorMessage = "لا يمكنك إرسال ملاحظة فارغة.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "يجب أن يكون نص الملاحظة بين 5 إلى 500 حرف.")]
        public string Note { get; set; }
        public int StudentId { get; set; }

    }
}
