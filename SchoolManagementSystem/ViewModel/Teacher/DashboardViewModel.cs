namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class DashboardViewModel
    {
        public string? Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public List<string> Subjects { get; set; }
       public NavigationViewModel NavigationInfo { get; set; }= new NavigationViewModel();
    }
}

