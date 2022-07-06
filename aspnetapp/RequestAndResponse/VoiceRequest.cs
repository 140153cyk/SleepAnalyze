namespace aspnetapp.RequestAndResponse;

public class VoiceRequest
{
    public string url { get; set; } = "";
    public string audioFormat { get; set; } = "";

    public VoiceRequest()
    {
        
    }

    public VoiceRequest(string url, string audioFormat)
    {
        this.url = url;
        this.audioFormat = audioFormat;
    }
}