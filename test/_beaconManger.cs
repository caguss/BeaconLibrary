using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UniversalBeacon.Library.Core.Entities;
using UniversalBeacon.Library.Core.Interop;

namespace test
{
    public class _beaconManger
    { 
     private readonly Ibeacon _provider;

    private readonly Action<Action> _invokeAction;

    public ObservableCollection<Beacon> BluetoothBeacons
    {
        get;
        set;
    } = new ObservableCollection<Beacon>();


    public event EventHandler<Beacon> BeaconAdded;

    public _beaconManger(Ibeacon provider, Action<Action> invokeAction = null)
    {
        _provider = provider;
        _provider.AdvertisementPacketReceived += OnAdvertisementPacketReceived;
        _invokeAction = invokeAction;
    }

    public void Start()
    {
        _provider.Start();
    }

    public void Stop()
    {
        _provider.Stop();
    }

    private void OnAdvertisementPacketReceived(object sender, BLEAdvertisementPacketArgs e)
    {
        if (_invokeAction != null)
        {
            _invokeAction(delegate
            {
                ReceivedAdvertisement(e.Data);
            });
        }
        else
        {
            ReceivedAdvertisement(e.Data);
        }
    }

    private void ReceivedAdvertisement(BLEAdvertisementPacket btAdv)
    {
        if (btAdv == null)
        {
            return;
        }

        foreach (Beacon bluetoothBeacon in BluetoothBeacons)
        {
            if (bluetoothBeacon.BluetoothAddress == btAdv.BluetoothAddress)
            {
                bluetoothBeacon.UpdateBeacon(btAdv);
                return;
            }
        }

        Beacon beacon = new Beacon(btAdv);
        BluetoothBeacons.Add(beacon);
        this.BeaconAdded?.Invoke(this, beacon);
    }
}
}
