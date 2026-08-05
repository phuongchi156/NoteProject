
using NoteProject.DTO.TodoTaskDTO;
using NoteProject.Models;


namespace NoteProject.Interfaces
{
    public interface ITodoTaskService
    {
        Task CreateTaskAsync(CreateTodoTask dto, Guid userId);
        Task<UpdateTodoTask> GetTodoTaskByIdAsync(Guid userId, Guid taskId);
        Task<List<UpdateTodoTask>> GetAllTasksAsync(Guid userId);
        Task UpdateTodoTaskAsync(Guid userId, Guid taskId, UpdateTodoTask updateTodoTaskDto);
        bool DeleteTodoTaskAsync(Guid userId, Guid taskId);
        Task SearchTaskByStatusAsync(bool isCompleted, Guid userId);
        Task SearchTaskByPriorityAsync(int priority, Guid userId);
    }
}
