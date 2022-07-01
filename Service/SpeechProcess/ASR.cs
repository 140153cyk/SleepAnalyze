using System.Net;

namespace Service.SpeechProcess;

using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Asr.V20190614;
using TencentCloud.Asr.V20190614.Models;

public class ASR
{
    public static string RecognizeSpeech(string url, string usrAudioKey, string voiceFormat = "mp3")
    {
        string encodedUrl = WebUtility.UrlEncode(url);
        Credential cred = new Credential
        {
            SecretId = "SecretId",
            SecretKey = "SecretKey"
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
        SentenceRecognitionResponse resp = client.SentenceRecognitionSync(req);
        
        return AbstractModel.ToJsonString(resp);
    }
}