namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class StudentTableViewModel
    {
       public NavigationViewModel NavigationInfo { get; set; } = new NavigationViewModel();


        public List<ClassViewModel> Classes { get; set; } = new ();

        public int? SelectedClassId { get; set; }

        public string? CurrentTablePath { get; set; }
    }
}
