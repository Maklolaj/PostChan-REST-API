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

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "login/password combiantion is invalid" }
                };
            }

            return GenerateAuthenticationResultForUser(user);

        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existtingUser = await _userManager.FindByEmailAsync(email);

            if (existtingUser != null)
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

            return GenerateAuthenticationResultForUser(newUser);

        }


        private AuthenticationResult GenerateAuthenticationResultForUser(IdentityUser user)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = ascii.GetBytes(_jwtSettigns.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)  //Custom claim
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
