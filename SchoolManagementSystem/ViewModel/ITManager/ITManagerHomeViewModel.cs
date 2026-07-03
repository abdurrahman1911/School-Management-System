namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITManagerHomeViewModel
    {
       public NavigationViewModel NavigationInfo {  get; set; }

        public LoginITManagerInfo LoginITManagerInfo { get; set; }


        public SomeSchoolInfo someSchoolInfo { get; set; }
    }

    public class LoginITManagerInfo
    {
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime JoinDate { get; set; }




    }

    public class SomeSchoolInfo
    {
        public int StudentsNum { get; set; }

        public int TeachersNum { get; set; }

        public int ClassesNum { get; set; }

        public int SubjectsNum {  get; set; }

    }
}
