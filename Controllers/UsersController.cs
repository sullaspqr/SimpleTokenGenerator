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
        private readonly GenerateToken _generateToken;
        private readonly SimpletokenContext _context;

        // A konstruktorban kérjük el a Context-et és a Token generátort is
        public UsersController(GenerateToken generateToken, SimpletokenContext context)
        {
            _generateToken = generateToken;
            _context = context;
        }
    public class LoginModel {
        public string UName { get; set; }
        public string Pass { get; set; }
    }
        [HttpPost("Login")]
        public ActionResult Login([FromBody], LoginModel model)
        {
            // Nincs 'using', a _context-et használjuk, amit a rendszertől kaptunk
            var user = _context.Users.FirstOrDefault(x => x.UserName == model.UName && x.Password == model.Pass);

            if (user != null)
            {
                // Feltételezve, hogy a GenerateToken.Token metódusod így néz ki
                return Ok(new { token = _generateToken.Token(user.UserName, user.Id) });
            }

            return Unauthorized("Érvénytelen felhasználónév vagy jelszó!");
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetAllUser()
        {
            // Itt is a beoltott _context-et használjuk
            return Ok(_context.Users.ToList());
        }
    }
}
