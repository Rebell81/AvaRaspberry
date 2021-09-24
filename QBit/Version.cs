using System.IO;

namespace qBittorrent.qBittorrentApi
{
    public class Version : RequestResult
    {
        public int Major { get; private set; }

        public int Minor { get; private set; }

        public int Patch { get; private set; }

        public Version(int major, int minor, int patch)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
        }

        public static Version Parse(string versionStr)
        {
            if(versionStr[0] != 'v')
                throw new InvalidDataException();
            var versionSpitted = versionStr.Substring(1).Split('.');
            if (versionSpitted.Length != 3)
                throw new InvalidDataException();
            return new Version(int.Parse(versionSpitted[0]), int.Parse(versionSpitted[1]), int.Parse(versionSpitted[2]));
        }
    }
}
