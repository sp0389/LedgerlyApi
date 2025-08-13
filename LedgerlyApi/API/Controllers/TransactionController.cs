using LedgerlyApi.Application.DTO;
using LedgerlyApi.Application.Interfaces;
using LedgerlyApi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LedgerlyApi.API.Controllers;

[Authorize(Policy = "RequireMemberRole")]
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
    [Route("CreateTransaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transaction)
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

    [HttpGet]
    [Route("IncomeBalance")]
    public async Task<IActionResult> GetIncomeTransactionBalance()
    {
        try
        {
            var balance = await _transactionService.GetTransactionBalance(TransactionType.Income);
            return Ok(balance);
        }

        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error getting the income transaction balance.");
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("ExpenseBalance")]
    public async Task<IActionResult> GetExpenseTransactionBalance()
    {
        try
        {
            var balance = await _transactionService.GetTransactionBalance(TransactionType.Expense);
            return Ok(balance);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error getting the expense transaction balance.");
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("LastFive")]
    public async Task<IActionResult> GetLastFiveTransactions()
    {
        try
        {
            var transactions = await _transactionService.GetLastFiveTransactions();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error getting the last five transactions.");
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("MonthlyIncomeAmounts/{year:int}")]
    public async Task<IActionResult> GetMonthlyIncomeTransactionAmounts(int year)
    {
        try
        {
            var transactionAmounts = await _transactionService
                .GetMonthlyTransactionAmountsForYear(year, TransactionType.Income);
            return Ok(transactionAmounts);
        }
        
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error getting the monthly income transaction amounts.");
        }
        
        return BadRequest();
    }
    
    [HttpGet]
    [Route("MonthlyExpenseAmounts/{year:int}")]
    public async Task<IActionResult> GetMonthlyExpenseTransactionAmounts(int year)
    {
        try
        {
            var transactionAmounts = await _transactionService
                .GetMonthlyTransactionAmountsForYear(year, TransactionType.Expense);
            return Ok(transactionAmounts);
        }
        
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error getting the monthly expense transaction amounts.");
        }
        
        return BadRequest();
    }
}