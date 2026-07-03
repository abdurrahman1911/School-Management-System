using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagementSystem.ViewModel.Teacher;

namespace SchoolManagementSystem.ViewModel.ITManager
{
  
    public class ManageSuccessViewModel
    {
        public NavigationViewModel NavigationInfo {  get; set; }
        public int SelectedStageId { get; set; }
        public int SelectedLevelId { get; set; }
        public int SelectedClassId { get; set; }
        public int CurrentAcademicTermId { get; set; }
        public int NextAcademicTermId { get; set; }
        public List<TermData> termsData { get; set; }
        public List<ClassStudentsData> classStudentsData { get; set; }
    }

    public class ClassStudentsData
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool IsSuccess { get; set; }
    }

}
