using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models;
using System.Collections.Generic;

namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class ClassViewModel
    {
        public int ID { get; set; }
        public string StageName { get; set; }
        public string LevelName { get; set; }
        public string ClassName { get; set; }
    }

    public class AddEditClassViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "يرجى اختيار المرحلة")]
        public int StageID { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الصف")]
        public int LevelID { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الفصل")]
        public string ClassName { get; set; }

        public List<StageInSchool>? Stages { get; set; }
    }
}
