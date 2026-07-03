namespace SchoolManagementSystem.ViewModel.ITManager
{
    public class TargetEnrollementData
    {
        public NavigationViewModel NavigationInfo { get; set; }
        public List<StageInSchool>? Stages { get; set; }

        public List<TermData> termsData { get; set; }



    }

    public class TermData
    {
        public int TermID {  get; set; }

        public string TermName { get; set; }

        public string YearName { get; set; }
        public int AcademicYearID {  get; set; }
    }
}
