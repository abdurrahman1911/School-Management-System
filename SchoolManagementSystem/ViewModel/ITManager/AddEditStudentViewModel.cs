using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AddEditStudentViewModel
    {
        public NavigationViewModel? NavigationInfo { get; set; }

        public UserInfo UserInfo { get; set; }
        public StudentData StudentDataInfo { get; set; }

        public string? SSN { get; set; }

        public string? ParentSSN { get; set; }

        public StudentEnrollmentInfo StudentEnrollmentInfo { get; set; }

        public List<StageInSchool>? Stages { get; set; }

        public IFormFile? ProfileImageFile { get; set; }


    }

    

    public class StudentData
    {
        public int StudentUserID { get; set; }

        public int StudentID { get; set; }

        [Required(ErrorMessage = "الرقم القومي لولي الأمر مطلوب")]
        public string ParentSSN { get; set; }

        public int ParentID { get; set; }


        [Required(ErrorMessage = "صلة ولي الأمر بالطالب مطلوبة")]
        public string ParentRelation { get; set; }


        [Required(ErrorMessage = "تاريخ الالتحاق مطلوب")]
        public DateTime JoinDate { get; set; }

        public DateTime? ExitDate {  get; set; }

        public bool isGraduated { get; set; }

    }


    public class StudentEnrollmentInfo
    {
        [Required(ErrorMessage = "المرحلة الدراسية مطلوبة")]
        public int StageID { get; set; }


        [Required(ErrorMessage = "المستوى الدراسي مطلوب")]
        public int LevelID { get; set; }


        [Required(ErrorMessage = "الفصل الدراسي مطلوب")]
        public int ClassID { get; set; }
    }
    public class StageInSchool
    {
        public Stage Stage { get; set; }

        public List<LevelInStage> Levels { get; set; }

    }
   
    public class LevelInStage
    {
        public Level Level { get; set; }

        public List<Class> Classes { get; set; }

    }

   
}
