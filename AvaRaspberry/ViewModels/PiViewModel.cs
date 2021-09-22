using System.Threading.Tasks;

namespace AvaRaspberry.ViewModels
{
    public class PiViewModel : IDataPc
    {
        public PiViewModel()
        {
            Name = "Pi";
            Task.Factory.StartNew(UpdateAsync);
        }

        private void UpdateAsync()
        {
            Start(new PiCommunicator());
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

                        if(info.Data!=null)
                        {
                            TotalRam = info.Data.Memory.total_real * 1024;
                            CurrentRam = (info.Data.Memory.total_real - info.Data.Memory.avail_real) * 1024;

                            CurrentCpu = info.Data.cpu.system_load
                                         + info.Data.cpu.user_load;

                            CpuText = $"{CurrentCpu}%";


                            RamText = $"{GetSizeString(_currentRam)} / {GetSizeString(_totalRam)}";
                            Network =
                                $"{GetSizeString(info.Data.Network[0].rx, isSpeed: true)} / {GetSizeString(info.Data.Network[0].tx, isSpeed: true)}";
                        }
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