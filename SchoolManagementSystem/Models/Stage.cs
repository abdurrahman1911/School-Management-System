namespace SchoolManagementSystem.Models
{
    public class Stage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte Order { get; set; }

        // One-To-Many
        public virtual ICollection<Level> Levels { get; set; }
    }
}
