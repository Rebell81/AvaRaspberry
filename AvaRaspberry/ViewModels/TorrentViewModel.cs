using AvaRaspberry.Interfaces;

namespace AvaRaspberry.ViewModels
{
    public sealed class TorrentViewModel : NetworkChartsViewModel
    {
        public TorrentViewModel(INetworkCommunicator communicator, string title, int seconds, long maxLine, long mediumLine)
            : base(communicator, seconds, App.TorrentMaxTx, maxLine, mediumLine, isTitleOverrided: true)
        {
            WidgetTitle = title;
        }
    }
}