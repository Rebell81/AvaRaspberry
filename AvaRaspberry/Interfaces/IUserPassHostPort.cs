namespace AvaRaspberry.Interfaces
{
    public interface IUserPassHostPort
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public bool Ssl { get; set; }

    }
}