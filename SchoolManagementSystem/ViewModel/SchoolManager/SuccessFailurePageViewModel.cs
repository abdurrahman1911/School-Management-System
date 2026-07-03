using System.Collections.Generic;

namespace SchoolManagementSystem.ViewModel.SchoolManager
{
    public class SuccessFailurePageViewModel
    {
        public string ManagerName { get; set; }
        public string ManagerPhotoUrl { get; set; }
        public List<StageResultViewModel> Stages { get; set; } = new List<StageResultViewModel>();
    }

    public class StageResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}