// See https://aka.ms/new-console-template for more information

using Service.SpeechProcess;
using Service.Wav;
namespace Service.Test
{
    public class Promgram
    {
        
        public static void Main(string[] args)
        {
            WavReader reader = new WavReader();
            Dictionary<int, double> dic = reader.readDB("test.wav");
            WavProcessor processor = new WavProcessor(dic);
            double avg = 0;
            Dictionary<int, double> DBdic = processor.toDB(out avg);
            Console.WriteLine("平均分贝为:"+avg);
            
            foreach(KeyValuePair<double, double> kvp in reader.readFrequency("test.wav"))
            {
                Console.WriteLine((int)kvp.Key+","+kvp.Value);
            }
        }
    }
}
