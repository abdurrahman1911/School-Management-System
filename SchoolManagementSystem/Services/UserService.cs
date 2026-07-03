using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.ITManager;
using SchoolManagementSystem.ViewModel.Owner;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SchoolManagementSystem.Services
{
    
    public enum UserTypeEnum
    {
        Student = 1, Teacher, Parent, Owner, Supervisor, Headmaster, Admin,IT
    }
    public class UserService
    {
        readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public bool isEmailExist(string Email)
        {
            var checkEmailExist = _context.Users.FirstOrDefault(u => u.Email == Email);
            return (checkEmailExist != null);
        }

        public async Task<UserInfo> GetUserInfoForAddingNewRole(string SSN)
        {
            var UserInfo = new UserInfo();

            try
            {

                UserInfo = await _context.Users
                    .Where(u => u.SSN == SSN)
                    .Select(u => new UserInfo {
                        UserID = u.ID,
                        FirstName = u.FirstName,
                        SecondName = u.SecondName,
                        ThirdName = u.ThirdName,
                        LastName = u.LastName,
                        Phone = u.Phone,
                        Email = u.Email,
                        ProfilePhotoUrl = u.ProfilPhotoURL,
                        SSN = SSN,
                        BirthDate = u.BirthDate,
                        Governorate = u.Governorate,
                        City = u.City,
                        Street = u.Street,
                        Area = u.Area,
                        Gender = u.Gender,
                        Nationality = u.Nationality


                    }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            if (UserInfo == null)
            { UserInfo = new UserInfo { 
                SSN = SSN,
                BirthDate= DateTime.Now,
                Gender=true
                
            }; }

            return UserInfo;
        }
        public async Task<UserDataInfo> GetUserDataInfoForPresentationAsync(int userId)
        {
            var userData = await _context.Users
                .Where(u => u.ID == userId)
                .Select(u => new UserDataInfo
                {
                    ID = u.ID,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Email = u.Email,
                    ProfilPhotoURL = u.ProfilPhotoURL,
                    BirthDate = u.BirthDate,
                    AddedDate = u.AddedDate,
                    Governorate = u.Governorate,
                    City = u.City,
                    Street = u.Street,
                    Area = u.Area,
                    Gender = u.Gender,
                    Nationality = u.Nationality
                })
                .FirstOrDefaultAsync();


            return userData;
        }
        public bool isSSNExist(string SSN)
        {
            var checkSSNExist = _context.Users.FirstOrDefault(u => u.SSN == SSN);
            return (checkSSNExist != null);
        }

        public bool isPhoneExist(string Phone)
        {
            var checkPhoneExist = _context.Users.FirstOrDefault(u => u.Phone == Phone);
            return (checkPhoneExist != null);
        }

        public NavigationViewModel GetNavigationData(int userId)
        {   
            var user = _context.Users.Find(userId);
            if (user == null) return null;

            return new NavigationViewModel
            {
                FullName = user.FullName,
                ProfilePhotoUrl = user.ProfilPhotoURL
            };
        }

        public async Task<UserChangePasswordData> GetUserDataForChangePasswordAsync(string UserSSN)
        {
            return await _context.Users
                 .Where(u => u.SSN == UserSSN)
                 .Select(u => new ViewModel.ITManager.UserChangePasswordData
                 {
                     SSN=u.SSN,
                     Name=u.FullName,
                     Address=u.Governorate+" - "+u.City+" - "+ u.Area+ " - "+ u.Street,
                     Phone=u.Phone,
                     BirthDate=u.BirthDate,
                     PhotoURL=u.ProfilPhotoURL

                 })
             .FirstOrDefaultAsync();
        }
        public int AddBaseUser(BaseUserViewModel model, byte typeId)
        {


            if (isEmailExist(model.Email))
                throw new Exception("Email already exists.");



            if (isSSNExist(model.SSN))
                throw new Exception("SSN already exists.");


            if (isPhoneExist(model.Phone))
                throw new Exception("Phone already exists");

            string hashedPassword = clsBCrypt.GetHash(model.Password);


            var user = new User
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                LastName = model.LastName,
                Phone = model.Phone,
                Email = model.Email,
                Password = hashedPassword,
                ProfilPhotoURL = model.ProfilePhotoURL,
                SSN = model.SSN,
                BirthDate = model.BirthDate,
                AddedDate = model.AddedDate,
                Governorate = model.Governorate,
                City = model.City,
                Street = model.Street,
                Area = model.Area,
                Gender = model.Gender,
                Nationality = model.Nationality
            };




            //  add to database
            _context.Add(user);
            _context.SaveChanges();


            // Return generated User ID

            return user.ID;
        }
        public async Task<bool> Setting(User user, string currentPassword, string newPassword)
        {
            try
            {
                if (!clsBCrypt.VerifyPassword(currentPassword, user.Password))
                    return false;

                user.Password = clsBCrypt.GetHash(newPassword);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public List<Absence> GetUserAbsencesForLastTerm(int userId)
        {
            //Get the last term based on the most recent start date. This assumes terms are defined with StartDate and EndDate. where EndDate can be null.
            //so StartDate is better to determine the latest term.
            var lastTerm = _context.AcademicTerms
                .OrderByDescending(t => t.StartDate)
                .FirstOrDefault();

            if (lastTerm == null)
            {
                return new List<Absence>();
            }

            // A clean, lightweight query with no JOINs
            var absences = _context.Absences
                .Where(a => a.UserId == userId
                         && a.AbsenceDate >= lastTerm.StartDate
                         && a.AbsenceDate <= lastTerm.EndDate)
                .OrderByDescending(a => a.AbsenceDate)
                .ToList();

            return absences;
        }

        public int GetUserAbsencesCountForLastTerm(int userId)
        {
            //Get the last term based on the most recent start date. This assumes terms are defined with StartDate and EndDate. where EndDate can be null.
            //so StartDate is better to determine the latest term.
            var lastTerm = _context.AcademicTerms
                .OrderByDescending(t => t.StartDate)
                .FirstOrDefault();

            if (lastTerm == null)
            {
                return 0;
            }

            // A clean, lightweight query with no JOINs
            var absencesCount = _context.Absences
                .Where(a => a.UserId == userId
                         && a.AbsenceDate >= lastTerm.StartDate
                        ).Count();



            return absencesCount;
        }

        public string GetUserFullName(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return string.Empty;
            return user.FullName;


        }

        public int GetStudentIDByUserID(int userID)
        {
            var student = _context.Students.FirstOrDefault(s => s.UserId == userID);
            if (student == null)
                return 0; // or throw an exception, depending on your error handling strategy
            return student.ID;
        }
    }
}
