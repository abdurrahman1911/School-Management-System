using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel
{
    public class BaseUserViewModel
    {
        // Identity
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string SecondName { get; set; }
     
        public string? ThirdName { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string LastName { get; set; }

        // Contact
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Phone { get; set; }
        
        public string? Email { get; set; }

        // Security
        public string Password { get; set; }

        // Personal info
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime BirthDate { get; set; }

        public DateTime AddedDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public bool Gender { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Nationality { get; set; }

        // Address
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Governorate { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string City { get; set; }
        public string? Street { get; set; }
        public string? Area { get; set; }

        // Optional
        public string? ProfilePhotoURL { get; set; }
    }

}
