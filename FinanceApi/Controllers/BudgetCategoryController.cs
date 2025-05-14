using FinanceApi.DAL.Interface;
using FinanceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryController : ControllerBase
    {
        private readonly IBudgetCategoryRepository _budgetCategoryRepository;
        private readonly ILogger<IBudgetCategoryRepository> _logger;

        public BudgetCategoryController(IBudgetCategoryRepository budgetCategoryRepository, 
            ILogger<IBudgetCategoryRepository> logger)
        {
            _budgetCategoryRepository = budgetCategoryRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudgetCategory(BudgetCategory budgetCategory)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    bool result = await _budgetCategoryRepository
                        .AddBudgetCategoryAsync(budgetCategory);
                    
                    if(!result)
                    {
                        _logger.LogInformation("Created budget category successfully.");
                        return Ok();
                    }
                }

                catch(ApplicationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex,"There was an error creating the budget category.");
                }
            }
            
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBudgetCategory(BudgetCategory budgetCategory)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    BudgetCategory updatedBudgetCategory = await _budgetCategoryRepository
                        .UpdateBudgetCategoryAsync(budgetCategory);
                    
                    _logger.LogInformation("Budget category was updated successfully.");
                    return Ok(updatedBudgetCategory);
                }

                catch(ApplicationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "There was an error updating the budget category.");
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBudgetCategory(int budgetCategoryId)
        {
            try
            {
                bool result = await _budgetCategoryRepository
                    .RemoveBudgetCategoryAsync(budgetCategoryId);
                
                if (!result)
                {
                    _logger.LogInformation("Budget category was removed successfully.");
                    return Ok();
                }
            }
            
            catch(ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex, "There was an error removing the budget category.");
            }

            return BadRequest();
        }
    }
}
