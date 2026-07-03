namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class ExamGradesViewModel
    {
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalDegree { get; set; }
        public int StudentCount { get; set; }
        public decimal SuccessDegree { get; set; }
        public List<StudentGrades> StudentGrades{ get; set; }

    }

    public class StudentGrades
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal grade { get; set; }
        public string status { get; set; }
        public DateTime Date { get; set; }
    }
}
