using FinanceApi.Application.DTO;

namespace FinanceApi.Application.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUser(UserDto userDto);
    Task<string> GetJwtTokenForUser(UserDto userDto);
}