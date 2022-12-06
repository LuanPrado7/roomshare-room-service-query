using Microsoft.AspNetCore.Mvc;
using roomshare_room_service_query.Model.Kafka;

namespace roomshare_room_service_query.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;

        public RoomController(ILogger<RoomController> logger)
        {
            _logger = logger;
        }

        [HttpPost("changed")]
        public ActionResult Post(RoomChangedRequest data)
        {   
            Console.WriteLine(data.Id);

            return Ok(data);
        }
    }
}