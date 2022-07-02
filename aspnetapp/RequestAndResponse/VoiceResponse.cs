namespace aspnetapp.RequestAndResponse;

public class VoiceResponse
{
    public bool talkInSleep { get; set; }
    public string speech { get; set; }

    public VoiceResponse()
    {
        
    }
    
    public VoiceResponse(bool talkInSleep, string speech)
    {
        this.talkInSleep = talkInSleep;
        this.speech = speech;
    }
}