namespace SchoolManagementSystem.Models
{
    public class Admin
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? ExiteDate { get; set; }
        //One-To-One
        public virtual User User { get; set; }

        //One-To-Many
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Supervisor> Supervisors { get; set; }
        public virtual ICollection<Parent> Parents { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Headmaster> Headmasters { get; set; }

    }
}
