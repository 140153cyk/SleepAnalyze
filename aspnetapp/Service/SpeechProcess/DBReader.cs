namespace aspnetapp.Service.SpeechProcess
{
    public class DBReader
    {
        public Dictionary<int, double> readDB(string filePath)
        {
            var silenceDict = new Dictionary<int, double>();
            using (NAudio.Wave.WaveFileReader wave = new NAudio.Wave.WaveFileReader(filePath))
            {
                var samplesPerSecond = wave.WaveFormat.SampleRate * wave.WaveFormat.Channels/10;
                var bytesPerSecond = samplesPerSecond * 2;
                var readBuffer = new byte[bytesPerSecond];
                int samplesRead;
                int i = 1;
                do
                {
                    samplesRead = wave.Read(readBuffer, 0, bytesPerSecond);
                    if (samplesRead == 0) break;
                    double avg = 0;
                    for(int j=0; j < samplesPerSecond; j++)
                    {
                        avg += BitConverter.ToInt16(readBuffer,2*j) * 1.0 / samplesPerSecond;
                    }
                    silenceDict.Add(i,Math.Abs(avg));
                    i++;
                } while (samplesRead > 0);
            }
            return silenceDict;

        }
        private int bytArray2Int(byte[] bytArray)
        {
            return bytArray[0] | (bytArray[1] << 8);
        }

    }
}