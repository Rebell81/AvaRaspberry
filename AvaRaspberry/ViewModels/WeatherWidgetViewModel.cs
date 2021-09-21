using System.Threading.Tasks;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Models;
using AvaRaspberry.Services;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public class WeatherWidgetViewModel : ViewModelBase
    {
        private readonly YandexWeatherService _weatherService = new();
        
        private WeatherModel _weatherModel = new("Unknown", 0);
        private string name;

        public string Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }


        public WeatherWidgetViewModel()
        {
            Task.Run(async () => await UpdateForecast());
        }

        private async Task UpdateForecast()
        {
            var pp = ConfigurationSingleton.GetInstance();

            Name = pp.Widgets.Synology.User;
        }
    }
}