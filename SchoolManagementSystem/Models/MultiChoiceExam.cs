namespace SchoolManagementSystem.Models
{
    public class MultiChoiceExam
    {
     
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ActualDate { get; set; }
        public int DurationInMinutes { get; set; }
        public string ExamType { get; set; }
        public decimal TotalDegree { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }

        // Many-To-One
        public Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Class Class { get; set; }
        // One-To-Many
        public virtual ICollection<StudentExamDegree> StudentExamDegrees { get; set; }
        public virtual ICollection<Question> Questions { get; set; }


    }
}
