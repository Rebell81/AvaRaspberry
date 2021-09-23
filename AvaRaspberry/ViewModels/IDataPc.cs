using System;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public class IDataPc : WidgetViewModel
    {
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

        protected static string GetSizeString(long length, bool showEmpty = true, bool isSpeed = false)
        {
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size;
            var suffix = nameof(B);

            double selSize;
            if (length >= TB)
            {
                selSize = TB;
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                selSize = GB;
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                selSize = MB;
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                selSize = KB;
                suffix = nameof(KB);
            }
            else
            {
                var app = isSpeed ? "/s" : "";
                return showEmpty ? $"0 KB{app}" : string.Empty;
            }

            size = Math.Round(length / selSize, 2);
            var ap2 = isSpeed ? "/s" : "";

            return $"{size} {suffix}{ap2}";
        }

    }
}