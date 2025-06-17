using FinanceApi.Application.DTO;
using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ILogger<ITransactionService> _logger;
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService, ILogger<ITransactionService> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAllTransactions();

        return Ok(transactions);
    }

    [HttpGet]
    [Route("{transactionId:int}")]
    public async Task<IActionResult> GetTransactionById(int transactionId)
    {
        var transaction = await _transactionService.GetTransactionsById(transactionId);

        return Ok(transaction);
    }

    [HttpGet]
    [Route("Category")]
    public async Task<IActionResult> GetTransactionsByCategory(CategoryType categoryType)
    {
        var transactions = await _transactionService
            .GetTransactionsByCategory(categoryType);
        return Ok(transactions);
    }

    [HttpPost]
    [Route("/CreateTransaction")]
    public async Task<IActionResult> CreateTransaction(TransactionDto transaction)
    {
        if (ModelState.IsValid)
            try
            {
                var result = await _transactionService.AddTransaction(transaction);

                if (result)
                {
                    _logger.LogInformation("Created transaction successfully.");
                    return Ok();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the transaction.");
                ModelState.AddModelError("", ex.Message);
            }

        return BadRequest();
    }

    [HttpPost]
    [Route("CreateMonthlyTransaction")]
    public async Task<IActionResult> CreateMonthlyTransaction(TransactionDto transaction)
    {
        if (ModelState.IsValid)
            try
            {
                var result = await _transactionService.AddRepeatingMonthlyTransaction(transaction);

                if (result)
                {
                    _logger.LogInformation("Successfully created the monthly transactions.");
                    return Ok();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the monthly transactions.");
                ModelState.AddModelError("", ex.Message);
            }

        return BadRequest();
    }

    [HttpPost]
    [Route("CreateBiWeeklyTransaction")]
    public async Task<IActionResult> CreateBiWeeklyTransaction(TransactionDto transaction)
    {
        if (ModelState.IsValid)
            try
            {
                var result = await _transactionService.AddRepeatingBiWeeklyTransaction(transaction);

                if (result)
                {
                    _logger.LogInformation("Successfully created the bi-weekly transactions.");
                    return Ok();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the bi-weekly transactions.");
                ModelState.AddModelError("", ex.Message);
            }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTransaction(TransactionDto transaction)
    {
        if (ModelState.IsValid)
            try
            {
                var updatedTransaction = await _transactionService
                    .UpdateTransaction(transaction);

                _logger.LogInformation("Transaction was updated successfully.");
                return Ok(updatedTransaction);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error updating the transaction.");
                ModelState.AddModelError("", ex.Message);
            }

        return BadRequest();
    }

    [HttpDelete]
    [Route("{transactionId:int}")]
    public async Task<IActionResult> DeleteTransaction(int transactionId)
    {
        try
        {
            var result = await _transactionService.RemoveTransactionById(transactionId);

            if (result)
            {
                _logger.LogInformation("Transaction was deleted successfully.");
                return Ok();
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "There was an error deleting the transaction.");
            ModelState.AddModelError("", ex.Message);
        }

        return BadRequest();
    }
}