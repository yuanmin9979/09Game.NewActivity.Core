using System;
using System.Collections.Generic;
using System.Text;

namespace Billing.Api.Utility
{
    /// <summary>
    /// 订单编号生成器
    /// </summary>
    public class IdGenerator
    {
        private readonly long _max;
        private int _seed;
        private string _pre;
        private readonly object _locker = new object();
        private int _seedWidth;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="seedWith"></param>
        public IdGenerator(int seedWith, string pre = "")
        {
            _pre = pre;
            _seedWidth = seedWith;
            _max = (long)Math.Pow(10, seedWith) - 1;
        }

        private const string TimeFormat = "yyMMdd";

        /// <summary>
        /// 生成Id
        /// </summary>
        /// <returns></returns>
        public string GetId(DateTime time)
        {
            var prefix = time.ToString(TimeFormat);

            var stamp = (time.Hour * 3600 + time.Minute * 60 + time.Second).ToString().PadLeft(5, '0');

            lock (_locker)
            {
                _seed++;
                var id = $"{prefix}{stamp}{_seed.ToString().PadLeft(_seedWidth, '0')}";

                if (_seed >= _max)
                {
                    _seed = 0;
                }
                return $"{_pre}{id}";
            }
        }

        public string RndNum(DateTime time)
        {
            var prefix = time.ToString(TimeFormat);

            var stamp = (time.Hour * 3600 + time.Minute * 60 + time.Second).ToString().PadLeft(5, '0');
            //验证码可以显示的字符集合  
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
                ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
                ",R,S,T,U,V,W,X,Y,Z";
            //string Vchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < 3 + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return RndNum(time);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return $"{prefix}{stamp}{code}";
        }

        public  string UUID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            long long_guid = BitConverter.ToInt64(buffer, 0);

            string _Value = System.Math.Abs(long_guid).ToString();
            byte[] buf = new byte[_Value.Length];
            int p = 0;
            for (int i = 0; i < _Value.Length;)
            {
                byte ph = System.Convert.ToByte(_Value[i]);
                int fix = 1;
                if ((i + 1) < _Value.Length)
                {
                    byte pl = System.Convert.ToByte(_Value[i + 1]);
                    buf[p] = (byte)((ph << 4) + pl);
                    fix = 2;
                }
                else
                {
                    buf[p] = (byte)(ph);
                }
                if ((i + 3) < _Value.Length)
                {
                    if (System.Convert.ToInt16(_Value.Substring(i, 3)) < 256)
                    {
                        buf[p] = System.Convert.ToByte(_Value.Substring(i, 3));
                        fix = 3;
                    }
                }
                p++;
                i = i + fix;
            }
            byte[] buf2 = new byte[p];
            for (int i = 0; i < p; i++)
            {
                buf2[i] = buf[i];
            }
            string cRtn = System.Convert.ToBase64String(buf2);
            if (cRtn == null)
            {
                cRtn = "";
            }
            cRtn = cRtn.ToLower();
            cRtn = cRtn.Replace("/", "");
            cRtn = cRtn.Replace("+", "");
            cRtn = cRtn.Replace("=", "");
            if (cRtn.Length == 12)
            {
                return cRtn;
            }
            else
            {
                return UUID();
            }
        }
    }
}
