using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Wav
{
    public class WavProcessor
    {
        public enum VoiceKind{ SPEECH , UNUSE , SNORE }
        /**
         * 获取rawdataDic中各个时间段的分贝大小
         */ 
        public static Dictionary<int,double> toDB(Dictionary<int, double> dataDic)
        {
            Dictionary<int,double> res = new Dictionary<int, double>();
            foreach(KeyValuePair<int,double> kvp in dataDic)
            {
                double db = 20 * Math.Log10((kvp.Value) );
                res.Add(kvp.Key, db);
            }
            return res;
        }

        /**
         * 获取rawdataDic中的平均分贝
         */ 
        public static double getAvgDb(Dictionary<int, double> dataDic)
        {
            Dictionary<int, double> dbDic = toDB(dataDic);
            double total = 0;
            foreach(KeyValuePair<int,double> kvp in dbDic)
            {
                if(kvp.Value > 0)total += kvp.Value;
            }
            return total / dbDic.Count;
        }

        /**
         * 获取一段音频中小于4000Hz与小于700Hz的能量占比
         */ 
        public static void getMainFrequency(Dictionary<double, double> freDic,out double lessThanFTR, out double lessThanSHR)
        {
            double sum = 0;
            double lessThanFT = 0;
            double lessThanSH = 0;
            foreach(KeyValuePair<double, double> kvp in freDic)
            {
                sum += kvp.Value;
                if (kvp.Key > 4000) break;
                lessThanFT += kvp.Value;
                if (kvp.Key > 700) break;
                lessThanSH += kvp.Value;
            }
            lessThanFTR = lessThanFT / sum;
            lessThanSHR = lessThanSH / sum;
        }

        /**
         * 根据频率判断音频种类
         */ 
        public static VoiceKind judgeFre(Dictionary<double, double> freDic)
        {
            getMainFrequency(freDic, out double lessThanFTR, out double lessThanSHR);
            if (lessThanSHR > 0.5) return VoiceKind.SNORE;
            if (lessThanFTR > 0.5) return VoiceKind.SPEECH;
            return VoiceKind.UNUSE;
        }

    }
}
