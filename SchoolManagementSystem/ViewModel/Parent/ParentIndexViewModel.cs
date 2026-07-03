namespace SchoolManagementSystem.ViewModel.Parent
{
    public class ParentIndexViewModel
    {
        public string ParentFullName { get; set; }
        public string ParentFirstLetter { get; set; }
        public List<ChildInfo> Children { get; set; } = new List<ChildInfo>();
    }

    public class ChildInfo
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string GradeName { get; set; }
        public string ClassName { get; set; }
        public string PhotoUrl { get; set; }
        public bool Gender { get; set; }
        public List<TeacherContact> Teachers { get; set; } = new List<TeacherContact>();
    }

    public class TeacherContact
    {
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public string Phone { get; set; }
    }
}
