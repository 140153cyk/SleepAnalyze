﻿using NAudio.Wave;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.Audio.Windows;
using Accord.Math;
using System.Numerics;

namespace Service.Wav
{
    public class WavReader
    {
        /**
         * 把mp3问价转换为wav文件，存储在wavfile指定的路径
         */ 
        public static void ConvertMp3ToWav(string mp3file, string wavfile)
        {
            using (MediaFoundationReader reader = new MediaFoundationReader(mp3file))
            {
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(wavfile, pcmStream);
                }
            }
        }

        /**
         * 读取wav文件，获取每十毫秒中的平均数据，可以由WavProcessor处理
         */ 
        public static Dictionary<int, double> readDB(string filePath)
        {
            var voiceDict = new Dictionary<int, double>();
            using (WaveFileReader wave = new WaveFileReader(filePath))
            {
                var bytesPerSample = wave.WaveFormat.BitsPerSample / 8;
                var samplesPerSecond = wave.WaveFormat.SampleRate * wave.WaveFormat.Channels/1000;
                var bytesPerSecond = samplesPerSecond * bytesPerSample;
                var readBuffer = new byte[bytesPerSecond];
                int samplesRead;
                int i = 0;
                do
                {
                    samplesRead = wave.Read(readBuffer, 0, bytesPerSecond);
                    if (samplesRead == 0) break;
                    double avg = 0;
                    for(int j=0; j < samplesPerSecond; j++)
                    {
                        //每十毫秒内的平均数据
                        avg +=Math.Abs(BitConverter.ToInt16(readBuffer,bytesPerSample*j) * 1.0 / (samplesRead/bytesPerSample));
                    }
                    voiceDict.Add(i, avg);

                    i++;
                } while (samplesRead > 0);
            }
            return voiceDict;
        }

        /**
         * 从wav文件中读取各个频段的能量占比，可以用WavProcessor中换算成说话频段和呼噜频段的占比
         */ 
        public static Dictionary<double, double> readFrequency(string filePath)
        {
            Dictionary<double, double> frequencyDict=new Dictionary<double, double>();

            WaveDecoder sourceDecoder = new WaveDecoder(filePath);
            Signal sourceSignal = sourceDecoder.Decode();

            //Create Hamming window so that signal will fit into power of 2:           
            RaisedCosineWindow window = RaisedCosineWindow.Hamming(1024);

            // Splits the source signal by walking each 512 samples, then creating 
            // a 1024 sample window. Note that this will result in overlapped windows.
            Signal[] windows = sourceSignal.Split(window, 512);

            // You might need to import Accord.Math in order to call this:
            ComplexSignal[] complex = windows.Apply(ComplexSignal.FromSignal);

            // Forward to the Fourier domain
            complex.ForwardFourierTransform(); 
            
            for(int i = 0; i < complex.Length; i++)
            {
                Complex[,] complexes = complex[i].ToArray();
                for(int j = 0; j < complex[i].Length/2; j++)
                {
                    var addEnergy = complexes[j, 0].SquaredMagnitude() + complexes[j, 1].SquaredMagnitude();
                    if (frequencyDict.TryGetValue(Index2Freq(j, sourceSignal.SampleRate, 1024), out double before)){
                        frequencyDict[Index2Freq(j, sourceSignal.SampleRate, 1024)] = before + addEnergy;
                    }
                    else frequencyDict.Add(Index2Freq(j, sourceSignal.SampleRate, 1024), addEnergy);

                }
            }
            sourceDecoder.Close();
            return frequencyDict;
        }

        public static double Index2Freq(int i, double samples, int nFFT)
        {
            return (double)i * (samples / nFFT);
        }

    }
}