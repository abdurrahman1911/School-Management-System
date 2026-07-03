namespace SchoolManagementSystem.Models
{
    public class Owner
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        //One-To-One
        public virtual User User { get; set; }
    }
}


