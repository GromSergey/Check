using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Check.Interfaces;
using Check.Models;
using Check.Database.Entities;
using Check.Database;

namespace Check.Services;

public class TransactionService : ITransactionService
{
    private readonly Mapper _mapper;
    private readonly AppDbContext _appDbContext;

    public TransactionService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<TransactionModel, Transaction>();
            cfg.CreateMap<Transaction, TransactionVm>();
        });
        _mapper = new Mapper(config);
    }

    public async Task<TransactionVm> Create(TransactionModel model)
    {
        var newTransaction = _mapper.Map<TransactionModel, Transaction>(model);

        _appDbContext.Transactions.Add(newTransaction);
        await _appDbContext.SaveChangesAsync();

        var result = _mapper.Map<Transaction, TransactionVm>(newTransaction);
        return result;
    }

    public async Task<TransactionVm> Get(Guid id)
    {
        var transaction = await _appDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (transaction == null)
            throw new Exception("Transaction not found");

        var transactionVm = _mapper.Map<Transaction, TransactionVm>(transaction);
        return transactionVm;
    }

    public async Task<List<TransactionVm>> GetAll()
    {
        var transactions = await _appDbContext.Transactions.ToListAsync();
        var nonDeletedTransactions = transactions.Where(x => !x.IsDeleted).ToList();

        var transactionVms = _mapper.Map<List<Transaction>, List<TransactionVm>>(nonDeletedTransactions);
        return transactionVms;
    }

    public async Task<TransactionVm> Update(Guid id, TransactionModel model)
    {
        var transaction = await _appDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (transaction == null)
            throw new Exception("Transaction not found");

        transaction.GiftId = model.GiftId;
        transaction.UserId = model.UserId;
        transaction.GifterId = model.GifterId;
        transaction.IsCompleted = model.IsCompleted;
        await _appDbContext.SaveChangesAsync();

        var transactionVm = _mapper.Map<Transaction, TransactionVm>(transaction);
        return transactionVm;
    }

    public async Task<bool> SoftDelete(Guid id)
    {
        var transaction = await _appDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (transaction == null)
            throw new Exception("Transaction not found");

        transaction.IsDeleted = true;
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
