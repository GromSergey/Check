using Check.Models;

namespace Check.Interfaces;

public interface ISessionService
{
    Task<TokenVm> SignInAsync(SignInModel model);
    public string GetPasswordHash(string password);
}
