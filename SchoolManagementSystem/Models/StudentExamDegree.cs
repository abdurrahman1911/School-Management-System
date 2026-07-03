namespace SchoolManagementSystem.Models
{
    public class StudentExamDegree
    {
        public int ID { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public decimal Degree { get; set; }

        // Many-To-One
        public MultiChoiceExam Exam { get; set; }
        public Student Student { get; set; }

        // One-To-Many
        public virtual ICollection<StudentExamAnswers> StudentExamAnswers { get; set; }

    }
}
