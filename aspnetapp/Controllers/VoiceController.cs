using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers
{
    [Route("api/voice")]
    [ApiController]
    public class VoiceController:ControllerBase
    {
        [HttpGet]
        public bool getUseful()
        {
            return true;
        }
        

    }
}
