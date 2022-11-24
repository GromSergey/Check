using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Check.Interfaces;
using Check.Models;
using Check.Database;
using System.Security.Claims;

namespace Check.Controllers;

[ApiController]
[Route("[controller]")]
public class GiftController : Controller
{
    private readonly IGiftService _gitftService;

    public GiftController(IGiftService giftService)
    {
        _gitftService = giftService;
    }

    [HttpPost()]
    [Authorize]
    public async Task<ActionResult<GiftVm>> Create([FromBody] GiftModel model)
    {
        if (User.IsInRole("User") && User.FindFirstValue("guid") != model.UserId.ToString())
            throw new Exception("Security access denied");
        var newGift = await _gitftService.Create(model);
        return newGift;
    }

    [HttpGet()]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<GiftVm>>> GetAll(Guid userId)
    {
        var gifts = await _gitftService.GetAll(userId);
        return gifts;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<GiftVm>> Get(Guid id)
    {
        var gift = await _gitftService.Get(id);
        return gift;
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GiftVm>> Update(Guid id, [FromBody] GiftModel model)
    {
        var newGift = await _gitftService.Update(id, model);
        return newGift;
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var gift = await _gitftService.Get(id);
        if (User.IsInRole("User") && User.FindFirstValue("guid") != gift.UserId.ToString())
            throw new Exception("Security access denied");
        var result = await _gitftService.SoftDelete(id);
        return result;
    }

    [HttpDelete("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeleteBulk(Guid userId, bool isGifted)
    {
        if (User.IsInRole("User") && User.FindFirstValue("guid") != userId.ToString())
            throw new Exception("Security access denied");
        var result = await _gitftService.SoftDeleteBulk(userId, isGifted);
        return result;
    }
}
