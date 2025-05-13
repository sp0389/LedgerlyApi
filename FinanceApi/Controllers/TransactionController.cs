using FinanceApi.Models;
using FinanceApi.DAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            IEnumerable<Transaction> transactions = await _transactionRepository
                .GetAllTransactionsAsync();

            return Ok(transactions);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetTransactionsByCategory(int categoryId)
        {
            IEnumerable<Transaction> categoryTransactions = await _transactionRepository
                .GetTransactionsByCategoryIdAsync(categoryId);

            return Ok(categoryTransactions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _transactionRepository.AddTransactionAsync(transaction);

                    if (!result)
                    {
                        return Ok(transaction);
                    }
                }

                catch (ApplicationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Transaction updatedTransaction = await _transactionRepository
                        .UpdateTransactionAsync(transaction);

                    return Ok(updatedTransaction);
                }

                catch(ApplicationException ex)
                {
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
                bool result = await _transactionRepository.RemoveTransactionById(transactionId);

                if (!result)
                {
                    return Ok();
                }
            }

            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
        }
    }
}