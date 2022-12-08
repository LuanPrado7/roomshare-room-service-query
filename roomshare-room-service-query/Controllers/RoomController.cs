using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
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
        public async Task<ActionResult> Changed(RoomChangedRequest data)
        {
            var room = new Room()
            {
                Id = data.Id,
                Address = data.Address,
                CEP = data.CEP, 
                Description = data.Description, 
                LocatorKey = data.LocatorKey,   
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

        [HttpGet]
        public async Task<List<Room>> Get() => await _roomsService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get(long id)
        {
            var book = await _roomsService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }
    }
}