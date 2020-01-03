using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingADO.DAL.Auth;
using DatingADO.DTO;
using DatingADO.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingADO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _auth;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository auth,IConfiguration config)
        {
            _config = config;
            _auth = auth;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin loginDTO)
        {
            //validate the username
            loginDTO.Username = loginDTO.Username.ToLower();

            //get user from database
            User user = await _auth.Login(loginDTO.Username, loginDTO.Password);

            //check if user exist
            if (user == null)
                return Unauthorized("username or password doesn't match");

            //create JWT Token
            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username)
            };

            //create security key for credential
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token"]));

            //create credential
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //create descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token =tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });


        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister registerDTO)
        {
            //validate username
            registerDTO.Username = registerDTO.Username.ToLower();

            if (await _auth.UserExists(registerDTO.Username))
                return BadRequest("Username already exist");

            User user = new User()
            {
                Username = registerDTO.Username
            };

            try
            {
                //save data to database
                user = await _auth.Register(user, registerDTO.Password);
            }catch(Exception e)
            {
                return BadRequest("cannot register to database, please try again");
            }

            return StatusCode(201);
            
        }
        
    }
}