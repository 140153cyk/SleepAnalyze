namespace aspnetapp.RequestAndResponse;

public class VoiceResponse
{
    public bool talkInSleep;

    public VoiceResponse()
    {
        
    }
    
    public VoiceResponse(bool talkInSleep)
    {
        this.talkInSleep = talkInSleep;
    }
}