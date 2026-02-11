using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleTokenGenerate.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleTokenGenerate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GenerateToken generateToken;
        public UsersController(GenerateToken generateToken)
        {
            this.generateToken = generateToken;
        }

        [HttpPost]
        public ActionResult Login(string UName, string Pass)
        {
            using (var context = new SimpletokenContext())
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == UName && x.Password == Pass);

                if (user != null)
                {
                    return Ok( new { token = generateToken.Token(user.UserName, user.Id)});
                }

                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetAllUser()
        {
            using (var context = new SimpletokenContext())
            {
                return Ok(context.Users.ToList());
            }
        }

    } 
}
