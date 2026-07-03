namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITHeadmastersManagementViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<HeadmasterInfo> HeadmasterInfo { get; set; }

    }

    public class HeadmasterInfo
    {
        public int ID { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? ExitDate { get; set; }

    }

}
