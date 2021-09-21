#pragma warning disable 8618
namespace AvaRaspberry.Extenstion
{
    public class Weather
    {
        public string ApiToken { get; set; }
    }

    public class Widgets
    {
        public Weather Weather { get; set; } = new Weather();
    }
}