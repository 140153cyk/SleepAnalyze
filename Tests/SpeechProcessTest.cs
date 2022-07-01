using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class SpeechProcessTest
{
    [TestMethod]
    public void TestMethod1()
    {
        string url = "https://www.baidu.com/search=ab2&";
        var s = WebUtility.UrlEncode(url);
        Console.Write(s);
    }
}