namespace SchoolManagementSystem.ViewModel.SchoolManager
{
    public class SchoolManagerDashboard
    {

        public int StudentsCount { get; set; }
        public int TeachersCount { get; set; }
        public int SupervisorsCount { get; set; }

        public List<string> StudentChartLabels { get; set; } = new();
        public List<int> StudentChartData { get; set; } = new();

        public List<string> TeacherChartLabels { get; set; } = new();
        public List<int> TeacherChartData { get; set; } = new();

        public List<string> SupervisorChartLabels { get; set; } = new();
        public List<int> SupervisorChartData { get; set; } = new();
    }
}
