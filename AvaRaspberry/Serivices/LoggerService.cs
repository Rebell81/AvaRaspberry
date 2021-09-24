using System;
using System.Diagnostics;
using System.IO;

namespace AvaRaspberry.Serivices
{
    public class LoggerService
    {
        private static LoggerService _logger;
        private readonly string _path;
        private readonly object _lock = new object();

        public static LoggerService Instance => GetInstance();

        private LoggerService()
        {
            _path = "/home/pi/Documents/log.txt";
        }

        private static LoggerService GetInstance()
        {
            return _logger ??= new LoggerService();
        }

        public void Log(object message)
        {
            if (message != null)
            {
                lock (_lock)
                {
                    var data = $"{DateTime.Now} | {message}"+Environment.NewLine+Environment.NewLine;
                    

                    Console.WriteLine(data);
                    Debug.WriteLine(data);
                    File.AppendAllText(_path, data);
                }
            }
        }
    }
}