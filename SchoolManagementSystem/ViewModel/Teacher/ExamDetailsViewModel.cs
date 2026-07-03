using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class ExamDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "اسم الاختبار")]
        public string Name { get; set; }

        [Display(Name = "المادة")]
        public string SubjectName { get; set; }

        [Display(Name = "نوع الاختبار")]
        public string Type { get; set; }

        [Display(Name = "الصف الدراسي")]
        public string ClassName { get; set; }

        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "وقت الاختبار")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Display(Name = "المدة")]
        public int Duration { get; set; }

        [Display(Name = "إجمالي الدرجات")]
        public decimal TotalMarks { get; set; }

        [Display(Name = "الأسئلة")]
        public List<QuestionDetailsViewModel> Questions { get; set; } = new List<QuestionDetailsViewModel>();
    }

    public class QuestionDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نص السؤال")]
        public string Text { get; set; }

        [Display(Name = "الدرجة")]
        public decimal Mark { get; set; }

        [Display(Name = "الخيارات المتاحة")]
        public List<ChoiceViewModel> Choices { get; set; } = new List<ChoiceViewModel>();
    }

    public class ChoiceViewModel
    {
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}