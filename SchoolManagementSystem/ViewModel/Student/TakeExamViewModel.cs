namespace SchoolManagementSystem.ViewModel.Student
{
    public class TakeExamViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }

        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string SubjectName { get; set; }
        public int DurationInMinutes { get; set; }
        public decimal TotalDegree { get; set; }
        public int QuestionCount { get; set; }

        public List<ExamQuestionInfo> Questions { get; set; }
    }

    public class ExamQuestionInfo
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public decimal Degree { get; set; }
        public int QuestionNumber { get; set; }
        public List<AnswerOption> Answers { get; set; }
    }

    public class AnswerOption
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
    }

    public class ExamResultViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        public string ExamName { get; set; }
        public string SubjectName { get; set; }
        public decimal TotalDegree { get; set; }
        public decimal StudentDegree { get; set; }
        public decimal Percentage { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
    }

    public class ExamReviewViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        public string ExamName { get; set; }
        public string SubjectName { get; set; }
        public string ExamDate { get; set; }
        public decimal TotalDegree { get; set; }
        public decimal StudentDegree { get; set; }
        public decimal Percentage { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
        public List<ReviewQuestionInfo> Questions { get; set; }
    }

    public class ReviewQuestionInfo
    {
        public int QuestionNumber { get; set; }
        public string Title { get; set; }
        public decimal Degree { get; set; }
        public bool IsCorrect { get; set; }
        public bool DidNotAnswer { get; set; }
        public List<ReviewAnswerOption> Answers { get; set; }
    }

    public class ReviewAnswerOption
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public bool IsStudentAnswer { get; set; }
    }
}
