using Check.Models;

namespace Check.Interfaces;

public interface ITransactionService
{
    Task<TransactionVm> Create(TransactionModel model);
    Task<TransactionVm> Get(Guid id);
    Task<List<TransactionVm>> GetAll();
    Task<List<TransactionVm>> GetAll(Guid userId);
    Task<bool> Complete(Guid id);
    Task<TransactionVm> Update(Guid id, TransactionModel model);
    Task<bool> SoftDelete(Guid id);
}
