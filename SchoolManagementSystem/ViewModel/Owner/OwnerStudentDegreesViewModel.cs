namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerStudentDegreesViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }



        public StudentDegreeInfo StudentDegree { get; set; }

    }

    
    public class StudentDegreeInfo
    {
        public string StudentFullName { get; set; }

        public List<ExamDegreeInfo> ExamsDegree { get; set; }

    }

    public class ExamDegreeInfo
    {
        
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public string ExamType { get; set; }
        public DateTime ExamDate { get; set; }
        public decimal TotalDegree { get; set; }
        public int ExamDurationMinutes { get; set; }
        public decimal StudentDegree { get; set; }
    }
}
