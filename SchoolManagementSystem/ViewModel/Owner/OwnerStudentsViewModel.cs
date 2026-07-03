using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel.ITManager;

namespace SchoolManagementSystem.ViewModel.Owner
{


    public class OwnerStudentsViewModel
    {
        public NavigationViewModel NavigationViewModel { get; set; }

        public List<StudentInfo> SchoolStudents { get; set; }

        public List<StageInSchool>? Stages { get; set; }
    }

    public class StudentInfo
    {
        public int StudentUserID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentPhone { get; set; }
        public string StudentSSN { get; set; }
        public int ParentUserID { get; set; }
        public string ParentName { get; set; }
        public string ParentRelation { get; set; }
        public string ParentPhone { get; set; }

        public string ParentSSN {  get; set; }
        public int ClassID { get; set; }
        public string  ClassName { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public int StageID { get; set; }
        public string StageName { get; set; }
        public DateTime? ExitDate { get; set; }
        public DateTime JoinDate { get; set; }
        public int AbsencesCount { get; set; }

    }


    public class StageInSchool
    {
        public Stage Stage { get; set; }

        public List<LevelInStage> Levels { get; set; }

    }

    public class LevelInStage
    {
        public Level Level { get; set; }

        public List<Class> Classes { get; set; }

    }



}
