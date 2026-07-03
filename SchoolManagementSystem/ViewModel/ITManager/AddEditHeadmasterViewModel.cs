using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AddEditHeadmasterViewModel
    {
        public NavigationViewModel? NavigationInfo { get; set; }
        public UserInfo UserInfo { get; set; }

        public string SSN { get; set; }
        public HeadmasterData HeadmasterInfo { get; set; }
        public IFormFile? ProfileImageFile { get; set; }


    }


    public class HeadmasterData
    {
        public int UserID { get; set; }

        public int ID { get; set; }
        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }

        public DateTime? ExitDate { get; set; }


    }
}
