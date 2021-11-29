using System;
using System.Collections.Generic;
using System.Text;
using UniversalBeacon.Library.Core.Interop;

namespace test
{
    public interface Ibeacon
    {
        event EventHandler<BLEAdvertisementPacketArgs> AdvertisementPacketReceived;

        event EventHandler<BTError> WatcherStopped;



        void Start();

        void Stop();
    }
}
