using LedgerlyApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LedgerlyApi.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IUserService _userService;

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var userEmail = _userService.GetUserEmailFromToken(HttpContext);
            return await _userService.GetUserId(userEmail);
        }
    }
}