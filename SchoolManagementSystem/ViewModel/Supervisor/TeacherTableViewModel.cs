using SchoolManagementSystem.ViewModel.Teacher;

namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class TeacherTableViewModel
    {
        public List<IdNameViewModel> Teachers { get; set; } = new();

        public NavigationViewModel NavigationInfo { get; set; } = new NavigationViewModel();


    }
}
