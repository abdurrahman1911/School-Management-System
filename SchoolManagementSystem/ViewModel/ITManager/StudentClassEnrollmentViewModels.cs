using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    // ViewModel for the main enrollment listing page
    public class StudentClassEnrollmentPageViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }
        public List<StudentClassEnrollmentListItem> Enrollments { get; set; } = new();
    }

    // Each row in the enrollments table
    public class StudentClassEnrollmentListItem
    {
        public int EnrollmentID { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string SSN { get; set; }
        public string Phone { get; set; }
        public string StageName { get; set; }
        public string LevelName { get; set; }
        public string ClassName { get; set; }
        public string AcademicTermName { get; set; }
        public bool IsPassed { get; set; }
    }

    // ViewModel for add/edit enrollment form
    public class AddEditStudentClassEnrollmentViewModel
    {
        public int? ID { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الطالب")]
        public int StudentId { get; set; }

        // Helper for cascading dropdown (not saved directly)
        public int StageID { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الصف")]
        public int LevelID { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الفصل")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الترم الدراسي")]
        public int AcademicTermId { get; set; }

        public bool IsPassed { get; set; }

        // Dropdown data
        public List<StageInSchool>? Stages { get; set; }
        public List<AcademicTermSelectItem>? AcademicTerms { get; set; }
        public List<StudentSelectItem>? Students { get; set; }
    }

    public class StudentSelectItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AcademicTermSelectItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
