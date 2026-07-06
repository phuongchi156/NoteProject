using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using System;

namespace NoteProject.Models;

public class TodoTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int Priority { get; set; } //1 = hight, 2 = medium, 3 = low

    public User User { get; set; } = null!;
}
