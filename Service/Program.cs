// See https://aka.ms/new-console-template for more information

using Service.SpeechProcess;
namespace Service.SpeechProcess
{
    public class Promgram
    {
        public static WavProcessor processor = new WavProcessor();
        
        public static void Main(string[] args)
        {
            DBReader reader = new DBReader();
            double sum = 0;
            foreach(var entity in reader.readDB("test.wav"))
            {
                Console.WriteLine(entity);
                sum += entity.Value;
            }
            Console.WriteLine(sum / reader.readDB("test.wav").Count());
        }
    }
}
