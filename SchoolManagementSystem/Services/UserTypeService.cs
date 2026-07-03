using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class UserTypeService
    {
        readonly AppDbContext _context;
        public UserTypeService(AppDbContext context)
        {
            _context = context;
        }
        public void AddUserType(int userId,byte userTypeId)
        { 
            UserUserType userType = new UserUserType
            { 
                UserId = userId,
                UserTypeId = userTypeId,
            };
            _context.Add(userType);
        }
       


    }
}
