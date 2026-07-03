using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.ViewModel.Owner
{
    public class UserDataInfo
    {
        public int ID { get; set; }
        [DisplayName("الاسم")]
        public string FullName { get; set; }
        [DisplayName("رقم الهاتف")]
        public string Phone { get; set; }
        [DisplayName("البريد الالكتروني")]
        public string? Email { get; set; }
        [DisplayName("الصوره الشخصية")]

        public string? ProfilPhotoURL { get; set; }
        [DisplayName("تاريخ الميلاد")]

        public DateTime BirthDate { get; set; }
        [DisplayName("تاريخ الانضمام")]

        public DateTime AddedDate { get; set; }
        [DisplayName("المحافظة")]

        public string Governorate { get; set; }
        [DisplayName("المدينة")]

        public string City { get; set; }
        [DisplayName("الشارع")]

        public string? Street { get; set; }
        [DisplayName("المنطقة")]

        public string? Area { get; set; }
        [DisplayName("النوع")]

        public bool Gender { get; set; }
        [DisplayName("الجنسية")]

        public string Nationality { get; set; }
    }
}
