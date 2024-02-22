using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DEVAMEET_CSharp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<AuthController> logger, IUserRepository userRepository) : base(userRepository)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                User user = GetToken();
                return Ok(new UserResponseDto
                {
                    Name = user.Name,
                    Email = user.Email, 
                    Avatar = user.Avatar,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro na captura dos dados do usuario: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro na captura dos dados do usuario: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }

        [HttpGet]
        [Route("api/[controller]/getuserbyid")]
        public IActionResult GetUserById(int iduser)
        {
            try
            {
                User user = _userRepository.GetUserByLogin(iduser);
                return Ok(new UserResponseDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro na captura dos dados do usuario: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro na captura dos dados do usuario: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }
    }
}
