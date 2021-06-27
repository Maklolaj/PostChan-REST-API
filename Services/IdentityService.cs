using PostChan.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PostChan.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace PostChan.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettigns;
     

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettigns)
        {
            _userManager = userManager;
            _jwtSettigns = jwtSettigns;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {

            var existtingUser = await _userManager.FindByEmailAsync(email);

            if(existtingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email adress already exists" }
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            ASCIIEncoding ascii = new ASCIIEncoding();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = ascii.GetBytes(_jwtSettigns.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Succes = true,
                Token = tokenHandler.WriteToken(token)
            };


        }
    }
}
