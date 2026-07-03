namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class SupervisorTeacherViewModel
    {
      public List<TeacherItem> Teachers { get; set; }=new List<TeacherItem>();
        public NavigationViewModel NavigationInfo { get; set; } = new NavigationViewModel();
    }

    public class TeacherItem
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public List<string> Subject { get; set; } = new();

        public string SubjectsDisplay => string.Join("، ", Subject);
    }
}
