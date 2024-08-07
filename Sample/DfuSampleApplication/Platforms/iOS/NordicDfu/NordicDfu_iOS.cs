using System;
using CoreBluetooth;
using Foundation;

namespace AndroidDfuSampleApplication;
public partial class NordicDfu2 : INordicDfu2
{
    public NordicDfu2()
    {
    }
    CBCentralManager central = null;
    DfuImplementation implementation = null;
    public void StartDfuService(string deviceMacAddress, string deviceName, byte[] firmwareZip, INordicDfuCallbacks callbacks)
    {
        Action<CBPeripheral> startDfuAfterPeripheralScan = dfuDevice =>
        {
            if (dfuDevice == null) { callbacks?.OnError(1, 1, "Unable to find device"); }
            else
            {
                implementation = new DfuImplementation();
                implementation.Start(central, dfuDevice, firmwareZip, callbacks);
            }
        };
        var peripheralScanner = new CBPeripheralScanner(deviceMacAddress, startDfuAfterPeripheralScan);
        central = new CBCentralManager(peripheralScanner, null); // DispatchQueue.CurrentQueue);
    }
    public void StartDfuService(string deviceMacAddress, string deviceName, string firmwareZipPath, INordicDfuCallbacks callbacks)
    {
        Action<CBPeripheral> startDfuAfterPeripheralScan = dfuDevice => 
        {
            implementation = new DfuImplementation();
            implementation.Start(central, dfuDevice, firmwareZipPath, callbacks);
        };
        var peripheralScanner = new CBPeripheralScanner(deviceMacAddress,startDfuAfterPeripheralScan);
        central = new CBCentralManager(peripheralScanner,null);
    }
    public void StopDfuService()
    {
        implementation.Stop();
        Action<CBPeripheral> stopDfuAfterPeripheralScan = dfuDevice =>
        {
            if (dfuDevice != null)
            {
                implementation = new DfuImplementation();
                implementation.Stop(central, dfuDevice);
            }
        };
        var peripheralScanner = new CBPeripheralScanner("", stopDfuAfterPeripheralScan);
        central = new CBCentralManager(peripheralScanner, null); // DispatchQueue.CurrentQueue);
    }
}
class CBPeripheralScanner : CBCentralManagerDelegate
{
    public CBPeripheral DfuDevice;
    string deviceMacAddress;
    Action<CBPeripheral> onScanComplete;
    System.Timers.Timer timer = null;
    object stopScanningLock = new object();

    void stopScanning(CBCentralManager central, CBPeripheral foundDevice)
    {
        try
        {
            central?.StopScan();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error in StopScan: " + ex.GetType().FullName + ": " + ex.Message);
        }
        Action<CBPeripheral> onScanCompleteCopy;
        lock (stopScanningLock)
        {
            onScanCompleteCopy = onScanComplete;
            onScanComplete = null;
        }
        try
        {
            timer?.Stop();
            timer?.Dispose();
            timer = null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error stopping timer: " + ex.GetType().FullName + ": " + ex.Message);
        }
        if (onScanCompleteCopy != null)
        {
            this.DfuDevice = foundDevice;
            onScanCompleteCopy(foundDevice);
        }
    }

    public CBPeripheralScanner(string deviceMacAddress, Action<CBPeripheral> onScanComplete)
    {
        this.deviceMacAddress = deviceMacAddress;
        this.onScanComplete = onScanComplete;
    }

    override public void UpdatedState(CBCentralManager central)
    {
        if (central.State == CBManagerState.PoweredOn)
        {
            //Passing in null scans for all peripherals. Peripherals can be targeted by using service CBUIIDs
            CBUUID[] cbuuids = null;
            central.ScanForPeripherals(cbuuids); //Initiates async calls of DiscoveredPeripheral
                                                 //Timeout after 30 seconds

            timer = new System.Timers.Timer(30 * 1000);
            timer.Elapsed += (sender, e) => stopScanning(central, null);
            timer.Enabled = true;
            timer.Start();
        }
        else
        {
            //Invalid state -- Bluetooth powered down, unavailable, etc.
            System.Console.WriteLine("Bluetooth is not available");
            stopScanning(central, null);
        }
    }
    public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
    {
        Console.WriteLine("Discovered {0}, data {1}, RSSI {2}", peripheral.Name, advertisementData, RSSI);
        var services = (NSArray)advertisementData["kCBAdvDataServiceUUIDs"];
        if (services != null && services.Count > 0)
        {
            var first = services.ValueAt(0);
            var service = ObjCRuntime.Runtime.GetNSObject(first);
            CBUUID uuid = CBUUID.FromString("fe59");
            if (uuid.Equals(service)) // && peripheral.xxxx == deviceMacAddress)
            {
                System.Diagnostics.Debug.WriteLine("Found DFU Device: " + peripheral.Name);
                stopScanning(central, peripheral);
            }
        }
    }
}
//
public partial class NordicDfu : INordicDfu, INordicDfuCallbacks
{
    NordicDfu2 nordicDfu2 = null;
    public void StartDfuService(string deviceMacAddress, string deviceName, byte[] firmwareZip)
    {
        nordicDfu2 = new NordicDfu2();
        nordicDfu2.StartDfuService(deviceMacAddress, deviceName, firmwareZip, this);
    }
    public void StartDfuService(string deviceMacAddress, string deviceName, string firmwareZipPath)
    {
        throw new NotImplementedException();
    }
    public void StopDfuService()
    {
        if (nordicDfu2 == null) nordicDfu2 = new NordicDfu2();
        nordicDfu2.StopDfuService();
    }


