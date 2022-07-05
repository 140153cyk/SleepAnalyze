using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Service.SpeechProcess;

using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Asr.V20190614;
using TencentCloud.Asr.V20190614.Models;

public class ASR
{
    public static string RecognizeSpeech(string url, string usrAudioKey, string voiceFormat = "m4a")
    {
        string encodedUrl = HttpUtility.UrlPathEncode(url);
        // string encodedUrl = url;
        Credential cred = new Credential
        {
            SecretId = Config.SECRET_ID,
            SecretKey = Config.SECRET_KEY
        };

        ClientProfile clientProfile = new ClientProfile();
        HttpProfile httpProfile = new HttpProfile();
        httpProfile.Endpoint = ("asr.tencentcloudapi.com");
        clientProfile.HttpProfile = httpProfile;

        AsrClient client = new AsrClient(cred, "", clientProfile);
        SentenceRecognitionRequest req = new SentenceRecognitionRequest();
        req.ProjectId = 0; // 腾讯云项目Id
        req.SubServiceType = 2; // 云语音识别子服务类型 2：一句话识别
        req.EngSerViceType = "16k_zh"; // 引擎模型类型  16k 中文普通话通用
        req.SourceType = 0; // 语音数据来源。0：语音 URL
        req.Url = encodedUrl; // 语音 URL，需进行urlencode编码
        req.VoiceFormat = voiceFormat; // 识别音频的音频格式
        req.UsrAudioKey = usrAudioKey; // 用户端对此任务的唯一标识，用户自助生成，用于用户查找识别结果。
        req.FilterPunc = 2; // 是否过滤标点符号。 0：不过滤，1：过滤句末标点，2：过滤所有标点。
        SentenceRecognitionResponse resp = client.SentenceRecognitionSync(req);
        
        return AbstractModel.ToJsonString(resp);
    }

    public static string GetSpeech(string url, string usrAudioKey, string voiceFormat = "m4a")
    {
        string resp = RecognizeSpeech(url, usrAudioKey, voiceFormat);
        var jo = JsonConvert.DeserializeObject(resp) as JObject;
        string speech = "NULL";
        if (jo != null)
        {
            speech = jo["Result"].ToString();
        }
        return speech;
    }

    public static bool TalkInSleep(string url, string usrAudioKey, string voiceFormat = "m4a")
    {
        string speech = GetSpeech(url, usrAudioKey, voiceFormat);
        Regex regex = new Regex(@"嗯+");
        if (regex.IsMatch(speech) || string.IsNullOrWhiteSpace(speech)) return false;
        return true;
    }

    public static bool TalkInSleep(string speech)
    {
        Regex regex = new Regex(@"嗯+");
        if (regex.IsMatch(speech) || string.IsNullOrWhiteSpace(speech)) return false;
        return true;
    }
}