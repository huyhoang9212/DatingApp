using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text.Encodings;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const int V = 1;
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _authRepo = authRepo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto)
        {
            // validation here
            var userExist = await _authRepo.UserExists(userDto.Username);
            if(userExist) return BadRequest("The username is already taken.");

            var user = new User()
            {
                Username = userDto.Username            
            };
            
            
            user = await _authRepo.Register(user, userDto.Password);

            return StatusCode(201);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userDto)
        {
            var userFromDb = await _authRepo.Login(userDto.Username, userDto.Password);
            if(userFromDb == null) return Unauthorized();

            var claims = new []{
                new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromDb.Username)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new{
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}