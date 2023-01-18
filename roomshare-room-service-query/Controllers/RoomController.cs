using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using roomshare_room_service_query.Integration;
using roomshare_room_service_query.Model;
using roomshare_room_service_query.Model.Kafka;
using roomshare_room_service_query.Services;

namespace roomshare_room_service_query.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly RoomsService _roomsService;

        public RoomController(RoomsService roomsService, ILogger<RoomController> logger)
        {
            _roomsService = roomsService;
            _logger = logger;
        }

        [HttpPost("changed")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Changed(RoomChangedRequest data)
        {
            var locator = await AuthIntegration.GetUserByGuid(data.LocatorKey);

            var room = new Room()
            {
                Id = data.Id,
                Address = data.Address,
                CEP = data.CEP, 
                Description = data.Description, 
                Locator = locator,   
                Name = data.Name,
                RoomKey = data.RoomKey
            };

            if(data.Method == "POST")
            {
                await _roomsService.CreateAsync(room);
            } else if(data.Method == "PUT")
            {
                await _roomsService.UpdateAsync(room.Id, room);
            }
            else if(data.Method == "DELETE")
            {
                await _roomsService.RemoveAsync(room.Id);
            }

            return Ok(data);
        }

        /// <summary>
        /// Buscar lista de salas comerciais cadastradas pelo usuario.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var token = Request.Headers["Authorization"];
            var user = await AuthIntegration.GetUser(token);

            if (user == null)
            {
                return Unauthorized("Usuário inválido, tente novamente.");
            }

            return Ok(await _roomsService.GetAsync(user.guid));
        }

        /// <summary>
        /// Buscar uma sala comercial cadastrada pelo usuario atraves do Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var token = Request.Headers["Authorization"];
            var user = await AuthIntegration.GetUser(token);

            if (user == null)
            {
                return Unauthorized("Usuário inválido, tente novamente.");
            }

            var book = await _roomsService.GetAsync(id, user.guid);

            if (book is null)
            {
                return NotFound();
            }

            return Ok(book);
        }
    }
}