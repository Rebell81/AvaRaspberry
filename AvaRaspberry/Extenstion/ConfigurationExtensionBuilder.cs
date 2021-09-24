using Avalonia.Controls;
using AvaRaspberry.Serivices;
using System;
using System.Threading;

namespace AvaRaspberry.Extenstion
{
    public static class ConfigurationExtensionBuilder
    {
        public static T BuildConfiguration<T>(this T builder) where T : AppBuilderBase<T>, new()
        {

            while(true)
            {
                try
                {
                    BuildConfig();
                    break;
                }
                catch(Exception ex)
                {
                    LoggerService.Instance.Log(ex);
                    Thread.Sleep(5000);
                }
            }
          

            return builder;
        }
        
        private static void BuildConfig()
            => ConfigurationService.GetInstance();
    }
}