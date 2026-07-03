namespace SchoolManagementSystem.ViewModel.SchoolManager
{
    public class StudentGrades
    {
        public Dictionary<int, StudentGradesData> Students { get; set; }
    }



    public class StudentGradesData
    {
        public Dictionary<string, SubjectGrades> Subjects { get; set; }
    }


    public class SubjectGrades
    {
        public Dictionary<string, int> Exams { get; set; }
    }

}
