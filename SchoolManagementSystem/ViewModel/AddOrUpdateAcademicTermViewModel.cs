using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SchoolManagementSystem.ViewModel.Teacher;

namespace SchoolManagementSystem.ViewModel
{
    public class AddOrUpdateAcademicTermViewModel: IValidatableObject
    {
        public List<IdNameViewModel> AcademicYears { get; set; } = new List<IdNameViewModel>();

        [Required(ErrorMessage = "برجاء اختيار العام الدراسي")]
        [Display(Name = "العام الدراسي")]
        public int SelectedAcademicYearId { get; set; }

        [Required(ErrorMessage = "برجاء اختيار الفصل الدراسي")]
        public byte TermNumber { get; set; }

        [Required(ErrorMessage = "برجاء تحديد تاريخ البداية")]
        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "برجاء تحديد تاريخ النهاية")] 
        [Display(Name = "تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate.HasValue && EndDate.Value <= StartDate)
            {
                yield return new ValidationResult(
                    "تاريخ النهاية يجب أن يكون بعد تاريخ البداية.",
                    new[] { nameof(EndDate) }
                );
            }
        }
    }
}