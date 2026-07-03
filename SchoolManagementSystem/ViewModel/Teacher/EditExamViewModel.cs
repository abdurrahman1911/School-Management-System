using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class EditExamViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الاختبار مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "اسم الاختبار يجب أن يكون بين 3 و 100 حرف")]
        [Display(Name = "اسم الاختبار")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى اختيار المادة")]
        [Display(Name = "المادة")]
        public int SelectedSubjectId { get; set; }
        public List<IdNameViewModel>? Subjects { get; set; }

        [Required(ErrorMessage = "يرجى اختيار نوع الاختبار")]
        [Display(Name = "نوع الاختبار")]
        public string Type { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الصف الدراسي")]
        [Display(Name = "الصف الدراسي")]
        public int SelectedClassId { get; set; }
        public List<IdNameViewModel>? Classes { get; set; }

        [Required(ErrorMessage = "تاريخ الاختبار مطلوب")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "مدة الاختبار مطلوبة")]
        [Range(5, 300, ErrorMessage = "المدة يجب أن تكون بين 5 دقائق و 300 دقيقة")]
        [Display(Name = "المدة (دقائق)")]
        public int Duration { get; set; }

        public List<EditQuestionViewModel> Questions { get; set; } = new List<EditQuestionViewModel>();
    }

    public class EditQuestionViewModel
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "نص السؤال مطلوب")]
        public string Text { get; set; }

        [Required(ErrorMessage = "الدرجة مطلوبة")]
        [Range(1, 100, ErrorMessage = "الدرجة يجب أن تكون بين 1 و 100")]
        public decimal Mark { get; set; }=1;

        public List<EditChoiceViewModel> Choices { get; set; } = new List<EditChoiceViewModel>();
    }

    public class EditChoiceViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نص الاختيار مطلوب")]
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}