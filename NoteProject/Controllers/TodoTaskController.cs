using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NoteProject.DTO.TodoTaskDTO;
using NoteProject.Models;

namespace NoteProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoTaskController : ControllerBase
    {
        private readonly NoteDbContext _context;
        public TodoTaskController(NoteDbContext context)
        {
            _context = context;
        }
        //o Tạo công việc: Thêm công việc cần làm.
        //o Cập nhật công việc: Chỉnh sửa tên công việc, mô tả.
        //o Đánh dấu hoàn thành công việc: Đánh dấu công việc đã hoàn thành.
        //o Nhắc nhở công việc: Cài đặt ngày giờ nhắc nhở cho mỗi công việc.
        //o Phân loại công việc: Phân công công việc theo nhóm hoặc ưu tiên.

        //public int Priority { get; set; } //1 = hight, 2 = medium, 3 = low

        public User User { get; set; } = null!;

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
            var newTask = new TodoTask
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                UserId = Guid.Parse(user)
            };
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
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
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.IsCompleted = task.IsCompleted;
            existingTask.Priority = task.Priority;
            existingTask.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
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
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }
            _context.Tasks.Remove(existingTask);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("list")]
        /// <summary>
        /// Priority of the task.
        /// 1 = High
        /// 2 = Medium
        /// 3 = Low
        /// </summary>
        public ActionResult<List<UpdateTodoTask>> GetTasks()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var tasks = _context.Tasks.Where(t => t.UserId == Guid.Parse(user)).ToList();
            return Ok(tasks);
        }

        [HttpGet("list/{id}")]
        /// <summary>
        /// Priority of the task.
        /// 1 = High
        /// 2 = Medium
        /// 3 = Low
        /// </summary>
        public async Task<ActionResult<UpdateTodoTask>> GetTask(Guid id)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }
            return Ok(existingTask);
        }
    }
}
