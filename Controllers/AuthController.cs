using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using DEVAMEET_CSharp.Service;
using DEVAMEET_CSharp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DEVAMEET_CSharp.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository, IConfiguration configuration)
        {  
            _logger = logger; 
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/[controller]/login")]
        public IActionResult ExecuteLogin([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                if (!String.IsNullOrEmpty(loginRequestDto.Login) && !String.IsNullOrEmpty(loginRequestDto.Password) && 
                    !String.IsNullOrWhiteSpace(loginRequestDto.Login) && !String.IsNullOrWhiteSpace(loginRequestDto.Password))
                {
                    User user = _userRepository.GetUserByLoginPassword(loginRequestDto.Login.ToLower(),MD5Utils.GenerateHashMD5(loginRequestDto.Password));

                    if (user != null)
                    {
                        return Ok(new LoginResponseDto()
                        {
                            Email = user.Email,
                            Name = user.Name,
                            Token = TokenService.CreateToken(user, _configuration["JWT:SecretKey"])
                        });
            }
                    else
                    {
                        return BadRequest(new ErrorResponseDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Description = "Usuario e/ou senha são invalidos"
                        });
                    }
                }
                else
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Description = "Campos de login e senha estão preenchidos incorretamente"
                    });
                }

            }catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro no login: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro no login: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/[controller]/register")]
        public IActionResult SaveUser([FromBody] UserRegisterDto userdto )
        {
            try
            {
                if (userdto != null) 
                {
                    var errors = new List<string>();

                    if (string.IsNullOrEmpty(userdto.name) || string.IsNullOrWhiteSpace(userdto.name))
                    {
                        errors.Add("Nome Invalido");
                    }

                    if (string.IsNullOrEmpty(userdto.email) || string.IsNullOrWhiteSpace(userdto.email) || !userdto.email.Contains("@"))
                    {
                        errors.Add("Email Invalido");
                    }

                    if (string.IsNullOrEmpty(userdto.password) || string.IsNullOrWhiteSpace(userdto.password))
                    {
                        errors.Add("Senha Invalida");
                    }

                    if (errors.Count > 0) 
                    {
                        foreach (var error in errors)
                        {
                            _logger.LogError(error);
                        }

                        return BadRequest(new ErrorResponseDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Description = "Erros encontrados na requisição",
                            Errors = errors
                        });                     

                    }

                    User user = new User()
                    {
                        Email = userdto.email.ToLower(),
                        Password = MD5Utils.GenerateHashMD5(userdto.password),
                        Name = userdto.name,
                        Avatar = userdto.avatar
                    };

                    if (!_userRepository.VerifyEmail(user.Email))
                    {
                        _userRepository.Save(user);
                    }
                    else
                    {
                        _logger.LogError("Usuario já cadastrado");
                        return BadRequest("Usuario já cadastrado");
                    }


                }
            else 
            {
                _logger.LogError("O usuario para ser cadastrado está vazio");
                return BadRequest("O usuario para ser cadastrado está vazio");
            }

                return (Ok("Usuario Salvo com Sucesso"));

            }catch (Exception ex) 
            {
                _logger.LogError("Ocorreu o seguinte erro: " +  ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }
    }
}
