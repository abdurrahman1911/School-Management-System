namespace SchoolManagementSystem.ViewModel
{
    public class ParentPerformanceViewModel
    {
        public string ParentFullName { get; set; }
        public string ParentFirstLetter { get; set; }
        public List<PerformanceChildInfo> Children { get; set; } = new List<PerformanceChildInfo>();
    }

    public class PerformanceChildInfo
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string GradeName { get; set; }

        // KPI Cards
        public decimal OverallAverage { get; set; }       
        public decimal AttendanceRate { get; set; }         
        public decimal HomeworkSubmissionRate { get; set; } 
        public int UpcomingExamsCount { get; set; }          

        // Recent Stages Table
        public List<RecentGradeInfo> RecentGrades { get; set; } = new List<RecentGradeInfo>();
    }

    public class RecentGradeInfo
    {
        public string SubjectName { get; set; }   
        public string ExamName { get; set; }      
        public string Score { get; set; }         
        public string Date { get; set; }          
    }
}
