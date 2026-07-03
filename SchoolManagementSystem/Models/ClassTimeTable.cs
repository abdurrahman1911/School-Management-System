namespace SchoolManagementSystem.Models
{
    public class ClassTimeTable
    {
        public int ID { get; set; }
        public int ClassId { get; set; }
        public string PhotoLink { get; set; }
        // Many-To-One
        public virtual Class Class { get; set; }

    }
}
