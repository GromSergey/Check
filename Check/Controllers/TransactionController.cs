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
public class TransactionController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly ITransactionService _transactionService;

    public TransactionController(AppDbContext appDbContext, ITransactionService transactionService)
    {
        _appDbContext = appDbContext;
        _transactionService = transactionService;
    }

    [HttpPost()]
    public async Task<ActionResult<TransactionVm>> Create([FromBody] TransactionModel model)
    {
        var newTransaction = await _transactionService.Create(model);
        return newTransaction;
    }

    [HttpGet()]
    public async Task<ActionResult<List<TransactionVm>>> GetAll()
    {
        var transactions = await _transactionService.GetAll();
        return transactions;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<List<TransactionVm>>> GetAll(Guid userId)
    {
        var transactions = await _transactionService.GetAll(userId);
        return transactions;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionVm>> Get(Guid id)
    {
        var transaction = await _transactionService.Get(id);
        return transaction;
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TransactionVm>> Update(Guid id, [FromBody] TransactionModel model)
    {
        var newTransaction = await _transactionService.Update(id, model);
        return newTransaction;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _transactionService.SoftDelete(id);
        return result;
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<bool>> Complete(Guid id)
    {
        var result = await _transactionService.Complete(id);
        return result;
    }
}
