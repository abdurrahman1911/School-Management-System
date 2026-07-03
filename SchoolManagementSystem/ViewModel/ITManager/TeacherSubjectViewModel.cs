namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class TeacherSubjectViewModel
    {
        public int Id { get; set; }
        
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? LevelName { get; set; }
        public string? StageName { get; set; }
    }
}
