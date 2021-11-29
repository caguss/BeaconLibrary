﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenNETCF.IoC;
using UniversalBeacon.Library.Core.Entities;
using UniversalBeacon.Library.Core.Interfaces;
using Xamarin.Forms;

namespace test
{
    internal class BeaconService : IDisposable
    {
        private _beaconManger _manager;

        public BeaconService()
        {
            // get the platform-specific provider
            var provider = RootWorkItem.Services.Get<Ibeacon>();
            var provider1 = DependencyService.Get<Ibeacon>();

            if (null != provider1)
            {
                 _manager = new _beaconManger(provider1, Device.BeginInvokeOnMainThread);
                // create a beacon manager, giving it an invoker to marshal collection changes to the UI thread
                _manager.Start();
#if DEBUG
                _manager.BeaconAdded += _manager_BeaconAdded;
                provider.AdvertisementPacketReceived += Provider_AdvertisementPacketReceived;
#endif // DEBUG
            }
        }

        public void Dispose()
        {
            _manager?.Stop();
        }

        public ObservableCollection<Beacon> Beacons => _manager?.BluetoothBeacons;

#if DEBUG
        void _manager_BeaconAdded(object sender, Beacon e)
        {
            Debug.WriteLine($"_manager_BeaconAdded {sender} Beacon {e}");
        }

        void Provider_AdvertisementPacketReceived(object sender, UniversalBeacon.Library.Core.Interop.BLEAdvertisementPacketArgs e)
        {
            Debug.WriteLine($"Provider_AdvertisementPacketReceived {sender} Beacon {e}");
        }
#endif // DEBUG
    }
}
