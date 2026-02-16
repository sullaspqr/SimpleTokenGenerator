using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleTokenGenerate
{
    public class GenerateToken
    {
        public string Token(string uname, int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Fontos: ugyanaz a kulcs legyen, mint a Program.cs-ben!
            var key = Encoding.UTF8.GetBytes("Ez egy 16 karakter hosszú szoveg legalább");

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, uname),
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Egyedi azonosító a tokennek
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Audience = "auth-client",
                Issuer = "auth-api",
                Subject = new ClaimsIdentity(claimList),
                IssuedAt = DateTime.UtcNow,        // <--- Itt adjuk hozzá az iat mezőt!
                Expires = DateTime.UtcNow.AddDays(1), // UtcNow használata javasolt
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
