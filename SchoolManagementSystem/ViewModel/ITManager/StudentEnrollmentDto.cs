namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class StudentEnrollmentDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Stage { get; set; }
        public string Level { get; set; }
        public string Class { get; set; }
        public string Term { get; set; }
        public List<SubjectCheckboxDto> AvailableSubjects { get; set; } = new List<SubjectCheckboxDto>();
    }

    public class SubjectCheckboxDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
