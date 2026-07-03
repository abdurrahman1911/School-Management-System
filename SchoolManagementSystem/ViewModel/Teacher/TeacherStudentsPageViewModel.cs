using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class TeacherStudentsPageViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; } = new();
        public List<TeacherStudentViewModel> Students { get; set; } = new();

        public int? SelectedLevelsId { get; set; }
        public int? SelectedClassId { get; set; }
        public int? SelectedStageId { get; set; }


        public List<IdNameViewModel> Stages { get; set; } = new();
        public List<IdNameViewModel> Levels { get; set; } = new();
        public List<IdNameViewModel> Classes { get; set; } = new();
    }

    public class TeacherStudentViewModel
    {

        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string ParentName { get; set; }
        public string StudentPhone { get; set; }
        public string ParentPhone { get; set; }
        public int AbsencesCount { get; set; }

        public string ExamPercentage { get; set; }
        public decimal HomworkPerecentage { get; set; }
        public decimal TotalPercentage { get; set; } 

        public string Performance { get; set; }
       

    }
}