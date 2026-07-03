namespace SchoolManagementSystem.Models
{
    public class QuestionAnswer
    {
        public int ID { get; set; }
        public string AnswerValue { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        // Many-To-One
        public virtual Question Question { get; set; }

        // One-To-Many
        public virtual ICollection<StudentExamAnswers> StudentExamAnswers { get; set; }
    }
}
