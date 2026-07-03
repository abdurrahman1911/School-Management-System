namespace SchoolManagementSystem.Models
{
   
    public class UserUserType
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public Byte UserTypeId { get; set; }
        //Many-To-One
        public User User { get; set; }
        public UserType UserType { get; set; }

        
    }

}
