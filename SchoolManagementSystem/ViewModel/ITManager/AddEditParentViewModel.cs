namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AddEditParentViewModel
    {

        public NavigationViewModel? NavigationInfo { get; set; }
        public UserInfo UserInfo { get; set; }

        public string SSN { get; set; }
        public ParentData ParentInfo { get; set; }
        public IFormFile? ProfileImageFile { get; set; }


    }


    public class ParentData
    {
        public int ParentUserID { get; set; }

        public int ParentID { get; set; }


    }

    
}
