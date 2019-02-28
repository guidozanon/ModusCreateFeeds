using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModusCreate.Core.Models;
using ModusCreate.Core.Services;
using ModusCreate.Web.Configuration;
using ModusCreate.Web.Models;
using ModusCreate.Web.Secutiry;
using System.Threading.Tasks;

namespace ModusCreate.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<GlobalConfiguration> _config;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _jwtTokenGenerator;

        public UserController(IUserService userService, IOptions<GlobalConfiguration> config, IMapper mapper
            , ITokenGenerator jwtTokenGenerator)
        {
            _config = config;
            _mapper = mapper;
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel signup)
        {
            if (ModelState.IsValid)
            {
                var newUser = _mapper.Map<User>(signup);

                newUser.AvatarUrl = signup.Email.GenerateImageUrl();

                var result = await _userService.CreateAsync(newUser, signup.Password);
                if (result.Succeeded)
                {
                    var token = _jwtTokenGenerator.Generate(_mapper.Map<User>(newUser));

                    _userService.SetCurrentUser(newUser.UserName);

                    return Ok(token);
                }
                else
                {
                    return StatusCode(400, result.ToString());
                }
            }

            return base.StatusCode(400, ModelState);
        }
    }
}
