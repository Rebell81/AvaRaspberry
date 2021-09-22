using System;
using System.Collections.Generic;
using System.Linq;
using AvaRaspberry.Extenstion;
using Newtonsoft.Json;
using ReactiveUI;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using SkiaSharp;
using SynologyClient;

namespace AvaRaspberry.ViewModels
{
    public class SynologyViewModel : IDataPc
    {
        private List<Entry> _entriesTx = new List<Entry>();
        private List<Entry> _entriesRx = new List<Entry>();

        private LineChart _chartTx, _chartRx;

        public SynologyViewModel()
        {
            Name = "Falcon";
            Task.Factory.StartNew(UpdateAsync);
        }

        private void UpdateAsync()
        {
            Start(new SynologyCommunicator());
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

        private void Start(IPcCommunicator api)
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

                        ProcessEntry(_entriesTx, info.Data.Network[0].tx, SKColor.Parse("#66BF11"),max, out var chartTx);
                        ProcessEntry(_entriesRx, info.Data.Network[0].rx, SKColor.Parse("#385AE3"),max, out var chartRx);

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