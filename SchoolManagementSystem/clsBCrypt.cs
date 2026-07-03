namespace SchoolManagementSystem
{
    static public class clsBCrypt
    {
        static public string  GetHash(string Password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(Password, 13); ;
        }

        static  public bool VerifyPassword(string Password,string HashValue)
        {
            return (BCrypt.Net.BCrypt.EnhancedVerify(Password, HashValue));
        }
    }
}
