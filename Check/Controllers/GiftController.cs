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
public class GiftController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly IGiftService _gitftService;

    public GiftController(AppDbContext appDbContext, IGiftService giftService)
    {
        _appDbContext = appDbContext;
        _gitftService = giftService;
    }

    [HttpPost()]
    public async Task<ActionResult<GiftVm>> Create([FromBody] GiftModel model)
    {
        var newGift = await _gitftService.Create(model);
        return newGift;
    }

    [HttpGet()]
    public async Task<ActionResult<List<GiftVm>>> GetAll()
    {
        var gifts = await _gitftService.GetAll();
        return gifts;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GiftVm>> Get(Guid id)
    {
        var gift = await _gitftService.Get(id);
        return gift;
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GiftVm>> Update(Guid id, [FromBody] GiftModel model)
    {
        var newGift = await _gitftService.Update(id, model);
        return newGift;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _gitftService.SoftDelete(id);
        return result;
    }
}
