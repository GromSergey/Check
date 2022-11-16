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
    readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost()]
    public async Task<ActionResult<UserVm>> Create([FromBody] UserModel model)
    {
        var newUser = await _userService.Create(model);
        return newUser;
    }

    [HttpGet()]
    public async Task<ActionResult<List<UserVm>>> GetAll()
    {
        var users = await _userService.GetAll();
        return users;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserVm>> Get(Guid id)
    {
        var user = await _userService.Get(id);
        return user;
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserVm>> Update(Guid id, [FromBody] UserModel model)
    {
        var newUser = await _userService.Update(id, model);
        return newUser;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _userService.SoftDelete(id);
        return result;
    }
}

