namespace SchoolManagementSystem.ViewModel.Student
{
    public class StudentAssignmentsViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        
        public int CurrentCount { get; set; }
        public int CompletedCount { get; set; }
        public int LateCount { get; set; }
        public decimal CompletionRate { get; set; }

        public List<AssignmentInfo> Assignments { get; set; }
    }

    public class AssignmentInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; } // current, completed, late
        public string HomeworkLink { get; set; }
        public string SubmissionLink { get; set; }
        public string SubmissionDate { get; set; }
    }
}
