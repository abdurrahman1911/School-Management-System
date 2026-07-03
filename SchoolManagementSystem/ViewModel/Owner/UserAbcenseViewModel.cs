using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModel.Owner
{
    public class UserAbcenseViewModel
    {
   

            public NavigationViewModel NavigationInfo { get; set; }

            public string UserFullName { get; set; }
            public List<Absence> Absences { get; set; }

        
    }
}
