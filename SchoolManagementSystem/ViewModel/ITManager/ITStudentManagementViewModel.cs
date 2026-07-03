using SchoolManagementSystem.ViewModel.Owner;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITStudentManagementViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<StageInSchool>? Stages { get; set; }

        public List<StudentInfo> Students { get; set; }

    }

    public class StudentInfo
    {
        public int ID {  get; set; }
        public string SSN { get; set; }
        public string PhoneNumber {  get; set; }
        public string StudentName { get; set; }
        public int ParentID { get; set; }
        public string ParentName {  get; set; }
        public string StageName {  get; set; }
        public int StageId { get; set; }    
        public string ClassName {  get; set; }
        public int ClassID {  get; set; }
        public string LevelName {  get; set; }
        public int LevelId { get; set; }
        public DateTime JoinDate {  get; set; }

    }
}
