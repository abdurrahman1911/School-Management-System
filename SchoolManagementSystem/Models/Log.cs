namespace SchoolManagementSystem.Models
{
    public class Log
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; }
        public string? ActionDetails { get; set; }
        public DateTime LogDate { get; set; }

        public virtual User User { get; set; }
    }
}
