using SchoolManagementSystem.Models;
namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerTeacherSubjectsViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public TeacherSubjects TeacherSubjects { get; set; }
    }

    public class TeacherSubjects
    {
        public string FullName { get; set; }

        public List<Subject> Subjects { get; set; }
    }

    
}
