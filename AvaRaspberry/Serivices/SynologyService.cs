using AvaRaspberry.Interfaces;
using AvaRaspberry.Serivices.Communicators;


namespace AvaRaspberry.Serivices
{
    public class SynologyService
    {
        private static IPcCommunicator _synologyCommunicator;

        public static IPcCommunicator Instance => GetInstance();


        private static IPcCommunicator GetInstance()
        {
            return _synologyCommunicator ??= new SynologyCommunicator();
        }
    }



}
