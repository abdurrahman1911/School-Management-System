namespace SchoolManagementSystem.ViewModel.Supervisor
{
    public class AbsenceEntry
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsAbsent { get; set; }
        public string? Reason { get; set; }
    }
}
