using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleTokenGenerate.Models;
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

            var key = Encoding.UTF8.GetBytes("Ez egy 16 karakter hosszú szoveg legalább");

            var claimList = new List<Claim>
                    {

                        new Claim(JwtRegisteredClaimNames.Name, uname),
                         new Claim(JwtRegisteredClaimNames.Sub, id.ToString())

                    };

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Audience = "auth-client",
                    Issuer = "auth-api",
                    Subject = new ClaimsIdentity(claimList),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescription);

                return tokenHandler.WriteToken(token);

            }

        }
    }
