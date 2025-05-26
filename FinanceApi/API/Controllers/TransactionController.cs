using FinanceApi.Application.DTO;
using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Entities;
using FinanceApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<ITransactionRepository> _logger;
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService, ILogger<ITransactionRepository> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            IEnumerable<Transaction> transactions = await _transactionService.GetAllTransactions();

            return Ok(transactions);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetTransactionsByCategory(int categoryId)
        {
            IEnumerable<Transaction> categoryTransactions = await _transactionService
                .GetTransactionsByCategoryId(categoryId);
            
            return Ok(categoryTransactions);
        }
        
        //TODO: Add unique routing
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionDTO transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _transactionService.AddTransaction(transaction);

                    if (result)
                    {
                        _logger.LogInformation("Created transaction successfully.");
                        return Ok();
                    }
                }

                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "There was an error creating the transaction.");
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return BadRequest();
        }
        
        //TODO: Add unique routing
        [HttpPost]
        public async Task<IActionResult> CreateMonthlyTransaction(TransactionDTO transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _transactionService.AddRepeatingMonthlyTransaction(transaction);

                    if (result)
                    {
                        _logger.LogInformation("Successfully created the monthly transactions.");
                        return Ok();
                    }
                }

                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "There was an error creating the monthly transactions.");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBiWeeklyTransaction(TransactionDTO transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _transactionService.AddRepeatingBiWeeklyTransaction(transaction);

                    if (result)
                    {
                        _logger.LogInformation("Successfully created the bi-weekly transactions.");
                        return Ok();
                    }
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "There was an error creating the bi-weekly transactions.");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction(TransactionDTO transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Transaction updatedTransaction = await _transactionService
                        .UpdateTransaction(transaction);

                    _logger.LogInformation("Transaction was updated successfully.");
                    return Ok(updatedTransaction);
                }

                catch(ApplicationException ex)
                {
                    _logger.LogError(ex, "There was an error updating the transaction.");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            try
            {
                bool result = await _transactionService.RemoveTransactionById(transactionId);

                if (result)
                {
                    _logger.LogInformation("Transaction was deleted successfully.");
                    return Ok();
                }
            }

            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "There was an error deleting the transaction.");
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
        }
    }
}