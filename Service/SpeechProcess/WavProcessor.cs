using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SpeechProcess
{
    public class WavProcessor
    {
        public string Id;      //以'RIFF'为标识
        public double Size;    //整个文件长度减去Id和Size的长度
        public string Type;    //Type是WAVE表示后面需要两个子块：Format区块和Data区块

        public string formatId;    //文件标识
        public double formatSize;      //表示该区块数据的长度（不包含Id和Size）
        public int formatTag;          //音频数据格式，PCM音频数据的值为1
        public int num_Channels;       //声道数目  1：单声道 2：双声道
        public int SamplesPerSec;      //采样率
        public int AvgBytesPerSec;     //每秒数据字节数 = SamplesPerSec * num_Channels * BitsPerSample / 8
        public int BlockAlign;         //数据块对齐单位(每个采样需要的字节数) 
        public int BitsPerSample;      //每个采样需要的bit数
        public string additionalInfo;  //附加信息

        public string dataId;          //以'data'为标识
        public int dataSize;           //音频数据的长度=AvgBytesPerSec * seconds
        public List<byte> wavdata = new List<byte>();

        public void ReadWAVFile(string filePath)  //读取文件
        {
            if (filePath == "") return;
            byte[] id = new byte[4];
            byte[] size = new byte[4];
            byte[] type = new byte[4];

            byte[] formatid = new byte[4];
            byte[] formatsize = new byte[4];
            byte[] formattag = new byte[2];
            byte[] numchannels = new byte[2];
            byte[] samplespersec = new byte[4];
            byte[] avgbytespersec = new byte[4];
            byte[] blockalign = new byte[2];
            byte[] bitspersample = new byte[2];
            byte[] additionalinfo = new byte[2];    //可选

            byte[] datasize = new byte[4];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int sum = 0;
                using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
                {
                    #region  RIFF WAVE Chunk
                    br.Read(id, 0, 4);
                    br.Read(size, 0, 4);
                    br.Read(type, 0, 4);
                    Id = getString(id, 4);
                    long longsize = bytArray2Int(size);//十六进制转为十进制
                    Size = longsize * 1.0;
                    Type = getString(type, 4);
                    #endregion
                    #region Format Chunk
                    br.Read(formatid, 0, 4);
                    br.Read(formatsize, 0, 4);
                    br.Read(formattag, 0, 2);
                    br.Read(numchannels, 0, 2);
                    br.Read(samplespersec, 0, 4);
                    br.Read(avgbytespersec, 0, 4);
                    br.Read(blockalign, 0, 2);
                    br.Read(bitspersample, 0, 2);
                    if (getString(formatsize, 2) == "18")
                    {
                        br.Read(additionalinfo, 0, 2);
                        additionalInfo = getString(additionalinfo, 2);  //附加信息
                    }
                    formatId = getString(formatid, 4);
                    formatSize = bytArray2Int(formatsize);
                    byte[] tmptag = composeByteArray(formattag);
                    formatTag = bytArray2Int(tmptag);
                    byte[] tmpchanels = composeByteArray(numchannels);
                    num_Channels = bytArray2Int(tmpchanels);                //声道数目
                    SamplesPerSec = bytArray2Int(samplespersec);            //采样率
                    AvgBytesPerSec = bytArray2Int(avgbytespersec);          //每秒所需字节数   
                    byte[] tmpblockalign = composeByteArray(blockalign);
                    BlockAlign = bytArray2Int(tmpblockalign);              //数据块对齐单位(每个采样需要的字节数)
                    byte[] tmpbitspersample = composeByteArray(bitspersample);
                    BitsPerSample = bytArray2Int(tmpbitspersample);        // 每个采样需要的bit数     
                    #endregion
                    #region Data Chunk
                    byte[] d_flag = new byte[1];
                    while (true)
                    {
                        br.Read(d_flag, 0, 1);
                        if (getString(d_flag, 1) == "d")
                        {
                            break;
                        }
                    }
                    byte[] dt_id = new byte[4];
                    dt_id[0] = d_flag[0];
                    br.Read(dt_id, 1, 3);
                    dataId = getString(dt_id, 4);
                    br.Read(datasize, 0, 4);
                    dataSize = bytArray2Int(datasize);

                    for (int i = 0; i < dataSize; i++)  //将数据保存
                    {
                        byte wavdt = br.ReadByte();
                        wavdata.Add(wavdt);
                    }
                    Console.WriteLine(wavdata.Count);
                    Console.WriteLine(BlockAlign);
                    #endregion
                }



            }

        }
        // 数字节数组转换为int
        private int bytArray2Int(byte[] bytArray)
        {
            return bytArray[0] | (bytArray[1] << 8) | (bytArray[2] << 16) | (bytArray[3] << 24);
        }
        // 将字节数组转换为字符串
        private string getString(byte[] bts, int len)
        {
            char[] tmp = new char[len];
            for (int i = 0; i < len; i++)
            {
                tmp[i] = (char)bts[i];
            }
            return new string(tmp);
        }
        // 组成4个元素的字节数组
        private byte[] composeByteArray(byte[] bt)
        {
            byte[] tmptag = new byte[4] { 0, 0, 0, 0 };
            tmptag[0] = bt[0];
            tmptag[1] = bt[1];
            return tmptag;
        }

    }
}
