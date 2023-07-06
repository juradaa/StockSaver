using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRO_APBD.Server.Models;
using PRO_APBD.Server.Repositories;
using PRO_APBD.Server.Services;
using PRO_APBD.Shared.DTOs;

namespace PRO_APBD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _users;
        private readonly ITokenService _tokenService;

        public AuthController(IUsersRepository users, ITokenService tokenService)
        {
            _users = users;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserAuthDto userDto)
        {
            var user = await _users.GetUser(userDto.Username);
            if(user == null)
            {
                return Unauthorized("Wrong username or password");

            }
            var passwordHasher = new PasswordHasher<User>();
            var isValidPassword =  passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Success;
            if(!isValidPassword) 
            {
                return Unauthorized("Wrong username or password");
            }
            var stringifiedToken = _tokenService.GenerateToken(userDto.Username);
            var response = new LoginResponseDto
            {
                Token = stringifiedToken,
            };
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            if(userDto.Password != userDto.RepeatPassword)
            {
                return BadRequest("Passwords do not match");
            }

            var conflictingUser = await _users.GetUser(userDto.Username);
            if(conflictingUser is not null) 
            {
                return Conflict($"username {userDto.Username} is taken");
            }

            var passwordHasher = new PasswordHasher<User>();
            var hashed = passwordHasher.HashPassword(new User(),userDto.Password);
            var user = new User()
            {
                Username = userDto.Username,
                PasswordHash = hashed,
                Email = userDto.Email,
            };
            await _users.AddUser(user);
            return Ok();
        }


    }
}
