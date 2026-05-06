namespace NoteProject.DTO.UserDTO;

public class UpdateProfileDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
    public IFormFile? AvatarUrl { get; set; }
}
