namespace SchoolManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("StudentExamAnswers")]
    public class StudentExamAnswers
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public int StudentAnswersID { get; set; }
        public int StudentExamDegreeID { get; set; }


        // Many-To-One
        public virtual Question Question { get; set; }
        public virtual QuestionAnswer QuestionAnswer { get; set; }
        public virtual StudentExamDegree StudentExamDegree { get; set; }
    }
}
