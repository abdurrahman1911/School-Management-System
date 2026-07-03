using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; 

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class EditAssignmentViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "اسم الواجب مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم طويل جداً، بحد أقصى 100 حرف")]
        public string Name { get; set; }

        public string SubjectName { get; set; }

        public string ClassName { get; set; }

        [Required(ErrorMessage = "تاريخ البدء مطلوب")]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "تاريخ الاستحقاق مطلوب")]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; } = DateTime.Now.AddDays(3);

        public IFormFile? File { get; set; }

        public string? ExistingFilePath { get; set; }
    }
}