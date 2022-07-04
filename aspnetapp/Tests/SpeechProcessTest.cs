using System.Web;
using aspnetapp.Service.SpeechProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace aspnetapp.Tests;

[TestClass]
public class SpeechProcessTest
{
    [TestMethod]
    public void TestRecognizeSpeech()
    {
        string url = "https://636c-cloud1-8gbtq97y394b518b-1311389966.tcb.qcloud.la/records/2022/07/02%2010%3A41%3A54-37850.mp3?sign=2b765254c939b245c50f4a5f08e5e764&t=1656729958";
        var s = HttpUtility.UrlPathEncode(url);
        Console.WriteLine(s);
        var res = ASR.RecognizeSpeech(url, "1");
        Console.WriteLine(res);
    }

    [TestMethod]
    public void TestGetResult()
    {
        string url = "https://636c-cloud1-8gbtq97y394b518b-1311389966.tcb.qcloud.la/records/2022/07/02 15:45:22-75080.mp3";
        var s = ASR.GetSpeech(url, "2");
        Console.WriteLine(s);
    }

    [TestMethod]
    public void TestTalkInSleep()
    {
        string url =
            "https://636c-cloud1-8gbtq97y394b518b-1311389966.tcb.qcloud.la/records/2022/07/02 15:45:22-75080.mp3";
        var res = ASR.TalkInSleep(url, Guid.NewGuid().ToString());
        Console.WriteLine(res);
    }
}