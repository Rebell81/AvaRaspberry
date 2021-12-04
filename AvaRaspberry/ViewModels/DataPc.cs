using System;
using AvaRaspberry.Interfaces;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public class DataPc : NetworkChartsViewModel
    {

        protected DataPc(INetworkCommunicator communicator, ushort max)
            : base(communicator, max)
        {

        }


        private string _name = string.Empty;
        private string _errorCode = string.Empty;
        private bool _isConnected;
        protected string _cpuText = string.Empty, _ramText = String.Empty, _network = String.Empty;

        protected long _totalRam, _currentRam = 0, _currentCpu = 0;

        public long TotalRam
        {
            get => _totalRam;
            protected set => this.RaiseAndSetIfChanged(ref _totalRam, value);
        }

        public long CurrentRam
        {
            get => _currentRam;
            protected set => this.RaiseAndSetIfChanged(ref _currentRam, value);
        }

        public long CurrentCpu
        {
            get => _currentCpu;
            protected set => this.RaiseAndSetIfChanged(ref _currentCpu, value);
        }

        public string Name
        {
            get => _name;
            protected set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            protected set => this.RaiseAndSetIfChanged(ref _isConnected, value);
        }

        public string ErrorCode
        {
            get => _errorCode;
            protected set => this.RaiseAndSetIfChanged(ref _errorCode, value);
        }

        public string CpuText
        {
            get => _cpuText;
            protected set => this.RaiseAndSetIfChanged(ref _cpuText, value);
        }

        public string RamText
        {
            get => _ramText;
            protected set => this.RaiseAndSetIfChanged(ref _ramText, value);
        }

        public string Network
        {
            get => _network;
            protected set => this.RaiseAndSetIfChanged(ref _network, value);
        }
    }
}