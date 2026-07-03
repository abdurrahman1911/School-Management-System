using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using System.Transactions;

namespace SchoolManagementSystem.Services
{
     public class HeadmasterService
    {
        readonly AppDbContext _context;
        readonly UserService _userService;
        readonly UserTypeService _userTypeService;
        public HeadmasterService(AppDbContext context, UserService userService, UserTypeService userTypeService)
        {
            _context = context;
            _userService = userService;
            _userTypeService = userTypeService;
        }
       

         private int AddHeadmaster(HeadmasterViewModel model, int userID)
        {
            Headmaster headmaster = new Headmaster
            {
                UserId = userID,
                HireDate = model.HireDate,
                ExiteDate = model.ExiteDate,
            };




            _context.Add(headmaster);
            _context.SaveChanges();


            return headmaster.ID;

        }
         public bool AddNewHeadmaster(HeadmasterViewModel model)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    int userID = _userService.AddBaseUser(model, (byte)UserTypeEnum.Headmaster);

                    AddHeadmaster(model, userID);
                    _userTypeService.AddUserType(userID, (byte)UserTypeEnum.Headmaster);
                    _context.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                return false;
            }



            return true;

        }
    }
}
