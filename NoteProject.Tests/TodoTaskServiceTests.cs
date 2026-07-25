using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.TodoTaskDTO;
using NoteProject.Services;

namespace NoteProject.Tests
{
    public class TodoTaskServiceTests
    {

        //CreateTask_Should_Add_New_Task ---------------
        //UpdateTask_Should_Update_Task ---------------
        //UpdateTask_Should_Throw_When_Task_Not_Found()-----------
        //UpdateTask_Should_Not_Update_Other_User_Task()-----------
        //DeleteTask_Should_Remove_Task -----------------
        //GetAllTasks_Should_Return_User_Tasks-----------------
        //GetTaskById_Should_Return_Task----------------
        //GetTaskById_Should_NotFound_Other_User_Task-----------------
        //GetTaskById_Should_Throw_When_Task_Not_Found-----------------
        //SearchTask_Should_Return_Matching_Task
        //FilterTask_Should_Return_HighPriority_Task
        //CompleteTask_Should_Set_IsCompleted_True

        [Fact]
        public async Task CreateTask_Should_Add_New_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var context = new NoteDbContext(options);

            var service = new TodoTaskService(context);

            var dto = new CreateTodoTask
            {
                Title = "Learn Unit Test",
                Description = "Practice xUnit",
                Priority = 1
            };

            var userId = Guid.NewGuid();

            // Act
            await service.CreateTaskAsync(dto, userId);

            // Assert
            Assert.Single(context.Tasks);

            var task = context.Tasks.First();

            Assert.Equal("Learn Unit Test", task.Title);
            Assert.Equal(userId, task.UserId);
        }

        [Fact]
        public async Task DeleteTask_Should_Remove_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId = Guid.NewGuid();
            var task = new Models.TodoTask
            {
                Title = "Learn Unit Test",
                Description = "Practice xUnit",
                Priority = 1,
                UserId = userId
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            // Act
            var result = service.DeleteTodoTaskAsync(userId, task.Id);
            // Assert
            Assert.True(result);
            Assert.Empty(context.Tasks);
        }

        [Fact]
        public async Task UpdateTask_Should_Update_Task
            ()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new NoteDbContext(options);

            var service = new TodoTaskService(context);

            var userId = Guid.NewGuid();

            var task = new Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Learn Unit Test",
                Description = "Practice xUnit",
                Priority = 1,
                UserId = userId
            };

            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            var dto = new UpdateTodoTask
            {
                Title = "Learn Integration Test",
                Description = "Practice xUnit and Moq",
                Priority = 2
            };

            // Act
            await service.UpdateTodoTaskAsync(userId, task.Id, dto);
            // Assert
            var updatedTask = await context.Tasks.FindAsync(task.Id);

            Assert.NotNull(updatedTask);
            Assert.Equal("Learn Integration Test", updatedTask!.Title);
            Assert.Equal("Practice xUnit and Moq", updatedTask.Description);
            Assert.Equal(2, updatedTask.Priority);
        }

        [Fact]
        public async Task UpdateTask_Should_Throw_When_Task_Not_Found()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var dto = new UpdateTodoTask
            {
                Title = "Learn Integration Test",
                Description = "Practice xUnit and Moq",
                Priority = 2
            };
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(
                () => service.UpdateTodoTaskAsync(userId, taskId, dto));

            //Assert
            Assert.Equal("Task not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateTask_Should_Not_Update_Other_User_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var task = new Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Learn Unit Test",
                Description = "Practice xUnit",
                Priority = 1,
                UserId = userId1
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            var dto = new UpdateTodoTask
            {
                Title = "Learn Integration Test",
                Description = "Practice xUnit and Moq",
                Priority = 2
            };
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(
                () => service.UpdateTodoTaskAsync(userId2, task.Id, dto));
            // Assert
            Assert.Equal("Task not found.", ex.Message);
        }

        [Fact]
        public async Task GetAllTasks_Should_Return_User_Tasks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId = Guid.NewGuid();
            var task1 = new Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description 1",
                Priority = 1,
                UserId = userId
            };
            var task2 = new Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                Description = "Description 2",
                Priority = 2,
                UserId = userId
            };
            context.Tasks.AddRange(task1, task2);
            await context.SaveChangesAsync();
            // Act
            var tasks = await service.GetAllTasksAsync(userId);
            // Assert
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public async Task GetTaskById_Should_Return_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId = Guid.NewGuid();
            var task = new Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description 1",
                Priority = 1,
                UserId = userId
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            // Act
            var result = await service.GetTodoTaskByIdAsync(userId, task.Id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Task 1", result.Title);
        }

        [Fact]
        public async Task GetTaskById_Should_NotFound_Other_User_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var task = new Models.TodoTask
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description 1",
                Priority = 1,
                UserId = userId1
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(
                () => service.GetTodoTaskByIdAsync(userId2, task.Id));
            // Assert
            Assert.Equal("Task not found.", ex.Message);
        }

        [Fact]
        public async Task GetTaskById_Should_Throw_When_Task_Not_Found()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var context = new NoteDbContext(options);
            var service = new TodoTaskService(context);
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            // Act
            var ex = await Assert.ThrowsAsync<Exception>(
                () => service.GetTodoTaskByIdAsync(userId, taskId));
            // Assert
            Assert.Equal("Task not found.", ex.Message);
        }

    }
}