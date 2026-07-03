namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ITTeachersManagementViewModel
    {
       public NavigationViewModel NavigationInfo {  get; set; }

       public List<TeacherInfo> Teachers { get; set; }

    }

    public class TeacherInfo
    {
        public int TeacherID { get; set; }

        public string TeacherName { get; set; }

        public string TeacherSSN { get; set; }

        public string TeacherPhone {  get; set; }

        public DateTime HireDate { get; set; }


        
    }

    
}
