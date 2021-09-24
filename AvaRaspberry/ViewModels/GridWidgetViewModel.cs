using System;
using System.Collections.Generic;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Serivices;
using ReactiveUI;
using System.Linq;
using System.Threading.Tasks;
using AvaRaspberry.Serivices.Communicators;

namespace AvaRaspberry.ViewModels
{
    public class GridWidgetViewModel : ViewModelBase
    {
        private TorrentViewModel _qbTorrentViewModel, _qbTorrentViewModelPi;

        private NetworkChartsViewModel _networkChartsViewModelSynology,
            _networkChartsViewModelPi,
            _networkChartsViewModelFalcon;

        private SynologyViewModel _synologyViewModel;


        public SynologyViewModel SynologyViewModel
        {
            get => _synologyViewModel;
            protected set => this.RaiseAndSetIfChanged(ref _synologyViewModel, value);
        }

        public TorrentViewModel QbTorrentViewModelPi
        {
            get => _qbTorrentViewModelPi;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModelPi, value);
        }

        public TorrentViewModel QbTorrentViewModelFalcon
        {
            get => _qbTorrentViewModel;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModel, value);
        }

        public NetworkChartsViewModel NetworkChartsViewModelSynology
        {
            get => _networkChartsViewModelSynology;
            protected set => this.RaiseAndSetIfChanged(ref _networkChartsViewModelSynology, value);
        }

        public NetworkChartsViewModel NetworkChartsViewModelTorrentPi
        {
            get => _networkChartsViewModelPi;
            protected set => this.RaiseAndSetIfChanged(ref _networkChartsViewModelPi, value);
        }

        public NetworkChartsViewModel NetworkChartsViewModelTorrentFalcon
        {
            get => _networkChartsViewModelFalcon;
            protected set => this.RaiseAndSetIfChanged(ref _networkChartsViewModelFalcon, value);
        }















        public GridWidgetViewModel()
        {
            QBitTorrentService.Init(ConfigurationService.Instance.Widgets.Torrents.Falcon, ConfigurationService.Instance.Widgets.Torrents.Pi);


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
            SynologyViewModel = new SynologyViewModel(SynologyService.Instance);
        }

        private void InitNetworkChartsViewModelSynology()
        {
            NetworkChartsViewModelSynology = new NetworkChartsViewModel(
                SynologyService.Instance,
                (int)TimeSpan.FromHours(24).TotalSeconds, App.SynologyMaxTx, App.SynologyMaxTxLine, App.SynologyMediumTxLine);
        }







        private void InitTorrentViewModelPi()
        {
            QbTorrentViewModelPi = new TorrentViewModel(
                QBitTorrentService.InstancePi, "QBittorrent Pi",
                30, App.TorrentMaxTxLine, App.TorrentMediumTxLine);
        }

        private void InitNetworkChartsViewModelPi()
        {
            NetworkChartsViewModelTorrentPi = new NetworkChartsViewModel(
                QBitTorrentService.InstancePi, (int)TimeSpan.FromHours(24).TotalSeconds, App.TorrentMaxTx,
                App.TorrentMaxTxLine, App.TorrentMediumTxLine);
        }






        private void InitTorrentViewModelFalcon()
        {
            QbTorrentViewModelFalcon = new TorrentViewModel(
                QBitTorrentService.InstanceFalcon, "QBittorrent Falcon",
                30, App.TorrentMaxTxLine, App.TorrentMediumTxLine);
        }

        private void InitNetworkChartsViewModelFalcon()
        {
            NetworkChartsViewModelTorrentFalcon = new NetworkChartsViewModel(
                 QBitTorrentService.InstanceFalcon, (int)TimeSpan.FromHours(24).TotalSeconds,
                 App.TorrentMaxTx, App.TorrentMaxTxLine, App.TorrentMediumTxLine, true);
        }
    }
}