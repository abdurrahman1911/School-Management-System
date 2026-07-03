using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModel.Owner
{
    public class AbcenseViewModel
    {

        public NavigationViewModel NavigationInfo { get; set; }
        public AbsenceInfo AbsenceInfo{ get; set; }
        
    }

    public class AbsenceInfo
    {
        public int userID { get; set; }
        public string FullName { get; set; }
        public List<Absence> Absences { get; set; }
    }
}
