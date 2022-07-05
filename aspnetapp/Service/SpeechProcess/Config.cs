namespace Service.SpeechProcess;

public class Config
{
    /*
	 * Please do not commit the secrets to the repository.
	 */

    public static readonly string SECRET_ID = Environment.GetEnvironmentVariable("SECRET_ID");
	
    public static readonly string SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY");
}