using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AddOrEditeAcademicYearViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "اسم السنة الدراسية مطلوب.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "اسم السنة يجب أن يكون بين 4 إلى 50 أحرف (مثال: 2025-2026).")]
        [Display(Name = "اسم السنة الدراسية")]
        public string Name { get; set; }

        [Required(ErrorMessage = "تاريخ بداية السنة الدراسية مطلوب.")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ البداية")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ نهاية السنة الدراسية مطلوب.")] 
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ النهاية")]
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