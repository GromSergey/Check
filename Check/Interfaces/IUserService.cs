using Check.Models;

namespace Check.Interfaces;

public interface IUserService
{
    Task<UserVm> Create(UserModel model);
}
