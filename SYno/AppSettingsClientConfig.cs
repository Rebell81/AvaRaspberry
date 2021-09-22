namespace SynologyClient
{
    public class AppSettingsClientConfig : ISynologyClientConfig
    {
        public AppSettingsClientConfig()
        {
            ApiBaseAddressAndPathNoTrailingSlash = "jsb.by";
            User = "Shark";
            Password = "1231";
        }

        public string ApiBaseAddressAndPathNoTrailingSlash { get; set; }
        public string User { get;  set; }
        public string Password { get;  set; }
    }
}