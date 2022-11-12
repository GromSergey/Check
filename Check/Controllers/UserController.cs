using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Check.Interfaces;
using Check.Models;
using Check.Database;
using Check.Database.Entities;

namespace Check.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    readonly AppDbContext _appDbContext;
    readonly IUserService _userService;

    public UserController(IUserService userService, AppDbContext context)
    {
        _appDbContext = context;
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserVm>> Create([FromBody] UserModel model)
    {
        UserVm newUser = await _userService.Create(model);
        return newUser;
    }
}

