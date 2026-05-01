using NoteProject.DTO;

namespace NoteProject.Interfaces;

public interface IUserService
{
    UpdateProfileDto UpdateProfile(Guid userId);
}
