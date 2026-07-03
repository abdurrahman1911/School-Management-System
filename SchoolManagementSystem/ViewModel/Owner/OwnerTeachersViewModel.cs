namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerTeachersViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }
        public List<TeacherInfo> TeachersInfo { get; set; }
    }

    public class  TeacherInfo
    {
        public int UserID { get; set; }

        public int TeacherID { get; set; }
        public string  FullName { get; set; }

        public int AbcenseDaysCount { get; set; }

        public DateTime  JoinDate { get; set; }

        public DateTime? ExitDate { get; set; }


    }
}
