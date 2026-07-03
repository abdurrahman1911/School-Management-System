namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITManagersManagementViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<ITManagerInfo> ITManagerInfos { get; set; }

    }

    public class ITManagerInfo
    {
        public int ID { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? ExitDate { get; set; }

    }
}
