using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel
{
    public class StudentViewModel:BaseUserViewModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int ParentID { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        public string ParentRelation { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime JoinDate { get; set; }
        public DateTime? ExiteDate { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public bool isGraduated {  get; set; }
    }
}
