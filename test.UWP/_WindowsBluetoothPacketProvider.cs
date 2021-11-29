using System;
using Windows.Devices.Bluetooth.Advertisement;
using UniversalBeacon.Library.Core.Interfaces;
using UniversalBeacon.Library.Core.Interop;
using Xamarin.Forms;
using UniversalBeacon.Library.UWP;
using test;
using System.Runtime.InteropServices.WindowsRuntime;

[assembly: Dependency(typeof(_WindowsBluetoothPacketProvider))]
namespace UniversalBeacon.Library.UWP
{
    public class _WindowsBluetoothPacketProvider : Ibeacon
    {
        public event EventHandler<BLEAdvertisementPacketArgs> AdvertisementPacketReceived;
        public event EventHandler<BTError> WatcherStopped;

        private readonly BluetoothLEAdvertisementWatcher _watcher;
        private bool _running;

        public _WindowsBluetoothPacketProvider()
        {
            _watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
        }

        /// <summary>
        /// Gets the BluetoothLEAdvertisementWatcher used by the provider instance
        /// </summary>
        public BluetoothLEAdvertisementWatcher AdvertisementWatcher
        {
            get => _watcher;
        }

        public BLEAdvertisementWatcherStatusCodes WatcherStatus
        {
            get
            {
                if (_watcher == null)
                {
                    return BLEAdvertisementWatcherStatusCodes.Stopped;
                }

                return (BLEAdvertisementWatcherStatusCodes)_watcher.Status;
            }
        }

        private void WatcherOnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            if (eventArgs.Advertisement == null) return;

            var result = new BLEAdvertisement
            {
                LocalName = eventArgs.Advertisement.LocalName
            };

            result.ServiceUuids.AddRange(eventArgs.Advertisement.ServiceUuids);

            if (eventArgs.Advertisement.DataSections != null)
            {
                foreach (var curDataSection in eventArgs.Advertisement.DataSections)
                {
                    var data = new BLEAdvertisementDataSection
                    {
                        DataType = curDataSection.DataType,
                        Data = curDataSection.Data.ToArray()
                    };

                    result.DataSections.Add(data);
                }
            }

            if (eventArgs.Advertisement.ManufacturerData != null)
            {
                foreach (var curManufacturerData in eventArgs.Advertisement.ManufacturerData)
                {
                    var data = new BLEManufacturerData
                    {
                        CompanyId = curManufacturerData.CompanyId,
                        Data = curManufacturerData.Data.ToArray()
                    };

                    result.ManufacturerData.Add(data);
                }
            }


            var packet = new BLEAdvertisementPacket
            {
                Timestamp = eventArgs.Timestamp,
                BluetoothAddress = eventArgs.BluetoothAddress,
                RawSignalStrengthInDBm = eventArgs.RawSignalStrengthInDBm,
                AdvertisementType = (BLEAdvertisementType)eventArgs.AdvertisementType,
                Advertisement = result
            };

            AdvertisementPacketReceived?.Invoke(this, new BLEAdvertisementPacketArgs(packet));
        }
      
        public void Start()
        {
            if (_running) return;

            lock (_watcher)
            {
                _watcher.Received += WatcherOnReceived;
                _watcher.Stopped += WatcherOnStopped;
                _watcher.Start();

                _running = true;
            }
        }

        private void WatcherOnStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            WatcherStopped?.Invoke(this, new BTError((BTError.BluetoothError) args.Error));
        }

        public void Stop()
        {
            if (!_running) return;

            lock (_watcher)
            {
                _watcher.Received -= WatcherOnReceived;
                _watcher.Stop();

                _running = false;
            }
        }


    }
}
