namespace SchoolManagementSystem.ViewModel.Student
{
    public class StudentGradesViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        public string GradeName { get; set; }

        public List<StudentGradeInfo> Grades { get; set; }
    }

    public class StudentGradeInfo
    {
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public decimal Degree { get; set; }
        public decimal TotalDegree { get; set; }
        public string Score { get; set; }
        public string Date { get; set; }
    }
}
