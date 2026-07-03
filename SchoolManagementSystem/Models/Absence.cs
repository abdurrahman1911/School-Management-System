namespace SchoolManagementSystem.Models
{
    public class Absence
    {
        public int ID{ get; set; }
        public int UserId { get; set; }
        public DateTime AbsenceDate { get; set; }
        public string? Reason { get; set; }

        // Many-To-One
        public virtual User User { get; set; }

    }
}
