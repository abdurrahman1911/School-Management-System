using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class UserInfo
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; }

        public string? SecondName { get; set; }

        public string? ThirdName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string Phone { get; set; }

        public string? Email { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        public string Governorate { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        public string City { get; set; }

        public string? Street { get; set; }

        public string? Area { get; set; }

        [Required(ErrorMessage = "النوع مطلوب")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "الجنسية مطلوبة")]
        public string Nationality { get; set; }
    }
}
