using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System.Threading.Tasks;
using AvaRaspberry.Serivices;
using SkiaSharp;
using AvaRaspberry.Interfaces;
using Humanizer;
using AvaRaspberry.Converters;

namespace AvaRaspberry.ViewModels
{
    public class SynologyViewModel : DataPc
    {

        private BytesToUserFriendlyText converter = new BytesToUserFriendlyText();
        private BytesToUserFriendlyText converter2 = new BytesToUserFriendlyText() { IsSpeed = true };


        private readonly IPcCommunicator _communicator;


        public SynologyViewModel(IPcCommunicator communicator) : base(communicator, 30)
        {
            Name = "Falcon";
            Start();
            _communicator = communicator;
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

                        if (info != null && info.Data != null)
                        {


                            var totalInBytes = info.Data.Memory.memory_size * 1024;
                            TotalRam = totalInBytes;

                            //totalInBytes 100
                            //?? info.Data.Memory.real_usage


                            CurrentRam = (info.Data.Memory.real_usage * totalInBytes) / 100;

                            CurrentCpu = info.Data.cpu.system_load
                                         + info.Data.cpu.user_load;

                            CpuText = $"{CurrentCpu}%";


                            RamText = $"{converter.Convert(_currentRam)} / {converter.Convert(TotalRam)} ({info.Data.Memory.real_usage}%)";
                            Network = $"{converter2.Convert(info.Data.Network[0].rx)}     {converter2.Convert(info.Data.Network[0].tx)}";


                            //if (ChartTx?.Entries != null)
                            //{
                            //    var array = ChartTx.Entries.Concat(ChartRx.Entries);
                            //    if (array.Count() > 0)
                            //        max = array.Max(x => x.Value);
                            //}




                            //ProcessEntry(ref _entriesTx, info.Data.Network[0].tx, App.Green, App.SynologyMaxTx,
                            //    DateTime.Now.AddSeconds(-30), out var chartTx);

                            //ProcessEntry(ref _entriesRx, info.Data.Network[0].rx, App.Blue, App.SynologyMaxTx,
                            //    DateTime.Now.AddSeconds(-30), out var chartRx);


                            //ChartTx = chartTx;
                            //ChartRx = chartRx;
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggerService.Instance.Log(ex);
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