    public void OnDeviceConnected() { DfuDeviceConnected?.Invoke(this, new OverskuddDfuDeviceConnectedEventArgs()); }
    public void OnDeviceConnecting() { DfuDeviceConnecting?.Invoke(this, new OverskuddDfuDeviceConnectingEventArgs()); }
    public void OnDeviceDisconnected() { DfuDeviceDisconnected?.Invoke(this, new OverskuddDfuDeviceDisconnectedEventArgs()); }
    public void OnDeviceDisconnecting() { DfuDeviceDisconnecting?.Invoke(this, new OverskuddDfuDeviceDisconnectingEventArgs()); }
    public void OnDfuAborted() { DfuAborted?.Invoke(this, new OverskuddDfuAbortedEventArgs()); }
    public void OnDfuCompleted() { DfuCompleted?.Invoke(this, new OverskuddDfuCompletedEventArgs()); }
    public void OnDfuProcessStarted() { DfuProcessStarted?.Invoke(this, new OverskuddDfuProcessStartedEventArgs()); }
    public void OnDfuProcessStarting() { DfuProcessStarting?.Invoke(this, new OverskuddDfuProcessStartingEventArgs()); }
    public void OnEnablingDfuMode() { DfuModeEnabling?.Invoke(this, new OverskuddDfuEnablingDfuModeEventArgs()); }
    public void OnError(int error, int errorType, string message) { DfuError?.Invoke(this, new OverskuddDfuErrorEventArgs() { Error = error, ErrorType = errorType, Message = message }); }
    public void OnFirmwareValidating() { DfuFirmwareValidating?.Invoke(this, new OverskuddDfuFirmwareValidatingEventArgs()); }
    public void OnProgressChanged(int percent, float speed, float avgSpeed, int currentPart, int partsTotal) { DfuProgressChanged?.Invoke(this, new OverskuddDfuProgressChangedEventArgs() { Percent = percent, Speed = speed, AvgSpeed = avgSpeed, CurrentPart = currentPart, PartsTotal = partsTotal }); }

    public event EventHandler<OverskuddDfuDeviceConnectingEventArgs>? DfuDeviceConnecting;
    public event EventHandler<OverskuddDfuDeviceConnectedEventArgs>? DfuDeviceConnected;
    public event EventHandler<OverskuddDfuProcessStartingEventArgs>? DfuProcessStarting;
    public event EventHandler<OverskuddDfuProcessStartedEventArgs>? DfuProcessStarted;
    public event EventHandler<OverskuddDfuEnablingDfuModeEventArgs>? DfuModeEnabling;
    public event EventHandler<OverskuddDfuProgressChangedEventArgs>? DfuProgressChanged;
    public event EventHandler<OverskuddDfuFirmwareValidatingEventArgs>? DfuFirmwareValidating;
    public event EventHandler<OverskuddDfuDeviceDisconnectingEventArgs>? DfuDeviceDisconnecting;
    public event EventHandler<OverskuddDfuDeviceDisconnectedEventArgs>? DfuDeviceDisconnected;
    public event EventHandler<OverskuddDfuCompletedEventArgs>? DfuCompleted;
    public event EventHandler<OverskuddDfuAbortedEventArgs>? DfuAborted;
    public event EventHandler<OverskuddDfuErrorEventArgs>? DfuError;

    public void OnDeviceConnected(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDeviceConnecting(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDeviceDisconnected(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDeviceDisconnecting(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDfuAborted(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDfuCompleted(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDfuProcessStarted(string deviceAddress) { throw new NotImplementedException(); }
    public void OnDfuProcessStarting(string deviceAddress) { throw new NotImplementedException(); }
    public void OnEnablingDfuMode(string deviceAddress) { throw new NotImplementedException(); }
    public void OnError(string deviceAddress, int error, int errorType, string message) { throw new NotImplementedException(); }
    public void OnFirmwareValidating(string deviceAddress) { throw new NotImplementedException(); }
    public void OnProgressChanged(string deviceAddress, int percent, float speed, float avgSpeed, int currentPart, int partsTotal) { throw new NotImplementedException(); }

    
}

