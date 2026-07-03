namespace SchoolManagementSystem.ViewModel.Student
{
    public class StudentAttendanceViewModel
    {
        public string StudentFullName { get; set; }
        public string FirstLetter { get; set; }
        
        public int AbsenceDays { get; set; }

        public List<AbsenceRecordInfo> Absences { get; set; }
    }

    public class AbsenceRecordInfo
    {
        public string Date { get; set; }
        public string DayName { get; set; }
        public string Reason { get; set; }
    }
}
