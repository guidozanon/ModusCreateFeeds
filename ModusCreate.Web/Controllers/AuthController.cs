using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModusCreate.Core.DAL.Domain;
using ModusCreate.Core.Models;
using ModusCreate.Core.Services;
using ModusCreate.Web.Models;
using ModusCreate.Web.Secutiry;
using System.Threading.Tasks;

namespace ModusCreate.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenGenerator _tokenGenerator;

        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AuthController(ITokenGenerator tokenGenerator, IMapper mapper, IUserService userService)
        {
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindByEmailAsync(login.Email);

                if (user != null)
                {
                    var result = await _userService.CheckPasswordSignInAsync(user, login.Password);

                    if (result.Succeeded)
                    {
                        var token = _tokenGenerator.Generate(_mapper.Map<User>(user));

                        _userService.SetCurrentUser(user.UserName);

                        return Ok(token);
                    }
                }
            }

            return BadRequest("Wrong username or password");
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenModel model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.RefreshToken))
            {
                var user = await _userService.GetUser(model.RefreshToken);

                if (user != null)
                {
                    var newToken = _tokenGenerator.Generate(user);

                    _userService.SetCurrentUser(user.Email);

                    await _userService.RevokeToken(model.RefreshToken, TokenType.RefreshToken);
                    await _userService.AddToken(newToken.Jwt, TokenType.Token);
                    await _userService.AddToken(newToken.RefreshToken, TokenType.RefreshToken);

                    return Ok(newToken);
                }
            }

            return BadRequest("Invalid refresh token");
        }
    }
}
