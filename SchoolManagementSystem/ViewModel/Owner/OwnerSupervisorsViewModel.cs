using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.ViewModel.Owner
{
    public class OwnerSupervisorsViewModel
    {
        public NavigationViewModel NavigationData { get; set; }

        public List<SupervisorInfo> SchoolSupervisors { get; set; }

    }

    public class SupervisorInfo
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        public string FullName { get; set; }
        public decimal PerformanceRating { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? ExiteDate { get; set; }
        
        public int AbcenseCount { get; set; }
    }
}
