namespace SchoolManagementSystem.Models
{
    public class StudentsSubjectsEnrollment
    {
        public int ID { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int AcademicTermId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public bool IsPassed { get; set; }
        
        // Many-To-One
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get;   set; }
        public virtual AcademicTerm AcademicTerm { get; set; }




    }
}
