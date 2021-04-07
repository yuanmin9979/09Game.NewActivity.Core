using System;
using System.Collections.Generic;
using System.Text;

namespace _09Game.NewActivity.Core.Log
{
    public class Logger
    {
        public static void Debug(string message)
        {
            LoggerManager.Logger.Debug(message);
        }

        public static void Debug(string message, Exception exception)
        {
            LoggerManager.Logger.Debug(exception, message);
        }

        public static void Info(string message)
        {
            LoggerManager.Logger.Info(message);
        }

        public static void Info(string message, Exception exception)
        {
            LoggerManager.Logger.Info(exception, message);
        }

        public static void Warn(string message)
        {
            LoggerManager.Logger.Warn(message);
        }

        public static void Warn(string message, Exception exception)
        {
            LoggerManager.Logger.Warn(exception, message);
        }

        public static void Error(string message)
        {
            LoggerManager.Logger.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            LoggerManager.Logger.Error(exception, message);
        }

        public static void Fatal(string message)
        {
            LoggerManager.Logger.Fatal(message);
        }

        public static void Fatal(string message, Exception exception)
        {
            LoggerManager.Logger.Fatal(exception, message);
        }

        public static void Trace(string message)
        {
            LoggerManager.Logger.Trace(message);
        }

        public static void Trace(string message, Exception exception)
        {
            LoggerManager.Logger.Trace(exception, message);
        }
    }
}
