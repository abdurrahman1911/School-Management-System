namespace SchoolManagementSystem.Models
{
    public class StudentHomeworkAnswer
    {
        public int ID { get; set; }
        public int HomeworkId { get; set; }
        public int StudentId { get; set; }
        public string Link { get; set; }
        public DateTime AssignDate { get; set; }

        // Many-To-One
        public virtual Homework Homework { get; set; }
        public virtual Student Student { get; set; }


    }
}
