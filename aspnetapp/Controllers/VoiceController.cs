using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers
{
    [Route("api/voice")]
    [ApiController]
    public class VoiceController:ControllerBase
    {
        private readonly CounterContext _context;

        public VoiceController(CounterContext context)
        {
            _context = context;
        }
        [HttpGet]
        public bool getUseful()
        {
            return true;
        }
        

    }
}
