namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class RegisterSubjectsForClassViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<SubjectsData> Subjects { get; set; }

        public int TargetTermID {  get; set; }

        public int ClassID { get; set; }

        public int LevelID { get; set; }

        public int StageID { get; set; }
    }

    public class SubjectsData
    {
        public int SubjectID { get; set; }

        public string SubjectName { get; set; }

        public List<SubjectTeacherData> SubjectTeachers { get; set; }

        public int selectedTeacherID { get; set; }
        public bool isSelected { get; set; }


    }

    public class SubjectTeacherData
    {
        public int TeacherID { get; set; }

        public string TeacherName { get; set; }

    }
}
