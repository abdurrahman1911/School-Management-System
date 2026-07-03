namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITSupervisorsManagementViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<SupervisorInfo> Supervisors { get; set; }

    }

    public class SupervisorInfo
    {
        public int ID { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
       
        public DateTime HireDate { get; set; }

        public DateTime? ExitDate { get; set; }

    }
}
