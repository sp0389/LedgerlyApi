using FinanceApi.Application.DTO;
using FinanceApi.Application.Interfaces;
using FinanceApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinanceApi.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<bool> RegisterUser(UserDto userDto)
    {
        var user = new User()
        {
            Email = userDto.Email,
            UserName = userDto.Email
        };
        
        var result = await _userManager.CreateAsync(user, userDto.Password);
        
        return result.Succeeded;
    }

    public async Task<string> GetJwtTokenForUser(UserDto userDto)
    {
        //TODO:
        throw new NotImplementedException();
    }
}