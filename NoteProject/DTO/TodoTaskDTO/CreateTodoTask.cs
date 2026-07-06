namespace NoteProject.DTO.TodoTaskDTO
{
    public class CreateTodoTask
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; } //1 = hight, 2 = medium, 3 = low
    }
}
