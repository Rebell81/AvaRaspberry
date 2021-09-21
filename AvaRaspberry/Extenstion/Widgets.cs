#pragma warning disable 8618
namespace AvaRaspberry.Extenstion
{
    public class Synology
    {
        public string User { get; set; }
        public string Password { get; set; }

    }

    public class Widgets
    {
        public Synology Synology { get; set; }
    }
}