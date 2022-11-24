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
    public SessionController(ISessionService sessionService, AppDbContext context)
    {
        _sessionService = sessionService;
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult<TokenVm>> SignInAsync([FromBody] SignInModel model)
    {
        return await _sessionService.SignInAsync(model);
    }
}

