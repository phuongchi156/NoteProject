using Microsoft.EntityFrameworkCore;
using NoteProject.DTO;
using NoteProject.Interfaces;
using NoteProject.Models;

namespace NoteProject.Services;

public class UserService : IUserService
{
    public UpdateProfileDto UpdateProfile(Guid userId)
    {
        throw new NotImplementedException();
    }

    public bool CheckAvtUrl(IFormFile file, Guid UserId)
    {
        //var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        //var extension = Path.GetExtension(file.FileName).ToLower();
        //if (file == null || file.Length == 0)
        //    return BadRequest("Invalid file");

        //var userId = int.Parse(User.FindFirst("id")!.Value);

        //var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //var path = Path.Combine("wwwroot/avatars", fileName);

        //using (var stream = new FileStream(path, FileMode.Create))
        //{
        //    await file.CopyToAsync(stream);
        //}

        //var user = await _context.Users.FindAsync(userId);
        //user!.AvatarUrl = $"/avatars/{fileName}";
        //return allowedExtensions.Contains(extension);
        return true;
    }
}
