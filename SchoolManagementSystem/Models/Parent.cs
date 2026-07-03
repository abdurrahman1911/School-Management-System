namespace SchoolManagementSystem.Models
{
    public class Parent
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int AdminId { get; set; }

        //One-To-Many
        public virtual ICollection<Student>? Students { get; set; }
        //One-To-One
        public virtual User User { get; set; }
        // Many-To-One
        public virtual Admin Admin { get; set; }
    }
}
