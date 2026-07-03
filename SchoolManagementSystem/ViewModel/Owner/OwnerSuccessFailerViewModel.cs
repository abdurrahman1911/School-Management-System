namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerSuccessFailerViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }

        public List<AcademicTermsFilter> termsFiltersInfo { get; set; }

        public List<StageSuccessFailerInfo> StagesSuccessFailerInfos { get; set; }
    }

    public class StageSuccessFailerInfo
    {

        public int StageID {  get; set; }

        public string StageName { get; set; }
        public List<LevelSuccessFailerInfo> LevelsSuccessFailerInfos { get; set; }

    }

    public class LevelSuccessFailerInfo
    {
        public int LevelID { get; set; }

        public string LevelName { get; set; }
        public int TotalStudents { get; set; }

        public int PassedStudentsNumber {  get; set; }

        public int FailedStudentsNumber { get; set; }

        public decimal SuccessPercentage { get; set; }

        public decimal FailurePercentage { get; set; }


    }


    public class AcademicTermsFilter
    {
        public int ID { get; set; }

        public string TermName { get; set; }

        public string YearName { get; set; }


    }
}
