using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;
using Service.Wav;

[TestClass]
public class WavTest
{
    [TestMethod]
    public void TestConvertMp3ToWav()
    {
        WavReader.ConvertMp3ToWav("https://636c-cloud1-8gbtq97y394b518b-1311389966.tcb.qcloud.la/records/2022/07/05%2016%3A22%3A14-16512.mp3?sign=b3082d58a4214403ad9bcc0109acc230&t=1657009344", "1.wav");
    }
}