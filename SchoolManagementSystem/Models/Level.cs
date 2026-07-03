namespace SchoolManagementSystem.Models
{
    public class Level
    {
        public int ID { get; set; }
        public int StageID { get; set; }
        public string Name { get; set; }
        public byte Order { get; set; }

        // One-To-Many
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<StudentClassEnrollment> StudentClassEnrollments { get; set; }


        //Many-To-One
        public virtual Stage Stage { get; set; }


    }
}
