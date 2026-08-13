using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using NoteProject.DTO.TodoTaskDTO;
using NoteProject.Interfaces;
using NoteProject.Models;
using System.Threading.Tasks;

namespace NoteProject.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly NoteDbContext _context;
        private readonly ILogger<TodoTaskService> _logger;

        public TodoTaskService(NoteDbContext context, ILogger<TodoTaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateTaskAsync(CreateTodoTask dto, Guid userId)
        {
            var task = new TodoTask
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                UserId = userId
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();
        }

        public bool DeleteTodoTaskAsync(Guid userId, Guid taskId)
        {
            var existingTask = _context.Tasks.FirstOrDefault(a => a.Id == taskId && a.UserId == userId);

            if (existingTask == null)
            {
                return false;
            }

            _context.Tasks.Remove(existingTask);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<UpdateTodoTask>> GetAllTasksAsync(Guid userId)
        {
            var tasks = await _context.Tasks
                .Where(a => a.UserId == userId)
                .Select(a => new UpdateTodoTask
                {
                    Title = a.Title,
                    Description = a.Description,
                    DueDate = a.DueDate,
                    IsCompleted = a.IsCompleted,
                    Priority = a.Priority
                }).ToListAsync();
            return tasks;
        }

        public async Task<UpdateTodoTask> GetTodoTaskByIdAsync(Guid userId, Guid taskId)
        {
            var existingTask = await _context.Tasks.FirstOrDefaultAsync(a => a.Id == taskId && a.UserId == userId);
            if (existingTask == null)
            {
                throw new Exception("Task not found.");
            }

            return new UpdateTodoTask
            {
                Title = existingTask.Title,
                Description = existingTask.Description,
                DueDate = existingTask.DueDate,
                IsCompleted = existingTask.IsCompleted,
                Priority = existingTask.Priority
            };
        }

        public async Task UpdateTodoTaskAsync(Guid userId, Guid taskId, UpdateTodoTask updateTodoTaskDto)
        {
            var existingTask = await _context.Tasks.FirstOrDefaultAsync(a=> a.Id == taskId && a.UserId == userId);

            _logger.LogInformation("User {UserId} is updating task {TaskId}",userId,taskId);

            if (existingTask == null)
            {
                _logger.LogWarning("Task {TaskId} was not found for user {UserId}", taskId, userId);
                throw new Exception("Task not found.");
            }

            existingTask.Title = updateTodoTaskDto.Title;
            existingTask.Description = updateTodoTaskDto.Description;
            existingTask.DueDate = updateTodoTaskDto.DueDate;
            existingTask.IsCompleted = updateTodoTaskDto.IsCompleted;
            existingTask.Priority = updateTodoTaskDto.Priority;
            existingTask.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} was updated successfully by user {UserId}", taskId, userId);
        }

        public async Task<List<UpdateTodoTask>> SearchTodoTasksAsync(Guid userId, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("Title is empty.");
            }
            var tasks = await _context.Tasks
                .Where(a => a.UserId == userId && a.Title.Contains(title))
                .ToListAsync();
            return tasks.Select(a => new UpdateTodoTask
            {
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                IsCompleted = a.IsCompleted,
                Priority = a.Priority
            }).ToList();
        }

        public async Task<List<UpdateTodoTask>> SearchTodoTasksByTimeAsync(Guid userId, DateTime? startDate, DateTime? endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start time must be less than or equal to end time.");
            }

            if (!startDate.HasValue && !endDate.HasValue)
            {
                throw new ArgumentException(
                    "At least one date must be provided.");
            }

            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
            }

            if (!startDate.HasValue && endDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }

            var tasks = await _context.Tasks
                .Where(t =>
                    t.UserId == userId &&
                    t.DueDate >= startDate &&
                    t.DueDate <= endDate)
                .ToListAsync();

            return tasks.Select(a => new UpdateTodoTask
            {
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                IsCompleted = a.IsCompleted,
                Priority = a.Priority
            }).ToList();
        }

        public async Task<List<UpdateTodoTask>> SearchTaskByStatusAsync(bool isCompleted, Guid userId)
        {
            var tasks = await _context.Tasks
                .Where(a => a.UserId == userId && a.IsCompleted == isCompleted)
                .ToListAsync();
            return tasks.Select(a => new UpdateTodoTask
            {
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                IsCompleted = a.IsCompleted,
                Priority = a.Priority
            }).ToList();
        }

        public async Task<List<UpdateTodoTask>> SearchTaskByPriorityAsync(int priority, Guid userId)
        {
            var tasks = await _context.Tasks
                .Where(a => a.UserId == userId && a.Priority == priority)
                .ToListAsync();
            return tasks.Select(a => new UpdateTodoTask
            {
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate,
                IsCompleted = a.IsCompleted,
                Priority = a.Priority
            }).ToList();
        }
    }
}
