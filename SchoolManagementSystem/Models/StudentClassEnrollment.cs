namespace SchoolManagementSystem.Models
{
    public class StudentClassEnrollment
    {
        public int ID { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int LevelID { get; set; }
        public int AcademicTermId { get; set; }
        public bool IsPassed { get; set; }

        // Many-To-One
        public Student Student { get; set; }
        public Class Class { get; set; }
        public Level Level { get; set; }
        public AcademicTerm AcademicTerm { get; set; }
    }
    
}
