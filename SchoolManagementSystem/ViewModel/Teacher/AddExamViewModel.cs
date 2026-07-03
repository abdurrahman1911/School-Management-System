using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class AddExamViewModel
    {
        [Required(ErrorMessage = "اسم الاختبار مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "اسم الاختبار يجب أن يكون بين 3 و 100 حرف")]
        [Display(Name = "اسم الاختبار")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى اختيار المادة")]
        [Range(1, int.MaxValue, ErrorMessage = "اختيار المادة غير صحيح")]
        [Display(Name = "المادة")]
        public int SelectedSubjectId { get; set; }

        public List<IdNameViewModel>? Subjects { get; set; }

        [Required(ErrorMessage = "يرجى اختيار نوع الاختبار")]
        [Display(Name = "نوع الاختبار")]
        public string Type { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الصف الدراسي")]
        [Range(1, int.MaxValue, ErrorMessage = "اختيار الصف غير صحيح")]
        [Display(Name = "الصف الدراسي")]
        public int SelectedClassId { get; set; }

        public List<IdNameViewModel>? Classes { get; set; }

        [Required(ErrorMessage = "تاريخ الاختبار مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الاختبار")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "وقت الاختبار مطلوب")]
        [DataType(DataType.Time)]
        [Display(Name = "وقت الاختبار")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "مدة الاختبار مطلوبة")]
        [Range(5, 300, ErrorMessage = "المدة يجب أن تكون بين 5 دقائق و 300 دقيقة")]
        [Display(Name = "المدة (دقائق)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "يجب إضافة أسئلة للاختبار")]
        [MinLength(1, ErrorMessage = "يجب إضافة سؤال واحد على الأقل")]
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }

    public class QuestionViewModel
    {
        [Required(ErrorMessage = "نص السؤال مطلوب")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "نص السؤال يجب أن يكون بين 1 و 300 حرف")]
        [Display(Name = "نص السؤال")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "درجة السؤال مطلوبة")]
        [Range(1, 100, ErrorMessage = "الدرجة يجب أن تكون بين 1 و 100")]
        [Display(Name = "درجة السؤال")]
        public int QuestionDegree { get; set; } = 1;

        [Required(ErrorMessage = "يجب إضافة خيارات للسؤال")]
        public OptionViewModel Choices { get; set; } = new OptionViewModel();
    }
    public class OptionViewModel
    {
        [Required(ErrorMessage = "يجب تحديد الإجابة الصحيحة")]
        [Display(Name = "الإجابة الصحيحة")]
        public int CorrectOptionIndex { get; set; }

        [Required(ErrorMessage = "يجب إضافة خيارات الإجابة")]
        [MinLength(2, ErrorMessage = "يجب إضافة خيارين على الأقل")]
        public List<string> Options { get; set; } = new List<string>();
    }



}