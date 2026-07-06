namespace NoteProject.DTO.TodoTaskDTO
{
    public class UpdateTodoTask
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int Priority { get; set; } //1 = hight, 2 = medium, 3 = low
    }
}
