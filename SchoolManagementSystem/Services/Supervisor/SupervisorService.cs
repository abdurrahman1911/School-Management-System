using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.ViewModel.Supervisor;
using SchoolManagementSystem.ViewModel.Teacher;
using System.Transactions;

namespace SchoolManagementSystem.Services.Superbisor
{
    public class SupervisorService
    {
        readonly AppDbContext _context;

        private readonly IWebHostEnvironment webHostEnvironment;
        public SupervisorService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<ProfileViewModel>GetProfileDataAsync(int userid)
        {
            return await _context.Users
                .Where(u => u.ID == userid)
                .Select(u => new ProfileViewModel
                {
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Email = u.Email,
                    AddedDate=u.AddedDate,
                    PhotoUrl=u.ProfilPhotoURL
                }).FirstAsync();
        }
        public async Task<List<SupervisorStudentViewModel>> GetAllStudents(int? stageId = null, int? classId = null)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => u.UserUserTypes.Any(ut => ut.UserTypeId == (byte)UserTypeEnum.Student));

            if (stageId.HasValue)
            {
                query = query.Where(u => u.Student.StudentClassEnrollments
                    .Any(sce => sce.Level.StageID == stageId.Value));
            }

            if (classId.HasValue)
            {
                query = query.Where(u => u.Student.StudentClassEnrollments
                    .Any(sce => sce.LevelID == classId.Value));
            }

