using FinanceApi.Application.DTO;
using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Entities;
using FinanceApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetCategoryController : ControllerBase
    {
        private readonly IBudgetCategoryService _budgetCategoryService;
        private readonly ILogger<IBudgetCategoryRepository> _logger;

        public BudgetCategoryController(IBudgetCategoryService budgetCategoryService, 
            ILogger<IBudgetCategoryRepository> logger)
        {
            _budgetCategoryService = budgetCategoryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudgetCategory(BudgetCategoryDTO budgetCategory)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    bool result = await _budgetCategoryService
                        .AddBudgetCategory(budgetCategory);
                    
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
        public async Task<IActionResult> UpdateBudgetCategory(BudgetCategoryDTO budgetCategory)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    BudgetCategory updatedBudgetCategory = await _budgetCategoryService
                        .UpdateBudgetCategory(budgetCategory);
                    
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
                bool result = await _budgetCategoryService
                    .RemoveBudgetCategory(budgetCategoryId);
                
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