using Microsoft.AspNetCore.Mvc;
using aspnetapp.RequestAndResponse;
using Service.SpeechProcess;
using Service.Wav;

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

            //获取北京时间
            DateTime time = DateTime.UtcNow.AddHours(8);
            string currentTime = time.Year + "-" + time.Month + "-" + time.Day + "-" + time.Hour + "-" + time.Minute +
                                 "-" + time.Second;
            //将mp3格式文件转为wav
            string wavFilePath = "./WavFiles/" + currentTime + ".wav";
            WavReader.ConvertMp3ToWav(url, wavFilePath);

            var kind = WavProcessor.judgeFre(wavFilePath);
            //获取音频文件的平均分贝
            var avgDb = WavProcessor.getAvgDb(WavReader.readDB(wavFilePath));


            //删除录音文件
            if (System.IO.File.Exists(wavFilePath))
            {
                System.IO.File.Delete(wavFilePath);
            }
            else throw new Exception("Audio file to delete not found.");

            if(avgDb < 25)
                return new VoiceResponse(false, "", avgDb, VoiceKind.UNUSE, "");
            
            switch (kind)
            {
                case VoiceKind.UNUSE:
                    return new VoiceResponse(false, "", avgDb, VoiceKind.UNUSE, "");
                   

                case VoiceKind.SPEECH:
                    try
                    {
                        ASR asr = new ASR();
                        var speech = ASR.GetSpeech(url, currentTime);
                        var isTalk = ASR.TalkInSleep(speech);
                        VoiceKind voiceKind = VoiceKind.UNUSE;

                        if (isTalk)
                        {
                            voiceKind = VoiceKind.SPEECH;
                        }

                        return new VoiceResponse(isTalk, speech, avgDb, voiceKind, url);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return BadRequest(e.Message + '\n' + e.StackTrace);
                    }

                // case VoiceKind.SNORE:
                //     return new VoiceResponse(false, "", avgDb, VoiceKind.SNORE, url);
            }

            return BadRequest("Error");
        }
    }
}