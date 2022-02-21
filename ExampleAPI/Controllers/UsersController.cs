using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ExampleAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext _context;

        private readonly JWTSettings _jwtSettings;
        public UsersController(EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtSettings = jwtsettings.Value;
        }

        [Authorize]
        [HttpGet("Check")]
        public async Task<ActionResult<Users>> GetUsers()
        {
            string login = HttpContext.User.Identity.Name;
            var user = await _context.Users.Where(user => user.Login==login).FirstOrDefaultAsync();
            if (user==null)
                return NotFound();
            return Ok(user);
        }


        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost ("Login")]
        public async Task<ActionResult<Users>> Login( [FromBody] Users user)
        {
            var userdb = await _context.Users.Where(u => u.Login == user.Login).FirstOrDefaultAsync();
            if (user==null || ! (user.Password == userdb.Password))
                return BadRequest("Wron password or login");

            UserWithTocken result = new UserWithTocken(userdb);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userdb.Login)
                }),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Token = tokenHandler.WriteToken(token);

            return Ok(result);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }
    }
}
