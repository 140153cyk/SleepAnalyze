using Microsoft.AspNetCore.Mvc;
using aspnetapp.RequestAndResponse;
using Service.SpeechProcess;

namespace aspnetapp.Controllers
{
    [Route("api/voice")]
    [ApiController]
    public class VoiceController : ControllerBase
    {
        private readonly CounterContext _context;

        public VoiceController(CounterContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<VoiceResponse> GetTalkInSleep(VoiceRequest voiceRequest)
        {
            var url = voiceRequest.url;
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("Url is empty or consists of only whitespaces.");
            }

            try
            {
                var res = ASR.TalkInSleep(url);
                return new VoiceResponse(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}