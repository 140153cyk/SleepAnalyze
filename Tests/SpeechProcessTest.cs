using System;
using System.Net;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.SpeechProcess;

namespace Tests;

[TestClass]
public class SpeechProcessTest
{
    [TestMethod]
    public void TestMethod1()
    {
        string protocol = "https://";
        string url = "https://636c-cloud1-8gbtq97y394b518b-1311389966.tcb.qcloud.la/records/2022/07/02%2010%3A41%3A54-37850.mp3?sign=2b765254c939b245c50f4a5f08e5e764&t=1656729958";
        var s = HttpUtility.UrlPathEncode(url);
        Console.Write(s);
        var res = ASR.RecognizeSpeech(url, "1");
        Console.Write(res);
    }
    
    
}