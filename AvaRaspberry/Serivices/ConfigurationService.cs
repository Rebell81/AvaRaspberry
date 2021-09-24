using System;
using AvaRaspberry.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AvaRaspberry.Extenstion
{
    public class ConfigurationService
    {
        private ConfigurationService()
        {
        }

        private static ConfigurationService _instance;

        public static ConfigurationService GetInstance()
        {
            Console.WriteLine("GetInstance Start");
            //var file = File.ReadAllText("/home/pi/Documents/appsettings.json");
            //Console.WriteLine(file);
            Console.WriteLine("GetInstance Start2");

            try
            {
                if (_instance != null) return _instance;
#if DEBUG
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();
#else
                Console.WriteLine("GetInstance 1");

                var bulder = new ConfigurationBuilder();
                Console.WriteLine("GetInstance 2");

                    var filsse = bulder.AddJsonFile("/home/pi/Documents/appsettings.json",
                            true, false);
                    Console.WriteLine("GetInstance 3");

                    var configuration = filsse.Build();
                    Console.WriteLine("GetInstance 4");

#endif
                Console.WriteLine("GetInstance mid");

                
                _instance = new ConfigurationService()
                {
                    Widgets = configuration.GetSection(nameof(Widgets)).Get<Widgets>()
                };

                //_instance.Widgets.Torrents = configuration.GetSection($"{nameof(Widgets)}:{nameof(Models.Widgets.Torrents)}")
                //    .GetChildren()
                //    .ToList()
                //    .Select(x => new TorrentConfig
                //    {
                //        User = x.GetValue<string>(nameof(TorrentConfig.User)),
                //        Password = x.GetValue<string>(nameof(TorrentConfig.Password)),
                //        Host = x.GetValue<string>(nameof(TorrentConfig.Host)),
                //        Port = x.GetValue<string>(nameof(TorrentConfig.Port)),
                //        Ssl = x.GetValue<bool>(nameof(TorrentConfig.Ssl)),
                //    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }





            Console.WriteLine("GetInstance end");

            return _instance;
        }

        public static ConfigurationService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                return GetInstance();
            }
        }


        public Widgets Widgets { get; set; }
    }
}