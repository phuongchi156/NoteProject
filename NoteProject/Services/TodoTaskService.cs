using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.TodoTaskDTO;
using NoteProject.Interfaces;
using NoteProject.Models;
using System.Threading.Tasks;

namespace NoteProject.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly NoteDbContext _context;

        public TodoTaskService(NoteDbContext context)
        {
            _context = context;
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

        public Task<List<UpdateTodoTask>> GetAllTasksAsync(Guid userId)
        {
            var tasks = _context.Tasks
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

            if (existingTask == null)
            {
                throw new Exception("Task not found.");
            }

            existingTask.Title = updateTodoTaskDto.Title;
            existingTask.Description = updateTodoTaskDto.Description;
            existingTask.DueDate = updateTodoTaskDto.DueDate;
            existingTask.IsCompleted = updateTodoTaskDto.IsCompleted;
            existingTask.Priority = updateTodoTaskDto.Priority;
            existingTask.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
