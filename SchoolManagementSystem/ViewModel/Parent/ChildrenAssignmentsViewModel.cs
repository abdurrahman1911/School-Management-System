using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModel.Parent
{
    public class ChildrenAssignmentsViewModel
    {
        public int ParentId { get; set; }

        public string ParentFullName { get; set; }

        public List<ChildAssignmentsInfo> ChildrenAssignments { get; set; }
    }

    public class ChildAssignmentsInfo
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }

        public List<AssignmentInfo> Assignments { get; set; }
    }

    public class AssignmentInfo
    {
        public int HomeworkId { get; set; }
        public string Title { get; set; }
        public string subjectName { get; set; }
        public bool IsSolved { get; set; }
        public DateTime SolutionDate { get; set; }
        public DateTime LastDate { get; set; }
        public string TeacherName { get; set; }
    }
}
