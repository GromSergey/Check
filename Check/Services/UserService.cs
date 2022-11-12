using AutoMapper;
using Check.Interfaces;
using Check.Models;
using Check.Database.Entities;
using Check.Database;

namespace Check.Services;

public class UserService : IUserService
{
    private readonly Mapper _mapper;
    private readonly ISessionService _sessionService;
    private readonly AppDbContext _appDbContext;

    public UserService(ISessionService sessionService, AppDbContext appDbContext)
    {
        _sessionService = sessionService;
        _appDbContext = appDbContext;

        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<UserModel, User>()
                .ForMember("Password", opt => opt.MapFrom(c => _sessionService.GetPasswordHash(c.Password)));
            cfg.CreateMap<User, UserVm>();
        });
        _mapper = new Mapper(config);
    }

    public async Task<UserVm> Create(UserModel model)
    {
        var newUser = _mapper.Map<UserModel, User>(model);
        _appDbContext.Users.Add(newUser);
        await _appDbContext.SaveChangesAsync();
        var result = _mapper.Map<User, UserVm>(newUser);
        return result;
    }
}
