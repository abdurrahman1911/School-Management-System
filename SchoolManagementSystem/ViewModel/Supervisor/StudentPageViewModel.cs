namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class StudentPageViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; } = new();
        public int? SelectedStageId { get; set; }
        public int? SelectedLevelId { get; set; }

        public List<SupervisorStudentViewModel> Students { get; set; } = new();

        
        public List<StageViewModel> Stages { get; set; } = new();
        public List<ClassViewModel> Classes { get; set; } = new();
    }

}