using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DEVAMEET_CSharp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MeetController : BaseController
    {
        private readonly ILogger<MeetController> _logger;
        private readonly IMeetRepository _meetRepository;
        private readonly IMeetObjectsRepository _meetObjectsRepository;

        public MeetController(ILogger<MeetController> logger, IUserRepository userRepository, IMeetRepository meetRepository, IMeetObjectsRepository meetObjectsRepository ) : base(userRepository)
        {
            _logger = logger;
            _meetRepository = meetRepository;
            _meetObjectsRepository = meetObjectsRepository;
        }


        [HttpGet]
        public IActionResult GetMeet()
        {
            try
            {
                User user = GetToken();

                List<Meet> meets = _meetRepository.GetMeetsByUser(user.Id);

                return Ok(meets);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro na busca das salas de reunião: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro na busca das salas de reunião: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }
        [HttpPost]
        public IActionResult CreateMeet([FromBody] MeetRequestDto meetRequestDto) 
        {
            try
            {   
                if (String.IsNullOrEmpty(meetRequestDto.Name) || String.IsNullOrWhiteSpace(meetRequestDto.Name))
                {
                    _logger.LogError("O nome da sala de reunião precisa ser preenchido: ");
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto()
                    {
                        Description = "O nome da sala de reunião precisa ser preenchido: ",
                        Status = StatusCodes.Status400BadRequest,

                    });

                }
                else
                {
                    Meet meet = new Meet();
                    meet.Name = meetRequestDto.Name;
                    meet.Color = meetRequestDto.Color;
                    meet.Link = Guid.NewGuid().ToString();
                    meet.UserId = GetToken().Id;

                    _meetRepository.CreateMeet(meet);

                    return Ok("Sala de Reunião criada com sucesso!");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao salvar a sala de reunião: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao salvar a sala de reunião: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }

        [HttpGet]
        [Route("objects")]
        public IActionResult GetMeet(int meetid)
        {
            try
            {

                return Ok(_meetObjectsRepository.GetObjectsByMeet(meetid));
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro na busca dos objetos da sala de reunião: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro na busca dos objetos da sala de reunião: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }

        [HttpPut]
        public IActionResult UpdateMeet([FromBody] MeetUpdateDto meetUpdateDto, int meetId) 
        { 
            try
            {
                Meet meet = _meetRepository.GetMeetsById(meetId);
                meet.Name = meetUpdateDto.Name;
                meet.Color = meetUpdateDto.Color.ToString();
                _meetRepository.UpdateMeet(meet);
                List<MeetObjects> meetObjects = new List<MeetObjects>();
                foreach (MeetObjectsDto objectsDto in meetUpdateDto.Objects)
                {
                    MeetObjects meetObj = new MeetObjects();
                    meetObj.Name = objectsDto.Name;
                    meetObj.Orientation = objectsDto.Orientation;
                    meetObj.X = objectsDto.X;
                    meetObj.Y = objectsDto.Y;
                    meetObj.ZIndex = objectsDto.ZIndex;
                    meetObj.Walkable = objectsDto.Walkable == null ? true : false;
                    meetObj.MeetId = meet.Id;
                    meetObjects.Add(meetObj);
                }

                _meetObjectsRepository.CreateObjectsMeet(meetObjects, meetId);


                return Ok("Sala de Reunião salva com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao atualizar a sala de reunião: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao atualizar a sala de reunião: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }

        [HttpDelete]
        public IActionResult DeleteMeet(int meetId)
        {
            try
            {
                _meetRepository.DeleteMeet(meetId); 
                return Ok("Sala deletada com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao deletar a sala de reunião: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao deletar a sala de reunião: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError,

                });
            }
        }

    }
}
