namespace SchoolManagementSystem.Models
{
    public class Note
    {
        public int ID { get; set; }
        public int WriterUserId { get; set; }
        public int TargetUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string NoteDetails { get; set; }

        public User WriterUser { get; set; }   
        public User TargetUser { get; set; }

    }
}
