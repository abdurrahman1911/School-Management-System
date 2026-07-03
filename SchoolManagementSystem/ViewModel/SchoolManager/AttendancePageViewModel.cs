using System.Collections.Generic;

namespace SchoolManagementSystem.ViewModel.SchoolManager
{
    public class AttendancePageViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; } = new NavigationViewModel();
        public List<StageAttendanceResultViewModel> Stages { get; set; } = new List<StageAttendanceResultViewModel>();
    }

    public class StageAttendanceResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}