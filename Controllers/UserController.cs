using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DEVAMEET_CSharp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public UserController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                User user = LerToken();

            }catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro na captura dos dados do usuario: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro na captura dos dados do usuario: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
            return null;
        }
    }
}
