using Check.Models;

namespace Check.Interfaces;

public interface IUserService
{
    Task<UserVm> Create(UserModel model);
    Task<UserVm> Get(string username);
    Task<UserVm> Get(Guid id);
    Task<List<UserVm>> GetAll();
    Task<UserVm> Update(Guid id, UserModel model);
    Task<bool> SoftDelete(Guid id);
}
