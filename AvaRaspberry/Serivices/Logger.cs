using System;
using System.Diagnostics;
using System.IO;

namespace AvaRaspberry.Serivices
{
    public class Logger
    {
        private static Logger _logger;
        private readonly string _path;
        private readonly object _lock = new object();

        public static Logger Instance => GetInstance();

        private Logger()
        {
            _path = "/home/pi/Documents/log.txt";
        }

        private static Logger GetInstance()
        {
            return _logger ??= new Logger();
        }

        public void Log(object message)
        {
            if (message != null)
            {
                lock (_lock)
                {
                    var data = $"{DateTime.Now} | {message}";
                    

                    Console.WriteLine(data);
                    Debug.WriteLine(data);
                    File.AppendAllText(_path+Environment.NewLine+Environment.NewLine, data);
                }
            }
        }
    }
}