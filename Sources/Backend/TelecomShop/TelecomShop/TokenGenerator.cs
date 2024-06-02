using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TelecomShop.DBModels;

namespace TelecomShop
{
    public interface ITokenGenerator
    {
        string CreateToken(User user);
    }
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.MobilePhone, user.Msisdn),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             };
            var tokenSettings = _configuration.GetSection("TokenSettings");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    tokenSettings.GetValue<string>("Key")));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                audience: tokenSettings.GetValue<string>("Audience"),
                issuer: tokenSettings.GetValue<string>("Issuer"),
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddYears(1),
                                   signingCredentials: cred
   );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
