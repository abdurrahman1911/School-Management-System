using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class AddAssignmentViewModel
    {
        [Required(ErrorMessage = "اسم الواجب مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم طويل جداً، بحد أقصى 100 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى اختيار المادة")]
        [Range(1, int.MaxValue, ErrorMessage = "اختيار المادة غير صحيح")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الفصل")]
        [Range(1, int.MaxValue, ErrorMessage = "اختيار الفصل غير صحيح")]
        public int ClassId { get; set; }



        [Required(ErrorMessage = "تاريخ البدء مطلوب")]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "تاريخ الاستحقاق مطلوب")]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; } = DateTime.Now.AddDays(3);

        [Required(ErrorMessage = "يرجى رفع ملف الواجب")]
        public IFormFile file { get; set; }

        public List< IdNameViewModel>? Subjects { get; set; }
        public List< IdNameViewModel>? Classes { get; set; }
        
    }
}