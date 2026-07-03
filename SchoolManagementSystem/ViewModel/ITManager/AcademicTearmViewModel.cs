namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AcademicTearmViewModel
    {
        public List<AcdimicTermItem> AcademicTermList { get; set; } = new List<AcdimicTermItem>();
        public NavigationViewModel NavigationInfo { get; set; }= new NavigationViewModel();
    }
    public class AcdimicTermItem
    {
        public int Id { get; set; }
        public string TermName { get; set; }
        public string Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}
