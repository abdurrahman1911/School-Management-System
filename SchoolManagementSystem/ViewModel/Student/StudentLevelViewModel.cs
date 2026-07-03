namespace SchoolManagementSystem.ViewModel.Student
{
    public class StudentLevelViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        
        public List<string> Subjects { get; set; }
        public List<decimal> ThisMonthAverages { get; set; }
        public List<decimal> LastMonthAverages { get; set; }
        public List<decimal> SemesterAverages { get; set; }
        public List<decimal> YearAverages { get; set; }
        
        public decimal OverallAverage { get; set; }
        public string TopSubject { get; set; }
    }
}
