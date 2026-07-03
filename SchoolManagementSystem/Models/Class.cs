namespace SchoolManagementSystem.Models
{
    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int LevelID { get; set; }

        //One-To-One
        public virtual ClassTimeTable ClassTimeTable { get; set; }

        // One-To-Many
        public ICollection<Homework> Homeworks { get; set; }
        public virtual ICollection<StudentClassEnrollment>StudentClassEnrollments { get; set; }
        public virtual ICollection<ExtraSubjectMaterial> ExtraSubjectMaterials { get; set; }
        public virtual ICollection<MultiChoiceExam>MultiChoiceExam { get; set; }

        // Many-To-One
        public virtual Level Level { get; set; }

        
    }
}
