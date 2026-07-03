namespace SchoolManagementSystem.Models
{
    public class Supervisor
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int AdminId { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? ExiteDate { get; set; }

        //One-To-One
        public virtual User User { get; set; }
        //Many-To-One
        public virtual Admin Admin { get; set; }
    }
}
