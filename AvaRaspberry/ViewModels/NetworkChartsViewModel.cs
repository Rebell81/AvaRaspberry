using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AvaRaspberry.Converters;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using AvaRaspberry.Serivices;
using Humanizer;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public class pnt
    {
        public pnt(long val, DateTime time)
        {
            Value = val;
            Time = time;
        }

        public long Value { get; set; }
        public DateTime Time { get; set; }
    }


    public class NetworkChartsViewModel : WidgetViewModel
    {
        public List<ISeries> Series { get; set; }
        public ObservableCollection<pnt> RxData { get; set; }
        public ObservableCollection<pnt> TxData { get; set; }
        public List<Axis> YAxes { get; set; }
        public List<Axis> XAxes { get; set; }

        public BytesToUserFriendlyText converterSpeed = new BytesToUserFriendlyText() { IsSpeed = true };

        private readonly Task _updateTask;
        protected NetworkStatisticResponce _torrentClientStatistic;
        private protected readonly INetworkCommunicator Communicator;
        private ushort _maxSeconds;

        public NetworkStatisticResponce NetworkStatistic
        {
            get => _torrentClientStatistic ?? new NetworkStatisticResponce();
            protected set => this.RaiseAndSetIfChanged(ref _torrentClientStatistic, value);
        }


        public NetworkChartsViewModel()
        {
        }

        public NetworkChartsViewModel(INetworkCommunicator communicator, ushort maxSeconds = 0)
        {
            Communicator = communicator;
            _maxSeconds = maxSeconds;


            RxData = new ObservableCollection<pnt>();
            TxData = new ObservableCollection<pnt>();


            Series = new List<ISeries>
            {
                new LineSeries<pnt>
                {
                    Values = RxData,
                    LineSmoothness = 1,
                    //Fill = new SolidColorPaint(App.Blue),                    
                    //Stroke = new SolidColorPaint(App.Blue),
                    //GeometryFill= new SolidColorPaint(App.Blue),
                    //GeometryStroke = null,
                    GeometrySize = 1,
                    DataPadding = new LvcPoint(0,0),
                    TooltipLabelFormatter = (value) => converterSpeed.Convert(value.PrimaryValue).ToString(),
                },
                new LineSeries<pnt>
                {
                    Values = TxData,
                    LineSmoothness = 1,
                    //Fill = new SolidColorPaint(App.Green),
                    //Stroke = new SolidColorPaint(App.Green),
                    //GeometryStroke = null,
                    //GeometryFill= new SolidColorPaint(App.Green),
                    GeometrySize = 1,
                    DataPadding = new LvcPoint(0,0),
                    TooltipLabelFormatter = (value) => converterSpeed.Convert(value.PrimaryValue).ToString(),
                }
            };

            YAxes = new List<Axis>
            {
                new Axis
                {
                    IsVisible = true,
                    Name = "Speed",
                    Labeler = (value) => converterSpeed.Convert(value).ToString(),
                    LabelsRotation = -45,
                }
            };
            XAxes = new List<Axis>
            {
                new Axis
                {
                    IsVisible = false,
                    Name = "Time"
                }
            };

            LiveCharts.Configure(config => config.HasMap<pnt>((pnt, point) =>
            {
                point.PrimaryValue = (float)pnt.Value;
                point.SecondaryValue = point.Context.Index;
            }));

            _updateTask = Task.Run(GetStats);


        }

        private async Task GetStats()
        {
            while (true)
            {
                try
                {
                    NetworkStatistic = await Communicator.GetNetworkStatisticData();

                    long unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

                    ClearOld(TxData);
                    ClearOld(RxData);

                    TxData.Add(new pnt(NetworkStatistic.TotalTx, DateTime.Now));
                    RxData.Add(new pnt(NetworkStatistic.TotalRx, DateTime.Now));

                    //TxData.Add(networkStatistic.TotalRx);
                    //RxData.Add(networkStatistic.TotalTx);
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    await Task.Delay(App.GlobalDelay);
                }
            }
        }

        private void ClearOld(ObservableCollection<pnt> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                pnt pnt = collection[i];
                var totalDifference = DateTime.Now - pnt.Time;

                if (totalDifference.TotalSeconds > _maxSeconds)
                {
                    collection.Remove(pnt);
                }
                else
                {
                    break;
                }
            }
        }
    }
}