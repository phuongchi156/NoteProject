using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NoteProject.DTO.TodoTaskDTO;
using NoteProject.Interfaces;
using NoteProject.Models;
using System.Threading.Tasks;

namespace NoteProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoTaskController : ControllerBase
    {
        private readonly NoteDbContext _context;
        private readonly ITodoTaskService _taskService;
        public TodoTaskController(NoteDbContext context, ITodoTaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }
        //o Tạo công việc: Thêm công việc cần làm.
        //o Cập nhật công việc: Chỉnh sửa tên công việc, mô tả.
        //o Đánh dấu hoàn thành công việc: Đánh dấu công việc đã hoàn thành.
        //o Nhắc nhở công việc: Cài đặt ngày giờ nhắc nhở cho mỗi công việc.
        //o Phân loại công việc: Phân công công việc theo nhóm hoặc ưu tiên.

        //public int Priority { get; set; } //1 = hight, 2 = medium, 3 = low

        [HttpPost("create")]
        /// <summary>
        /// Task priority.
        /// 1 = High
        /// 2 = Medium
        /// 3 = Low
        /// </summary>
        public async Task<ActionResult<CreateTodoTask>> CreateTask([FromBody] CreateTodoTask task)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            await _taskService.CreateTaskAsync(task, Guid.Parse(user));
            return Ok();
        }

        // chỉnh sửa task cài đặt ngày giờ nhắc nhở cho mỗi công việc, xác nhận hoàn thành công việc, phân loại công việc theo nhóm hoặc ưu tiên.
        [HttpPut("update/{id}")]
        /// <summary>
        /// Task priority.
        /// 1 = High
        /// 2 = Medium
        /// 3 = Low
        /// </summary>
        public async Task<ActionResult<UpdateTodoTask>> UpdateTask(Guid id, [FromBody] UpdateTodoTask task)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            await _taskService.UpdateTodoTaskAsync(Guid.Parse(user), id, task);
            return Ok();
        }

        //[HttpPatch("complete/{id}")]
        //public async Task<ActionResult<UpdateTodoTask>> CompleteTask(Guid id)
        //{
        //    var existingTask = await _context.Tasks.FindAsync(id);
        //    if (existingTask == null)
        //    {
        //        return NotFound();
        //    }
        //    existingTask.IsCompleted = true;
        //    existingTask.UpdatedAt = DateTime.Now;
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<TodoTask>> DeleteTask(Guid id)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var result = _taskService.DeleteTodoTaskAsync(Guid.Parse(user), id);

            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("list")]
        /// <summary>
        /// Priority of the task.
        /// 1 = High
        /// 2 = Medium
        /// 3 = Low
        /// </summary>
        public async Task<ActionResult<List<UpdateTodoTask>>> GetTasks()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var result = await _taskService.GetAllTasksAsync(Guid.Parse(user));
            return Ok(result);
        }

        [HttpGet("list/{id}")]
        /// <summary>
        /// Priority of the task.
        /// 1 = High
        /// 2 = Medium
        /// 3 = Low
        /// </summary>
        public async Task<ActionResult<UpdateTodoTask>> GetTaskById(Guid id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            await _taskService.GetTodoTaskByIdAsync(Guid.Parse(userId), id);
            return Ok();
        }

        [HttpGet("searchByPriority")]
        public async Task<ActionResult<List<UpdateTodoTask>>> SearchTask(int prioty)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var result = await _taskService.SearchTaskByPriorityAsync(prioty, Guid.Parse(userId));
            return Ok(result);
        }

        [HttpGet("searchByStatus")]
        public async Task<ActionResult<List<UpdateTodoTask>>> SearchTaskByStatus(bool isCompleted)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var result = await _taskService.SearchTaskByStatusAsync(isCompleted, Guid.Parse(userId));
            return Ok(result);
        }

        [HttpGet("searchByTime")]
        public async Task<ActionResult<List<UpdateTodoTask>>> SearchTaskByTime(DateTime? startTime, DateTime? endTime)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var result = await _taskService.SearchTodoTasksByTimeAsync(Guid.Parse(userId), startTime, endTime);
            return Ok(result);
        }

        [HttpGet("searchByTitle")]
        public async Task<ActionResult<List<UpdateTodoTask>>> SehttprchTaskByTitle(string title)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var result = await _taskService.SearchTodoTasksAsync(Guid.Parse(userId), title);
            return Ok(result);
        }
    }
}
