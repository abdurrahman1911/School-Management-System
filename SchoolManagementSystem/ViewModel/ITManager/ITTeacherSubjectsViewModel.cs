using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITTeacherSubjectsViewModel
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
