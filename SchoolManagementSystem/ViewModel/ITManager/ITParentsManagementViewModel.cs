namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITParentsManagementViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<ParentInfo> parents { get; set; }

    }

    public class ParentInfo
    {
        public int ID { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }

        public int SonsNumber { get; set; }


    }
}
