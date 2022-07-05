using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Wav
{
    public class WavProcessor
    {
        public Dictionary<int, double> dataDic { get; set; }
        public WavProcessor(Dictionary<int,double> dic)
        {
            dataDic = dic;
        }

        public Dictionary<int,double> toDB(out double avgDB)
        {
            avgDB = 0;
            Dictionary<int,double> res = new Dictionary<int, double>();
            foreach(KeyValuePair<int,double> kvp in dataDic)
            {
                double db = 20 * Math.Log10((kvp.Value) );
                res.Add(kvp.Key, db);
                if(db>0)avgDB += db;
            }
            avgDB /= dataDic.Count;
            return res;
        }

        public Dictionary<float, float> toFrequency(out double avgFreq)
        {
            avgFreq = 0;
            float[] times = new float[dataDic.Count];
            float[] strength = new float[dataDic.Count];
            for(int i = 0; i < dataDic.Count; i++)
            {
                times[i] = Convert.ToSingle(i);
                strength[i] = Convert.ToSingle(dataDic[i]);
            }

            FFTUtil.FFT(times, strength);
            Dictionary<float,float> res = new Dictionary<float, float>();
            for(int i = 0; i < times.Length; i++)
            {
                res.Add(times[i], strength[i]);
            }
            return res;
        }
    }
}
