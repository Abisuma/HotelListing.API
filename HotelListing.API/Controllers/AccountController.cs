using HotelListing.API.DTOs.User;
using HotelListing.API.Repository;
using HotelListing.API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            _authManager = authManager;
             _logger = logger;
        }


        //Post: api/account/register
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody]APIUserDto userdto)
        {
            _logger.LogInformation($"Registration Attempt for {userdto.Email}");

            try
            {
                var errors = await _authManager.Register(userdto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok(userdto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something went wrong in the {nameof(Register)} - user attempt registration for {userdto.Email}");
                return Problem($"Something went wrong in the {nameof(Register)}, Please contact Support", statusCode: 500);
            }
           
        }

        [HttpPost]
        [Route("Register/Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> RegisterAdmin([FromBody] APIUserDto userdto)
        {
            _logger.LogInformation($"Registration Attempt for {userdto.Email}");

            try
            {
                var errors = await _authManager.Register(userdto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok(userdto);
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)} - user attempt registration for {userdto.Email}");
                return Problem($"Something went wrong in the {nameof(Register)}, Please contact Support", statusCode: 500);
            }

        }

        //Post: api/account/login
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginuserdto)
        {
            _logger.LogInformation($"Login Attempt for {loginuserdto.Email}");

            try
            {
                var authResponse = await _authManager.Login(loginuserdto);

                if (authResponse == null)
                {
                    return Unauthorized();
                }

                return Ok(authResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)} - user attempt login for {loginuserdto.Email}");
                return Problem($"Something went wrong in the {nameof(Login)}, Please contact Support", statusCode: 500);

            }
        }    
        
        
        //Post: api/account/refreshToken
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto responseDto)
        {
            var authResponse = await _authManager.VerifyRefreshToken(responseDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
    }
}
