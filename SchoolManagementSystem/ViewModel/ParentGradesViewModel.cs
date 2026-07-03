namespace SchoolManagementSystem.ViewModel
{
    public class ParentGradesViewModel
    {
        public string ParentFullName { get; set; }
        public string ParentFirstLetter { get; set; }
        public List<GradeChildInfo> Children { get; set; } = new List<GradeChildInfo>();
    }

    public class GradeChildInfo
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string GradeName { get; set; }
        public List<GradeDetailInfo> AllGrades { get; set; } = new List<GradeDetailInfo>();
    }

    public class GradeDetailInfo
    {
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public string Score { get; set; }
        public string Date { get; set; }
    }
}
