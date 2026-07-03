using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class TeacherExamsDashboardViewModel
    {
        public NavigationViewModel NavigationInfo { get; set; }
        public List<IdNameViewModel>Levels { get; set; }
        public List<IdNameViewModel>Classes { get; set; }
        public List<IdNameViewModel> Subjects { get; set; }
        public List<IdNameViewModel> Stages { get; set; }
        public int totalExams { get; set; }
        public int upcomingExams { get; set; }
        public int finishedExams { get; set; }
        public int ongoingExams { get; set; }


        public List<ExamViewModel> Exams { get; set; }

        public TeacherExamsDashboardViewModel()
        {
            Exams = new List<ExamViewModel>();
        }
    }
    public class ExamViewModel
    {
        public int Id { get; set; }
        public string ExamName { get; set; }
        public string Subject { get; set; }
        public string ExamType { get; set; }

        public DateOnly ExamDate { get; set; }
        public TimeOnly ExamTime { get; set; }

        public int Duration { get; set; }
        public int QuestionsCount { get; set; }
        public decimal TotalScore { get; set; }
        public string Status { get; set; }

    }
}