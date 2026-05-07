using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.UserDTO;
using NoteProject.Interfaces;
using NoteProject.Models;

namespace NoteProject.Services;

public class UserService : IUserService
{
    public UpdateProfileDto UpdateProfile(Guid userId)
    {
        throw new NotImplementedException();
    }
}
