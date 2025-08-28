using LedgerlyApi.Application.DTO;
using LedgerlyApi.Application.Interfaces;
using LedgerlyApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LedgerlyApi.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public UserService(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    
    public async Task<bool> RegisterUser(UserDto userDto)
    {
        var user = new User()
        {
            Email = userDto.Email,
            UserName = userDto.Email
        };

        var userResult =  await _userManager.CreateAsync(user, userDto.Password);

        if (userResult.Succeeded)
        {
            var roleResult = await AddUserToDefaultRole(user);

            if (!roleResult)
            {
                throw new ApplicationException("Could not add newly created user to default role.");
            }
        }

        return true;
    }

    private async Task<bool> AddUserToDefaultRole(User user)
    {
        var result = await _userManager.AddToRoleAsync(user, "Member");

        return result.Succeeded;
    }

    public async Task<string> GetJwtTokenForUser(UserDto userDto)
    {
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        
        if (user != null && await _userManager.CheckPasswordAsync(user, userDto.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            
            var claims = new List<Claim>
            {
                //TODO: Add more claims to token?
                new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };
            
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        return "Invalid Credentials!";
    }
}