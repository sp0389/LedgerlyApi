using FinanceApi.Application.DTO;
using FinanceApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetCategoryController : ControllerBase
{
    private readonly IBudgetCategoryService _budgetCategoryService;
    private readonly ILogger<IBudgetCategoryService> _logger;

    public BudgetCategoryController(IBudgetCategoryService budgetCategoryService,
        ILogger<IBudgetCategoryService> logger)
    {
        _budgetCategoryService = budgetCategoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgetCategories()
    {
        var budgetCategories = await _budgetCategoryService.GetAllBudgetCategories();

        _logger.LogInformation("Returned all budget categories");
        return Ok(budgetCategories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudgetCategory(BudgetCategoryDto budgetCategory)
    {
        if (ModelState.IsValid)
            try
            {
                var result = await _budgetCategoryService
                    .AddBudgetCategory(budgetCategory);

                if (!result)
                {
                    _logger.LogInformation("Created budget category successfully.");
                    return Ok();
                }
            }
            
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex, "There was an error creating the budget category.");
            }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBudgetCategory(BudgetCategoryDto budgetCategory)
    {
        if (ModelState.IsValid)
            try
            {
                var updatedBudgetCategory = await _budgetCategoryService
                    .UpdateBudgetCategory(budgetCategory);

                _logger.LogInformation("Budget category was updated successfully.");
                return Ok(updatedBudgetCategory);
            }
            
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex, "There was an error updating the budget category.");
            }

        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveBudgetCategory(int budgetCategoryId)
    {
        try
        {
            var result = await _budgetCategoryService
                .RemoveBudgetCategory(budgetCategoryId);

            if (!result)
            {
                _logger.LogInformation("Budget category was removed successfully.");
                return Ok();
            }
        }
        
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error removing the budget category.");
        }
        
        return BadRequest();
    }
}