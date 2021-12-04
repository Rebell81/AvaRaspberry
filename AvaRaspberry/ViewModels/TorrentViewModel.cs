using AvaRaspberry.Interfaces;

namespace AvaRaspberry.ViewModels
{
    public sealed class TorrentViewModel : NetworkChartsViewModel
    {
        public TorrentViewModel(INetworkCommunicator communicator, string title, ushort seconds, long maxLine, long mediumLine)
            : base(communicator, seconds)
        {
            WidgetTitle = title;
        }
    }
}