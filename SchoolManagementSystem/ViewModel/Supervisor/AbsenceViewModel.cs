namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class AbsenceViewModel
    {
        public int SelectedLevelId { get; set; }
        public int SelectedGradeId { get; set; }
        public DateTime AbsenceDate { get; set; } = DateTime.Now;
        public NavigationViewModel? NavigationInfo { get; set; } = new NavigationViewModel();
        public List<AbsenceEntry>? Users { get; set; } = new List<AbsenceEntry>();
    }

   
}
