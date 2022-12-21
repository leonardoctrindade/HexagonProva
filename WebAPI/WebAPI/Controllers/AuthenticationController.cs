
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class AuthenticationController : ControllerBase
    {

        private readonly DbContextClass _context;

        public AuthenticationController(DbContextClass context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login usuarioData)
        {
            if (usuarioData is null)
            {
                return BadRequest("Usuário ou Senha Inválidos.");
            }

            var userDataResult = _context.Logins.Where(x => x.Usuario == usuarioData.Usuario && x.Senha == usuarioData.Senha);

            if (!userDataResult.Any())
            {
                return Unauthorized();
            }
            else
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse { Token = tokenString });
            }
           
        }



        [HttpGet]
        [Route("LoginsList")]
        public async Task<ActionResult<IEnumerable<Login>>> Get()
        {
            try
            {
                var data = await _context.Logins.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar as Logins - " + ex.Message);
            }

        }



        [HttpGet]
        [Route("LoginsDetail")]
        public async Task<ActionResult<Login>> Get(int id, ActionResult<Login> data)
        {
            try
            {
                var resultData = await _context.Logins.FindAsync(id);

                if (resultData == null)
                    return NotFound();

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Listar as Logins - " + ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("CreateLogin")]
        public async Task<ActionResult<Login>> POST(Login data)
        {
            try
            {
                if (data == null)
                {
                    return NotFound();
                }

                _context.Logins.Add(data);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = data.LoginId }, data);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Criar a Login - " + ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteLogin")]
        public async Task<ActionResult<IEnumerable<Login>>> Delete(int id)
        {
            try
            {
                var data = await _context.Logins.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _context.Logins.Remove(data);
                await _context.SaveChangesAsync();

                return await _context.Logins.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Deletar a Login - " + ex.Message);
            }
        }


        [HttpPost]
        [Route("UpdateLogin")]
        public async Task<ActionResult<IEnumerable<Login>>> Update(int id, Login data)
        {
            try
            {
                if (id != data.LoginId)
                {
                    return BadRequest();
                }

                var resultData = await _context.Logins.FindAsync(id);
                if (resultData == null)
                {
                    return NotFound();
                }

                resultData.Usuario = data.Usuario;
                resultData.Senha = data.Senha;

                await _context.SaveChangesAsync();
                return await _context.Logins.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Atualizar a Login - " + ex.Message);
            }
        }



    }
}
