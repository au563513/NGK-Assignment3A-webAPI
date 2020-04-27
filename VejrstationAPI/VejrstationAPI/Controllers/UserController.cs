using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VejrstationAPI.Data;
using VejrstationAPI.Models;


namespace VejrstationAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly VejrstationAPIContext _context;
        private readonly UserManager<VejrstationAPIUser> _userManager;
        private readonly SignInManager<VejrstationAPIUser> _signInManager;


        public AccountController(VejrstationAPIContext context, UserManager<VejrstationAPIUser> userManager, SignInManager<VejrstationAPIUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] string email, [FromBody] string password, [FromBody] string name)
        public async Task<IActionResult> Register([FromBody] DtoUser dtoUser)
        {
            var newUser = new VejrstationAPIUser
            {
                UserName = dtoUser.Email,
                Email = dtoUser.Email,
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, dtoUser.Password);
            if (userCreationResult.Succeeded)
            {
                newUser.EmailConfirmed = true;
                await _context.SaveChangesAsync();
                return Ok(newUser);
            }
            foreach (var error in userCreationResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]DtoUser dtoUser)
        {
            //var user = await _userManager.FindByEmailAsync(email);
            //if (user == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid login");
            //    return BadRequest(ModelState);
            //}

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(dtoUser.Email, dtoUser.Password, isPersistent: false, lockoutOnFailure: false);
            if (passwordSignInResult.Succeeded)
            {
                return Ok();
            }

            ModelState.AddModelError(string.Empty, "Invalid login");
            return BadRequest(ModelState);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("jwtlogin")]
        public async Task<IActionResult> JWTlogin([FromBody]DtoUser dtoUser)
        {
            var user = await _userManager.FindByEmailAsync(dtoUser.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return BadRequest(ModelState);
            }
            var passwordSignInResult = await _signInManager.CheckPasswordSignInAsync(user, dtoUser.Password, false);
            if (passwordSignInResult.Succeeded)
                return new ObjectResult(GenerateToken(dtoUser.Email));
            return BadRequest("Invalid login");
        }

        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256")),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
