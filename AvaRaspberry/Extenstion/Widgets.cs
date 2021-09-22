using AvaRaspberry.Interfaces;

#pragma warning disable 8618
namespace AvaRaspberry.Extenstion
{
    public class Synology : IUserPassHostPort
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
    
    public class Qb : IUserPassHostPort
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }

    public class Widgets
    {
        public Synology Synology { get; set; }
        public Qb Qb { get; set; }
    }
}