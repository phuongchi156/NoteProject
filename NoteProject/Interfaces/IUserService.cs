using NoteProject.DTO.UserDTO;

namespace NoteProject.Interfaces;

public interface IUserService
{
    UpdateProfileDto UpdateProfile(Guid userId);
}
