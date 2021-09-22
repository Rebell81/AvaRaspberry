using Microsoft.Extensions.Configuration;
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



            _instance = new ConfigurationSingleton()
            {
                Widgets = configuration.GetSection(nameof(Widgets)).Get<Widgets>()
            };

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