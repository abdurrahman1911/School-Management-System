namespace SchoolManagementSystem.Models
{
    public class UserType
    {
        public Byte ID { get; set; }
        public string TypeName { get; set; }

        //One-To-Many
        public ICollection<UserUserType> UserUserTypes { get; set; }

    }
}
