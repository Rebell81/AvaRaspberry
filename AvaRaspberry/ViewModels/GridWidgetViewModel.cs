using System;
using System.Collections.Generic;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Serivices;
using ReactiveUI;
using System.Linq;
using System.Threading.Tasks;

namespace AvaRaspberry.ViewModels
{
    public class GridWidgetViewModel : ViewModelBase
    {
        private TorrentViewModel _qbTorrentViewModel, _qbTorrentViewModelLocal;

        private NetworkChartsViewModel _networkChartsViewModelSynology,
            _networkChartsViewModelPi,
            _networkChartsViewModelFalcon;

        private SynologyViewModel _synologyViewModel;

        public TorrentViewModel QbTorrentViewModel
        {
            get => _qbTorrentViewModel;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModel, value);
        }

        public TorrentViewModel QbTorrentViewModelLocal
        {
            get => _qbTorrentViewModelLocal;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModelLocal, value);
        }

        public SynologyViewModel SynologyViewModel
        {
            get => _synologyViewModel;
            protected set => this.RaiseAndSetIfChanged(ref _synologyViewModel, value);
        }

        public NetworkChartsViewModel NetworkChartsViewModelSynology
        {
            get => _networkChartsViewModelSynology;
            protected set => this.RaiseAndSetIfChanged(ref _networkChartsViewModelSynology, value);
        }

        public NetworkChartsViewModel NetworkChartsViewModelPi
        {
            get => _networkChartsViewModelPi;
            protected set => this.RaiseAndSetIfChanged(ref _networkChartsViewModelPi, value);
        }

        public NetworkChartsViewModel NetworkChartsViewModelFalcon
        {
            get => _networkChartsViewModelFalcon;
            protected set => this.RaiseAndSetIfChanged(ref _networkChartsViewModelFalcon, value);
        }

        public GridWidgetViewModel()
        {
            var tasks = new List<Task>
            {
                new(InitSynologyViewModel),
                new(InitTorrentViewModelPi),
                new(InitTorrentViewModelFalcon),
                new(InitNetworkChartsViewModelSynology),
                new(InitNetworkChartsViewModelPi),
                new(InitNetworkChartsViewModelFalcon),
            };
            
            Parallel.ForEach(tasks, task =>
            {
                task.Start();
            });
        }

        private void InitSynologyViewModel()
        {
            SynologyViewModel = new SynologyViewModel(new SynologyCommunicator());
        }

        private void InitTorrentViewModelPi()
        {
            QbTorrentViewModelLocal = new TorrentViewModel(
                new QBittorrentService(ConfigurationSingleton.Instance.Widgets.Torrents.Last()), "QBittorrent Pi",
                30, App.TorrentMaxTxLine, App.TorrentMediumTxLine);
        }

        private void InitTorrentViewModelFalcon()
        {
            QbTorrentViewModel = new TorrentViewModel(
                new QBittorrentService(ConfigurationSingleton.Instance.Widgets.Torrents.First()), "QBittorrent Falcon",
                30, App.TorrentMaxTxLine, App.TorrentMediumTxLine);
        }

        private void InitNetworkChartsViewModelSynology()
        {
            NetworkChartsViewModelSynology = new NetworkChartsViewModel(
                new SynologyCommunicator(),
                (int) TimeSpan.FromHours(24).TotalSeconds, App.SynologyMaxTx, App.SynologyMaxTxLine, App.SynologyMediumTxLine);
        }

        private void InitNetworkChartsViewModelPi()
        {
            NetworkChartsViewModelPi = new NetworkChartsViewModel(
                new QBittorrentService(ConfigurationSingleton.Instance.Widgets.Torrents.Last()),
                (int) TimeSpan.FromHours(24).TotalSeconds, App.TorrentMaxTx, App.TorrentMaxTxLine, App.TorrentMediumTxLine);
        }

        private void InitNetworkChartsViewModelFalcon()
        {
            NetworkChartsViewModelFalcon = new NetworkChartsViewModel(
                new QBittorrentService(ConfigurationSingleton.Instance.Widgets.Torrents.First()),
                (int) TimeSpan.FromHours(24).TotalSeconds, App.TorrentMaxTx, App.TorrentMaxTxLine, App.TorrentMediumTxLine, true);
        }
    }
}