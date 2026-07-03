namespace SchoolManagementSystem.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Degree { get; set; }
        public int ExamId { get; set; }
        // Many-To-One
        public virtual MultiChoiceExam MultiChoiceExam { get; set; }
        // One-To-Many
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<StudentExamAnswers> StudentExamAnswers { get; set; }
    }
}
