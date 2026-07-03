namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class AttendanceViewModel
    {
        public int TotalStudents { get; set; }
        public int StudentsWithNotesCount { get; set; }
        public int TodayNotesCount { get; set; }
        public int TotalNotesCount { get; set; }

        public List<StudentDetail> Students { get; set; } = new List<StudentDetail>();

       
    }
    public class StudentDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Grade { get; set; }
        public string LastUpdateDate { get; set; }
        public string CurrentNote { get; set; }
    }
}
