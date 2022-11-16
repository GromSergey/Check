using Check.Models;

namespace Check.Interfaces;

public interface IGiftService
{
    Task<GiftVm> Create(GiftModel model);
    Task<GiftVm> Get(Guid id);
    Task<List<GiftVm>> GetAll(Guid userId);
    Task<GiftVm> Update(Guid id, GiftModel model);
    Task<bool> SoftDelete(Guid id);
    Task<bool> SoftDeleteBulk(Guid userId, bool isGifted);
}
