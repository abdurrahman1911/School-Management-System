namespace SchoolManagementSystem.Models
{
    public class AcademicTerm
    {
        public int ID { get; set; }
        public int AcademicYearId { get; set; }
        public string Name { get; set; }
        public byte TermNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Many-To-One
        public virtual AcademicYear AcademicYear { get; set; }
        // One-To-Many
        public virtual ICollection<StudentClassEnrollment> StudentClassEnrollments { get; set; }
        public virtual ICollection<StudentsSubjectsEnrollment> StudentsSubjectsEnrollments { get; set; }


    }
}
