using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Check.Interfaces;
using Check.Models;
using Check.Database.Entities;
using Check.Database;

namespace Check.Services;

public class GiftService : IGiftService
{
    private readonly Mapper _mapper;
    private readonly AppDbContext _appDbContext;

    public GiftService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<GiftModel, Gift>();
            cfg.CreateMap<Gift, GiftVm>();
        });
        _mapper = new Mapper(config);
    }

    public async Task<GiftVm> Create(GiftModel model)
    {
        var newGift = _mapper.Map<GiftModel, Gift>(model);

        _appDbContext.Gifts.Add(newGift);
        await _appDbContext.SaveChangesAsync();

        var result = _mapper.Map<Gift, GiftVm>(newGift);
        return result;
    }

    public async Task<GiftVm> Get(Guid id)
    {
        var gift = await _appDbContext.Gifts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (gift == null)
            throw new Exception("Gift not found");

        var giftVm = _mapper.Map<Gift, GiftVm>(gift);
        return giftVm;
    }

    public async Task<List<GiftVm>> GetAll()
    {
        var gifts = await _appDbContext.Gifts.ToListAsync();
        var nonDeletedGifts = gifts.Where(x => !x.IsDeleted).ToList();

        var giftVms = _mapper.Map<List<Gift>, List<GiftVm>>(nonDeletedGifts);
        return giftVms;
    }

    public async Task<GiftVm> Update(Guid id, GiftModel model)
    {
        var gift = await _appDbContext.Gifts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (gift == null)
            throw new Exception("Gift not found");

        gift.Title = model.Title;
        gift.Description = model.Description;
        gift.ShopName = model.ShopName;
        gift.ShopUrl = model.ShopUrl;
        gift.Price = model.Price;
        gift.ImageUrl = model.ImageUrl;
        gift.IsGifted = model.IsGifted;
        gift.UserId = model.UserId;
        await _appDbContext.SaveChangesAsync();

        var giftVm = _mapper.Map<Gift, GiftVm>(gift);
        return giftVm;
    }

    public async Task<bool> SoftDelete(Guid id)
    {
        var gift = await _appDbContext.Gifts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (gift == null)
            throw new Exception("Gift not found");

        gift.IsDeleted = true;
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
