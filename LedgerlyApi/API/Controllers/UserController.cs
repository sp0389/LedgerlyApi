using LedgerlyApi.Application.DTO;
using LedgerlyApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LedgerlyApi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _userService.RegisterUser(userDto);

                if (result)
                {
                    _logger.LogInformation("User was registered successfully");
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex, "An error occured while registering user");
            }
        }
        return BadRequest();
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(UserDto userDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var token = await _userService.GetJwtTokenForUser(userDto);

                if (token != "Invalid Credentials!")
                {
                    return Ok(token);
                }
                
                return BadRequest(token);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.LogError(ex,"There was an error attempting to get login credentials.");
            }
        }

        return BadRequest();
    }

    // check if the token isn't expired - will return 401 if it is
    [Route("validate")]
    public IActionResult ValidateUserToken()
    {
        return Ok();
    }
}