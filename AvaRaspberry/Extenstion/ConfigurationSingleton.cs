using System;
using AvaRaspberry.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
#pragma warning disable 8618

namespace AvaRaspberry.Extenstion
{
    public class ConfigurationSingleton
    {
        private ConfigurationSingleton()
        {
        }

        private static ConfigurationSingleton _instance;

        public static ConfigurationSingleton GetInstance()
        {
            Console.WriteLine("GetInstance Start");

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (_instance != null) return _instance;
#if DEBUG
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();
#else
 var configuration = new ConfigurationBuilder()
                .AddJsonFile("/home/pi/appsettings.json", true, true)
                .Build();
#endif

            Console.WriteLine("GetInstance mid");


            _instance = new ConfigurationSingleton()
            {
                Widgets = configuration.GetSection(nameof(Widgets)).Get<Widgets>()
            };

            _instance.Widgets.Torrents = configuration.GetSection($"{nameof(Widgets)}:{nameof(Models.Widgets.Torrents)}")
                     .GetChildren()
                     .ToList()
                     .Select(x => new TorrentConfig
                     {
                         User = x.GetValue<string>(nameof(TorrentConfig.User)),
                         Password = x.GetValue<string>(nameof(TorrentConfig.Password)),
                         Host = x.GetValue<string>(nameof(TorrentConfig.Host)),
                         Port = x.GetValue<string>(nameof(TorrentConfig.Port)),
                         Ssl = x.GetValue<bool>(nameof(TorrentConfig.Ssl)),
                     }).ToList();

            Console.WriteLine("GetInstance end");

            return _instance;
        }

        public static ConfigurationSingleton Instance
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