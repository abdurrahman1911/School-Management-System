using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModel.Parent
{
    public class ChildrenAbsenceViewModel
    {
        public int ParentId { get; set; }

        public string ParentFullName { get; set; }
        public List<ChildAbsenceInfo> ChildrenAbsences { get; set; }
    }

    public class ChildAbsenceInfo
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public int TotalSchoolDays { get; set; }
        public int PresentDays => TotalSchoolDays - (Absences?.Count ?? 0);
        public decimal AttendanceRate => TotalSchoolDays > 0
            ? Math.Round((decimal)PresentDays / TotalSchoolDays * 100, 1)
            : 100;

        public List<Absence> Absences { get; set; }
    }
}
