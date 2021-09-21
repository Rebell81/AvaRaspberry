using System;
using AvaRaspberry.Extenstion;
using Newtonsoft.Json;
using ReactiveUI;
using System.Threading.Tasks;
using SynologyClient;

namespace AvaRaspberry.ViewModels
{
    public class SynologyViewModel : ViewModelBase
    {
        private string name = string.Empty;
        private string errorCode = string.Empty;
        private bool _isConnected;
        private string _cpu, _ramText, _network, _path;

        private long _totalRam, _currentRam;

        public long TotalRam
        {
            get => _totalRam;
            private set => this.RaiseAndSetIfChanged(ref _totalRam, value);
        }

        public long CurrentRam
        {
            get => _currentRam;
            private set => this.RaiseAndSetIfChanged(ref _currentRam, value);
        }


        public string Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            private set => this.RaiseAndSetIfChanged(ref _isConnected, value);
        }

        public string ErrorCode
        {
            get => errorCode;
            private set => this.RaiseAndSetIfChanged(ref errorCode, value);
        }

        public string Cpu
        {
            get => _cpu;
            private set => this.RaiseAndSetIfChanged(ref _cpu, value);
        }

        public string RamText
        {
            get => _ramText;
            private set => this.RaiseAndSetIfChanged(ref _ramText, value);
        }

        public string Network
        {
            get => _network;
            private set => this.RaiseAndSetIfChanged(ref _network, value);
        }

        public string Path
        {
            get => _path;
            private set => this.RaiseAndSetIfChanged(ref _path, value);
        }


        public SynologyViewModel()
        {
            Task.Factory.StartNew(UpdateAsync);
        }

        private void UpdateAsync()
        {
            try
            {
                var synology = ConfigurationSingleton.Instance.Widgets.Synology;


                Name = synology.User;
                // var settings = App.ServiceProvider.GetService<ISynologyConnectionSettings>();


                var dd = new AppSettingsClientConfig();

                dd.User = synology.User;
                dd.Password = synology.Password;
                dd.ApiBaseAddressAndPathNoTrailingSlash = "https://jsb.by:5001/webapi";

                var session = new SynologySession(dd);
                session.Login();

                var api = new SynologyApi(session, dd);

                start(api);
            }
            catch (Exception ex)
            {
            }
        }

        private void start(SynologyApi api)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var info = api.GetUtilization();

                        TotalRam = info.Data.Memory.total_real * 1024;
                        CurrentRam = (info.Data.Memory.total_real - info.Data.Memory.avail_real) * 1024;

                        Cpu = $"{info.Data.cpu.system_load}";
                        RamText = $"{GetSizeString(_currentRam)} / {GetSizeString(_totalRam)}";
                        Network =
                            $"{GetSizeString(info.Data.Network[0].rx, isSpeed: true)} / {GetSizeString(info.Data.Network[0].tx, isSpeed: true)}";
                        Path = $"{Environment.CurrentDirectory} | {Environment.CommandLine} | {Environment.OSVersion}";
                    }
                    catch
                    {
                        IsConnected = false;
                    }
                    finally
                    {
                        IsConnected = true;

                        await Task.Delay(1500);
                    }
                }
            });
        }

        public static string GetSizeString(long length, bool showEmpty = true, bool isSpeed = false)
        {
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = length;
            var suffix = nameof(B);

            double SelSize;
            if (length >= TB)
            {
                SelSize = TB;
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                SelSize = GB;
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                SelSize = MB;
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                SelSize = KB;
                suffix = nameof(KB);
            }
            else
            {
                var app = isSpeed ? "/s" : "";
                return showEmpty ? $"0 KB{app}" : string.Empty;
            }

            size = Math.Round(length / SelSize, 2);
            var ap2 = isSpeed ? "/s" : "";

            return $"{size} {suffix}{ap2}";
        }
    }
}