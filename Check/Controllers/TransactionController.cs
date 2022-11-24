using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Check.Interfaces;
using Check.Models;
using System.Security.Claims;

namespace Check.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost()]
    [Authorize]
    public async Task<ActionResult<TransactionVm>> Create([FromBody] TransactionModel model)
    {
        if (User.IsInRole("User") && User.FindFirstValue("guid") != model.GifterId.ToString())
            throw new Exception("Security access denied");
        var newTransaction = await _transactionService.Create(model);
        return newTransaction;
    }

    [HttpGet()]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<TransactionVm>>> GetAll()
    {
        var transactions = await _transactionService.GetAll();
        return transactions;
    }

    [HttpGet("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult<List<TransactionVm>>> GetAll(Guid userId)
    {
        if (User.IsInRole("User") && User.FindFirstValue("guid") != userId.ToString())
            throw new Exception("Security access denied");
        var transactions = await _transactionService.GetAll(userId);
        return transactions;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<TransactionVm>> Get(Guid id)
    {
        var transaction = await _transactionService.Get(id);
        if (User.IsInRole("User") && User.FindFirstValue("guid") != transaction.GifterId.ToString())
            throw new Exception("Security access denied");
        return transaction;
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TransactionVm>> Update(Guid id, [FromBody] TransactionModel model)
    {
        var newTransaction = await _transactionService.Update(id, model);
        return newTransaction;
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _transactionService.SoftDelete(id);
        return result;
    }

    [HttpPatch("Complete/{id:guid}")]
    [Authorize]
    public async Task<ActionResult<bool>> Complete(Guid id)
    {
        var transaction = await _transactionService.Get(id);
        if (User.IsInRole("User") && User.FindFirstValue("guid") != transaction.GifterId.ToString())
            throw new Exception("Security access denied");
        var result = await _transactionService.Complete(id);
        return result;
    }
}
