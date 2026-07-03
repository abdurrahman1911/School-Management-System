namespace SchoolManagementSystem.ViewModel.Student
{
    public class StudentTestsViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        
        public int UpcomingCount { get; set; }
        public int ActiveCount { get; set; }
        public int CompletedCount { get; set; }
        public int MissedCount { get; set; }

        public List<TestInfo> Tests { get; set; }
    }

    public class TestInfo
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Status { get; set; } // active, upcoming, completed, missed
        public decimal TotalMarks { get; set; }
        public decimal? Grade { get; set; }
        public decimal? Percentage { get; set; }
        public string ExamUrl { get; set; }
        public bool CanReview { get; set; }
    }
}
