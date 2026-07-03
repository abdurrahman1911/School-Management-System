using System.Collections.Generic;

namespace SchoolManagement.Models.ViewModels
{
    public class StudentsPageViewModel
    {
        public string ManagerName { get; set; }
        public string ManagerPhotoUrl { get; set; }

        // قوائم الفلترة الشجرية (مرحلة -> صف -> فصل)
        public List<StageSelectionViewModel> Stages { get; set; }

        // قوائم تحتوي على "كل البيانات" لتحميلها أول مرة في الصفحة
        public List<StudentResultViewModel> AllStudents { get; set; }
        public List<SubjectViewModel> AllSubjects { get; set; }
        public List<ExamResultViewModel> AllExams { get; set; }
        public List<GradeRowViewModel> InitialGrades { get; set; }
    }

    public class StageSelectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LevelSelectionViewModel> Levels { get; set; }
    }

    public class LevelSelectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ClassSelectionViewModel> Classes { get; set; }
    }

    public class ClassSelectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StudentResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ExamResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GradeRowViewModel
    {
        public int Index { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public decimal Degree { get; set; }
        public decimal TotalDegree { get; set; } = 100;
        public decimal Percentage => TotalDegree > 0 ? (Degree / TotalDegree) * 100 : 0;
        public string Status => Percentage >= 50 ? "ناجح" : "راسب";
    }
}