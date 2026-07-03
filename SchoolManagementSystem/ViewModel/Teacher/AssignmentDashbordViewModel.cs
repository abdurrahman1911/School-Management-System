using System;
using System.Collections.Generic;

namespace SchoolManagementSystem.ViewModel.Teacher
{
    public class AssignmentDashbordViewModel
    {
        public List<IdNameViewModel> Levels { get; set; }
        public List<IdNameViewModel> Classes { get; set; }
        public List<IdNameViewModel> Subjects { get; set; }
        public List<IdNameViewModel> Stages { get; set; }
        public NavigationViewModel NavigationInfo { get; set; }
        public int totalAssignments { get; set; }
        public int completedAssignments { get; set; }
        public int lateAssignments { get; set; }

        public List<AssignmentViewModel> Assignments { get; set; }

        public AssignmentDashbordViewModel()
        {
            Assignments = new List<AssignmentViewModel>();
            Levels = new List<IdNameViewModel>();
            Classes = new List<IdNameViewModel>();
            Subjects = new List<IdNameViewModel>();
            Stages = new List<IdNameViewModel>();
        }
    }

    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public int? subjectId { get; set; }
        public int? classId { get; set; }
        public int? levelId { get; set; } 
        public int? stageId { get; set; } 

        public string Name { get; set; }
        public string Subject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FileName { get; set; }

        public string Status
        {
            get
            {
                var today = DateTime.Today;
                if (today < StartDate.Date) return "لم يبدأ بعد";
                if (today > EndDate.Date) return "منتهي";
                return "جاري";
            }
        }
    }
}