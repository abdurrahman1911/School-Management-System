namespace SchoolManagementSystem.Models
{
    
    public class ExtraSubjectMaterial
    {
        public int ID { get; set; }
        public int SubjectID { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string? MaterialType { get; set; }
        public string MaterialLink { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ClassID { get; set; }

        //Many-To-One
        public Subject Subject { get; set; }
        public virtual Class Class{ get; set; }
        public Teacher Teacher { get; set; }

    }
}
