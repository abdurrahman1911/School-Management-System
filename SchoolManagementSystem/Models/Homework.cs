namespace SchoolManagementSystem.Models
{
    public class Homework
    {
        public int ID { get; set; }
        public int ClassId { get; set; }
        //I added the title and subjectId to make it easier to diplay this data according to the UI/UX design.
        public string Title { get; set; }
        public int SubjectID { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastDate { get; set; }
        public string Link { get; set; }
        public int TeacherId { get; set; }
        // Many-To-One
        public virtual Teacher Teacher { get; set; }
        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
        // One-To-Many
        public virtual ICollection<StudentHomeworkAnswer> StudentHomeworkAnswers { get; set; }


    }
}