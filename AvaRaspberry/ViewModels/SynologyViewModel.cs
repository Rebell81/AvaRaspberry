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

                        if (info.Data != null)
                        {


                            var totalInBytes = info.Data.Memory.memory_size * 1024;
                            TotalRam = totalInBytes;

                            //totalInBytes 100
                            //?? info.Data.Memory.real_usage


                            CurrentRam = (info.Data.Memory.real_usage * totalInBytes) /100 ;

                            CurrentCpu = info.Data.cpu.system_load
                                         + info.Data.cpu.user_load;

                            CpuText = $"{CurrentCpu}%";


                            RamText = $"{GetSizeString(_currentRam)} / {GetSizeString(_totalRam)} ({info.Data.Memory.real_usage}%)";
                            Network =
                                $"{GetSizeString(info.Data.Network[0].rx, isSpeed: true)} / " +
                                $"{GetSizeString(info.Data.Network[0].tx, isSpeed: true)}";


                            //if (ChartTx?.Entries != null)
                            //{
                            //    var array = ChartTx.Entries.Concat(ChartRx.Entries);
                            //    if (array.Count() > 0)
                            //        max = array.Max(x => x.Value);
                            //}




                            ProcessEntry(ref _entriesTx, info.Data.Network[0].tx, App.Green, App.SynologyMaxTx,
                                DateTime.Now.AddSeconds(-30), out var chartTx);

                            ProcessEntry(ref _entriesRx, info.Data.Network[0].rx, App.Blue, App.SynologyMaxTx,
                                DateTime.Now.AddSeconds(-30), out var chartRx);


                            ChartTx = chartTx;
                            ChartRx = chartRx;
                        }

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
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