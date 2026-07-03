namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class GradesViewModel
    {
       public List<IdNameViewModel> Subjects { get; set; } = new List<IdNameViewModel>();
        public List<IdNameViewModel> Classes { get; set; } = new List<IdNameViewModel>();
        public List<IdNameViewModel> Levels { get; set; } = new List<IdNameViewModel>();
        public List<IdNameViewModel> Stages { get; set; } = new List<IdNameViewModel>();
        public decimal maxDegree { get; set; }
        public int studentCount { get; set; }
        public decimal AverageGrade { get; set; }
        public NavigationViewModel NavigationInfo { get; set; }

        public List<StudentGradeViewModel> StudentGrades { get; set; }= new List<StudentGradeViewModel>();

    }
    public class StudentGradeViewModel
    {
        public string fullName { get; set; }
        public string Subject { get; set; }
        public decimal Grade { get; set; }
        public DateTime Data { get; set; }
        public decimal ExamPercentage { get; set; }

        public string Performance
        {
            get
            {
                if (ExamPercentage >= 90)
                    return "ممتاز";
                else if (ExamPercentage >= 80)
                    return "جيد جداً";
                else if (ExamPercentage >= 70)
                    return "جيد";
                else if (ExamPercentage >= 60)
                    return "مقبول";
                else
                    return "ضعيف";
            }
        }
    }
}
