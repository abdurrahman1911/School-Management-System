using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel.Owner;
using System;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Services
{
    public class LogService
    {
        readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateLogAsync(int actorUserId, string actionType, string? details)
        {
            try
            {
                var newLog = new Log
                {
                    UserId = actorUserId,
                    ActionType = actionType,
                    ActionDetails = details,
                    LogDate = DateTime.UtcNow
                };

                await _context.Logs.AddAsync(newLog);

                var saved = await _context.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception)
            {
              
                return false;
            }
        }

        public async Task<(List<LogInfo> Logs, int CurrentPage, int TotalPages)> GetPagedLogsAsync(int page, int pageSize)
        {
            int totalLogs = await _context.Logs.CountAsync();
            int totalPages = (int)Math.Ceiling(totalLogs / (double)pageSize);

          
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

           
            var fetchedLogs = await _context.Logs
                .Include(l => l.User)
                .OrderByDescending(l => l.LogDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new
                {
                    l.User.FirstName,
                    l.User.SecondName,
                    l.User.ThirdName,
                    l.User.LastName,
                    l.ActionType,
                    l.ActionDetails,
                    l.LogDate
                })
                .ToListAsync();

      
            var logInfos = fetchedLogs.Select(l =>
            {
                var nameParts = new[] { l.FirstName, l.SecondName, l.ThirdName, l.LastName };
                string actorFullName = string.Join(" ", nameParts.Where(n => !string.IsNullOrWhiteSpace(n)));

                return new LogInfo
                {
                    ActorName = actorFullName,
                    Action = l.ActionType,
                    Details = l.ActionDetails,
                    LogDate = l.LogDate
                };
            }).ToList();

           
            return (logInfos, page, totalPages);
        }
    }
}