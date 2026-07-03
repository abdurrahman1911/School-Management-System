namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class AcademicYearViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }= new NavigationViewModel();
        public List<AcdimicYearItem> AcadimicYearList { get; set; }= new List<AcdimicYearItem>();
    }
    public class AcdimicYearItem
    {
        public int Id { get; set; }
       public string Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}
