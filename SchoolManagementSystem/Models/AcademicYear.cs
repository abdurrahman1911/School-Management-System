namespace SchoolManagementSystem.Models
{
    public class AcademicYear
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // One-To-Many
        public virtual ICollection<AcademicTerm> AcademicTerms { get; set; }

    }
}
