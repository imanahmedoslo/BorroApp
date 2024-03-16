using BorroApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BorroApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BorroApp.Controller.Unauthorized
{
    [Route("api/[controller]")]
    [ApiController] 
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly BorroDbContext _context;
        public LoginController(IConfiguration config, BorroDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost()]
        public async Task <IActionResult> Post([FromBody] LoginRequest loginRequest)
        {
          User? user= await _context.User.Include(x => x.UserInfo).FirstOrDefaultAsync(user=>user.Email == loginRequest.Email && user.Password==loginRequest.Password);
            if (user == null)
            {
                return NotFound();
            }
            

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              new List<Claim>() { new Claim("address", user.UserInfo?.Address??""), new Claim("id", user.Id.ToString()) },
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials); ;

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(new
            {
                AccessToken = token,
                ExpiresAt = Sectoken.ValidTo, 
            });
        }
    }
}
