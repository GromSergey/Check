using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

    public async Task<UserVm> Get(Guid id)
    { 
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
            throw new Exception("User not found");

        var userVm = _mapper.Map<User, UserVm>(user);
        return userVm;
    }

    public async Task<List<UserVm>> GetAll()
    {
        var users = await _appDbContext.Users.ToListAsync();
        var nonDeletedUsers = users.Where(x => !x.IsDeleted).ToList();

        var userVms = _mapper.Map<List<User>, List<UserVm>>(nonDeletedUsers);
        return userVms;
    }

    public async Task<UserVm> Update(Guid id, UserModel model)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
            throw new Exception("User not found");

        user.Username = model.Username;
        user.Fullname = model.Fullname;
        user.Password = model.Password;
        user.Email = model.Email;
        user.About = model.About;
        user.ImageUrl = model.ImageUrl;
        user.BackgroundUrl = model.BackgroundUrl;
        user.Address = model.Address;
        user.IsVerified = model.IsVerified;
        user.Role = model.Role;
        user.TiktokName = model.TiktokName;
        user.TwitterName = model.TwitterName;
        user.VkName = model.VkName;
        user.TelegramName = model.TelegramName;
        user.InstagramName = model.InstagramName;
        await _appDbContext.SaveChangesAsync();

        var userVm = _mapper.Map<User, UserVm>(user);
        return userVm;
    }

    public async Task<bool> SoftDelete(Guid id)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
            throw new Exception("User not found");

        user.IsDeleted = true;
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
