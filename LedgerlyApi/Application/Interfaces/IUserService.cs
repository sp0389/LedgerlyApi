using LedgerlyApi.Application.DTO;

namespace LedgerlyApi.Application.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUser(UserDto userDto);
    Task<string> GetJwtTokenForUser(UserDto userDto);
    Task<string> GetUserId(string email);
    string GetUserEmailFromToken(HttpContext httpContext);
}