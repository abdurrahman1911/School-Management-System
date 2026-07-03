namespace SchoolManagementSystem.Models
{
    public class TeacherTimeTable
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string PhotoLink { get; set; }

        //One-to-One 
        public virtual Teacher Teacher { get; set; }
    }
}
