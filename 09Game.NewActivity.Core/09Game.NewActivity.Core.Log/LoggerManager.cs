using System;
using System.Collections.Generic;
using System.Text;

namespace _09Game.NewActivity.Core.Log
{
    internal class LoggerManager
    {
        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private LoggerManager() { }
    }
}