            return await query.Select(u => new SupervisorStudentViewModel
            {
                FullName = u.FullName ?? "",
                ClassId = u.Student.StudentClassEnrollments.Select(sce => sce.ClassId).FirstOrDefault(),
                StageId = u.Student.StudentClassEnrollments.Select(sce => sce.Class.Level.StageID).FirstOrDefault()
            }).ToListAsync();
        }


        public async Task<List<StageViewModel>> GetStagesAsync()
        {
            return await _context.Stages
                .AsNoTracking()
                .Select(g => new StageViewModel
                {
                    Id = g.ID,
                    Name = g.Name
                }).ToListAsync();
        }

        public async Task<List<ClassViewModel>> GetClassesAsync(int? stageId)
        {
            var query = _context.Classes.AsNoTracking();

            if (stageId.HasValue)
            {
                query = query.Where(c => c.Level.StageID == stageId.Value);
            }

            return await query.Select(c => new ClassViewModel
            {
                Id = c.ID,
                Name = c.Name,
                ClassId = c.Level.StageID
            }).ToListAsync();
        }

        public async Task<List<ClassViewModel>> GetLevelAsync(int? stageId)
        {
            var query = _context.Levels.AsNoTracking();

            if (stageId.HasValue)
            {
                query = query.Where(c => c.StageID == stageId.Value);
            }

            return await query.Select(c => new ClassViewModel
            {
                Id = c.ID,
                Name = c.Name,
                ClassId = c.StageID
            }).ToListAsync();
        }


        public async Task<SupervisorTeacherViewModel> GetAllTeacherWithSubject(int userid)
        {

             var teacher= await _context.Users
                .Where(u => u.UserUserTypes.Any(ut => ut.UserTypeId == (byte)UserTypeEnum.Teacher))
                .Select(u => new TeacherItem
                {
                    ID = u.ID,
                    FullName = u.FullName,
                    Subject = u.Teacher.TeacherSubjects
                        .Select(ts => ts.Subject.Name)
                        .Distinct()
                        .ToList(),
                        
                })
                .ToListAsync();
            return new SupervisorTeacherViewModel
            {
                Teachers = teacher,
                NavigationInfo = await GetNavigationDataAsync(userid)
            };
        }

        public async Task<string> GetStudentTablePath(int classId)
        {
            var table = await _context.ClassTimeTable
                .FirstOrDefaultAsync(t => t.ClassId == classId);
            return table?.PhotoLink;
        }

        public async Task<bool> UploadOrUpdateStudentTable(int classId, IFormFile file)
        {
            if (file == null || file.Length == 0) return false;

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/tables");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var existingTable = await _context.ClassTimeTable.FirstOrDefaultAsync(t => t.ClassId == classId);

            if (existingTable != null && !string.IsNullOrEmpty(existingTable.PhotoLink))
            {
                var oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, existingTable.PhotoLink.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    try { File.Delete(oldFilePath); } catch { }
                }
            }

            var fileName = $"table_class_{classId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dbPath = "/uploads/tables/" + fileName;

            if (existingTable != null)
            {
                existingTable.PhotoLink = dbPath;
                _context.ClassTimeTable.Update(existingTable);
            }
            else
            {
                await _context.ClassTimeTable.AddAsync(new ClassTimeTable
                {
                    ClassId = classId,
                    PhotoLink = dbPath
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<IdNameViewModel>> GetAllTeachersIdAndFullName()
        {

            return await _context.Teachers
                .AsNoTracking()
                .Select(u => new IdNameViewModel
                {
                    Id = u.UserId,
                    Name = u.User.FullName
                })
                .ToListAsync();
        }

        public async Task<string> GetTeacherTablePath(int teacherId)
        {
            var table = await _context.TeacherTimeTables
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            return table?.PhotoLink;
        }

        public async Task<bool> UploadOrUpdateTeacherTable(int teacherId, IFormFile file)
        {
            if (file == null || file.Length == 0) return false;

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/tables");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var existingTable = await _context.TeacherTimeTables.FirstOrDefaultAsync(t => t.TeacherId == teacherId);

            if (existingTable != null && !string.IsNullOrEmpty(existingTable.PhotoLink))
            {
                var oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, existingTable.PhotoLink.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    try { File.Delete(oldFilePath); } catch { }
                }
            }

            var fileName = $"table_teacher_{teacherId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dbPath = "/uploads/tables/" + fileName;

            if (existingTable != null)
            {
                existingTable.PhotoLink = dbPath;
                _context.TeacherTimeTables.Update(existingTable);
            }
            else
            {
                await _context.TeacherTimeTables.AddAsync(new TeacherTimeTable
                {
                    TeacherId = teacherId,
                    PhotoLink = dbPath
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<List<Level>> GetLevelsAsync()
        => await _context.Levels.ToListAsync();

        public async Task<List<Level>> GetLevelsByStageId(int stageid)
            => await _context.Levels.Where(g => g.StageID == stageid).OrderBy(g => g.Order).ToListAsync();

        public async Task<List<AbsenceEntry>> GetStudentsByGrade(int gradeId, DateTime absenceDate)
        {
            return await _context.StudentClassEnrollments
                .Where(e => e.LevelID == gradeId)
                .Select(s => new AbsenceEntry
                {
                    UserId = s.Student.UserId,
                    UserName = s.Student.User.FullName,

                    IsAbsent = _context.Absences.Any(a => a.UserId == s.Student.UserId && a.AbsenceDate.Date == absenceDate.Date),

                    Reason = _context.Absences
                        .Where(a => a.UserId == s.Student.UserId && a.AbsenceDate.Date == absenceDate.Date)
                        .Select(a => a.Reason)
                        .FirstOrDefault()
                }).ToListAsync();
        }
        public async Task<bool> SaveAbsence(AbsenceViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
              
                var studentIdsInList = model.Users.Select(s => s.UserId).ToList();

                var existingAbsences = await _context.Absences
                    .Where(a => a.AbsenceDate.Date == model.AbsenceDate.Date &&
                                studentIdsInList.Contains(a.UserId))
                    .ToListAsync();

                if (existingAbsences.Any())
                {
                    _context.Absences.RemoveRange(existingAbsences);
                }

                var newAbsenceRecords = model.Users
                    .Where(s => s.IsAbsent)
                    .Select(s => new Absence
                    {
                        UserId = s.UserId,
                        AbsenceDate = model.AbsenceDate.Date, 
                        Reason = s.Reason ?? ""
                    }).ToList();

                if (newAbsenceRecords.Any())
                {
                    await _context.Absences.AddRangeAsync(newAbsenceRecords);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الغياب: {ex.Message}");
                return false;
            }
        }

        public async Task<AbsenceViewModel> GetAbsencesByDate(DateTime? date,int userid)
        {
            var selectedDate = date ?? DateTime.Now.Date;

            var teachers = await GetAllTeachersIdAndFullName();

            var absences = await _context.Absences
                .Where(a => a.AbsenceDate.Date == selectedDate.Date)
                .ToListAsync();

            var model = new AbsenceViewModel
            {
                AbsenceDate = selectedDate,
                Users = teachers.Select(t =>
                {
                    var existing = absences.FirstOrDefault(a => a.UserId == t.Id);

                    return new AbsenceEntry
                    {
                        UserId = t.Id,
                        UserName = t.Name,
                        IsAbsent = existing != null,
                        Reason = existing?.Reason
                    };
                }).ToList(),

                NavigationInfo = await GetNavigationDataAsync(userid)
            };
            return (model);
        }


        public async Task<bool> AddNoteAsync(int supervisoruserid, AddNotesViewModel viewModel)
        {
            try
            {
                var note = new Note
                {
                    WriterUserId = supervisoruserid,
                    TargetUserId = viewModel.TeacherId,
                    NoteDetails = viewModel.Note,
                    AddedDate = DateTime.Now,
                };
                await _context.Notes.AddAsync(note);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<NavigationViewModel?> GetNavigationDataAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.ID == userId)
                .Select(u => new NavigationViewModel
                {
                    FullName = u.FirstName + " " + u.LastName,
                    ProfilePhotoUrl = u.ProfilPhotoURL?? "default.png"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<NoteDisplayViewModel> GetNoteToSupervisorAsync(int supervisoruserid)
        {
            var navigation = await GetNavigationDataAsync(supervisoruserid);

            var notesList = await _context.Notes
                .Where(e => e.TargetUserId == supervisoruserid)
                .Select(e => new NoteItemViewModel
                {
                    WriterName = e.WriterUser.FullName,
                    AddedDate = e.AddedDate,
                    NoteDetails = e.NoteDetails
                })
                .OrderByDescending(e => e.AddedDate)
                .ToListAsync();

            return new NoteDisplayViewModel
            {
                NavigationInfo = navigation,
                Notes = notesList
            };
        }

    }
}
