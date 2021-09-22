using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using AvaRaspberry.Serivices;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public class SynologyViewModel : IDataPc
    {
        private List<Tuple<DateTime, Entry>> _entriesTx = new List<Tuple<DateTime, Entry>>();
        private List<Tuple<DateTime, Entry>> _entriesRx = new List<Tuple<DateTime, Entry>>();
        private readonly IPcCommunicator _communicator;

        private LineChart _chartTx, _chartRx;

        public SynologyViewModel(IPcCommunicator communicator)
        {
            Name = "Falcon";
            Start();
            _communicator = communicator;
        }

        public LineChart ChartRx
        {
            get => _chartRx;
            protected set => this.RaiseAndSetIfChanged(ref _chartRx, value);
        }

        public LineChart ChartTx
        {
            get => _chartTx;
            protected set => this.RaiseAndSetIfChanged(ref _chartTx, value);
        }

        private void Start()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var info = _communicator.GetUtilization();

                        TotalRam = info.Data.Memory.total_real * 1024;
                        CurrentRam = (info.Data.Memory.total_real - info.Data.Memory.avail_real) * 1024;

                        CurrentCpu = info.Data.cpu.system_load
                                     + info.Data.cpu.user_load;

                        CpuText = $"{CurrentCpu}%";


                        RamText = $"{GetSizeString(_currentRam)} / {GetSizeString(_totalRam)}";
                        Network =
                            $"{GetSizeString(info.Data.Network[0].rx, isSpeed: true)} / " +
                            $"{GetSizeString(info.Data.Network[0].tx, isSpeed: true)}";

                        float max = 0;
                        if (ChartTx?.Entries != null)
                        {
                            var array = ChartTx.Entries.Concat(ChartRx.Entries);
                            max = array.Max(x => x.Value);
                        }

                        ProcessEntry(ref _entriesTx, info.Data.Network[0].tx, SKColor.Parse("#66BF11"), max,
                            DateTime.Now.AddSeconds(-30), out var chartTx);
                        ProcessEntry(ref _entriesRx, info.Data.Network[0].rx, SKColor.Parse("#385AE3"), max,
                            DateTime.Now.AddSeconds(-30), out var chartRx);

                        ChartTx = chartTx;
                        ChartRx = chartRx;
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
    }
}