using LedgerlyApi.Application.DTO;
using LedgerlyApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LedgerlyApi.Shared;
using Microsoft.AspNetCore.Identity;

namespace LedgerlyApi.API.Controllers;

[Authorize(Policy = "RequireMemberRole")]
[Route("api/[controller]")]
[ApiController]
public class BudgetCategoryController : ControllerBase
{
    private readonly IBudgetCategoryService _budgetCategoryService;
    private readonly ILogger<IBudgetCategoryService> _logger;
    private readonly IUserService _userService;


    public BudgetCategoryController(IBudgetCategoryService budgetCategoryService,
        ILogger<IBudgetCategoryService> logger, IUserService userService)
    {
        _budgetCategoryService = budgetCategoryService;
        _logger = logger;
        _userService = userService;
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
                var userEmail = _userService.GetUserEmailFromToken(HttpContext);
                var userId = await _userService.GetUserId(userEmail);

                var result = await _budgetCategoryService
                    .AddBudgetCategory(budgetCategory, userId);

                if (result)
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

            if (result)
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

    [HttpGet]
    [Route("/BudgetCategoryBalance")]
    public async Task<IActionResult> GetBudgetCategoryBalance(int budgetCategoryId)
    {
        try
        {
            var budgetCategory = await _budgetCategoryService.GetBudgetCategoryById(budgetCategoryId);
            var balance = await _budgetCategoryService.GetAvailableBudgetCategoryBalance(budgetCategory);

            return Ok(balance);
        }

        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            _logger.LogError(ex, "There was an error getting the budget category balance.");
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("BudgetCategoryTypes")]
    public IActionResult GetAvailableCategoryTypes()
    {
        var budgetCategoryTypes = _budgetCategoryService.GetAvailableCategoryTypes();
        return Ok(budgetCategoryTypes);
    }
}