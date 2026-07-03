using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.ViewModel.Teacher;

namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class AddNotesViewModel
    {
        [Required(ErrorMessage = "يرجى اختيار المعلم أولاً.")]
        [Range(1, int.MaxValue, ErrorMessage = "يرجى اختيار معلم صحيح.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "لا يمكنك إرسال ملاحظة فارغة.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "يجب أن يكون نص الملاحظة بين 5 إلى 500 حرف.")]
        public string Note { get; set; }

        public List<IdNameViewModel>? Teachers { get; set; } = new List<IdNameViewModel>();
        public NavigationViewModel? NavigationInfo { get; set; } = new NavigationViewModel();
    }
}