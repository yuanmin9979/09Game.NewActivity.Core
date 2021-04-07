using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit
{
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
    }
}
