namespace aspnetapp.RequestAndResponse;
using Service.Wav;

public class VoiceResponse
{
    public bool talkInSleep { get; set; }
    public string speech { get; set; }
    public double db { get; set; } = -1;
    public VoiceKind voiceKind { get; set; } = VoiceKind.UNUSE;
    public string url { get; set; }

    public VoiceResponse()
    {
        
    }
    
    public VoiceResponse(bool talkInSleep, string speech, double db, VoiceKind voiceKind, string url)
    {
        this.talkInSleep = talkInSleep;
        this.speech = speech;
        this.db = db;
        this.voiceKind = voiceKind;
        this.url = url;
    }
}