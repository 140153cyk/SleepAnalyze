using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using aspnetapp.RequestAndResponse;
using aspnetapp.Service.SpeechProcess;

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
        public ActionResult<VoiceResponse> GetTalkInSleep([FromBody] VoiceRequest data)
        {
            var url = data.url;
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("Url is empty or consists of only whitespaces.");
            }

            try
            {
                ASR asr = new ASR();
                var speech = ASR.GetSpeech(url, Guid.NewGuid().ToString());
                var isTalk = ASR.TalkInSleep(speech);
                return new VoiceResponse(isTalk, speech);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message + '\n' + e.StackTrace);
            }
        }
    }
}