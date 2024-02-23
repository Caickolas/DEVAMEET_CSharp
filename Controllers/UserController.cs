using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DEVAMEET_CSharp.Controllers
{

    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<AuthController> logger, IUserRepository userRepository) : base(userRepository)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("api/[controller]")]
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


        [HttpPut]
        [Route("api/[controller]")]
        public IActionResult UpdateUser([FromBody] UserRequestDto userdto)
        {
            try
            {
                User user = GetToken();

                if (user != null)
                {

                    if (!String.IsNullOrEmpty(user.Name) && !String.IsNullOrEmpty(user.Avatar) &&
                    !String.IsNullOrWhiteSpace(user.Name) && !String.IsNullOrWhiteSpace(user.Avatar))
                    {
                        user.Avatar = userdto.Avatar;
                        user.Name = userdto.Name;

                        _userRepository.UpdateUser(user);

                        return Ok("Usuario salvo com sucesso!");
                    }
                    else
                    {
                        _logger.LogError("Os dados do usuario devem estar preenchidos corretamente!");
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto()
                        {
                            Description = "Os dados do usuario devem estar preenchidos corretamente!",
                            Status = StatusCodes.Status400BadRequest,

                        });
                    }
                }
                else
                {
                    _logger.LogError("Este usuario não é valido!");
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                    {
                        Description = "Este usuario não é valido!",
                        Status = StatusCodes.Status500InternalServerError,

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro na atualização dos dados do usuario: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro na atualização dos dados do usuario: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });

            }


        }

    }
}
