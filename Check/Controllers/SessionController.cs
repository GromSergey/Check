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
public class SessionController : ControllerBase
{
    readonly ISessionService _sessionService;
    readonly AppDbContext _appDbContext;
    public SessionController(ISessionService sessionService, AppDbContext context)
    {
        _sessionService = sessionService;
        _appDbContext = context;
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult<TokenVm>> SignInAsync([FromBody] SignInModel model)
    {
        return await _sessionService.SignInAsync(model);
    }

    [Authorize]
    [HttpGet("Hello")]
    public async Task<ActionResult<List<User>>> hello()
    {
        var users = await _appDbContext.Users.ToListAsync();
        return users;
    }
}

