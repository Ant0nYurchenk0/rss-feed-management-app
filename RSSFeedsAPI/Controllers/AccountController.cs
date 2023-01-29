using System.Security.Cryptography;
using System.Text;
using RSSFeedsAPI.Data;
using RSSFeedsAPI.DTOs;
using RSSFeedsAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSSFeedsAPI.Interfaces;

namespace RSSFeedsAPI.Controllers;

public class AccountController : BaseApiController
{
  private readonly ITokenService _tokenService;
  private readonly DataContext _context;
  public AccountController(DataContext context, ITokenService tokenService)
  {
    _tokenService = tokenService;
    _context = context;
  }

  [HttpPost("register")]
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (await UserExists(registerDto.Username))
    {
      return BadRequest("username is taken");
    }
    using var hmac = new HMACSHA512();
    var user = new AppUser
    {
      UserName = registerDto.Username.ToLower(),
      PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(registerDto.Password)),
      PasswordSalt = hmac.Key
    };
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return new UserDto
    {
      Username = user.UserName,
      Token = _tokenService.CreateToken(user)
    };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
    if (user == null) return Unauthorized("invalid username");
    using var hmac = new HMACSHA512(user.PasswordSalt);
    var computedHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(loginDto.Password));
    for (var i = 0; i < computedHash.Length; i++)
    {
      if (computedHash[i] != user.PasswordHash[i])
      {
        return Unauthorized("invalid password");
      }
    }
    return new UserDto
    {
      Username = user.UserName,
      Token = _tokenService.CreateToken(user)
    };
  }
  private async Task<bool> UserExists(string username)
  {
    return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
  }
}
