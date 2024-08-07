
using Android.OS;
using Android.OS.Strictmode;

namespace AndroidDfuSampleApplication;
public partial class NordicDfuAndroidImpl : INordicDfu2
{
    public NordicDfuAndroidImpl()
    {

    }
    private DfuImplementation? implementation;
    public void StartDfuService(string deviceMacAddress, string deviceName, byte[] firmwareZip, INordicDfuCallbacks callbacks)
    {
        implementation = new DfuImplementation();
        implementation.Start(deviceMacAddress, deviceName, firmwareZip, callbacks);
    }
    public void StartDfuService(string deviceMacAddress, string deviceName, string firmwarezipPath,INordicDfuCallbacks callbacks)
    {
        implementation = new DfuImplementation();
        implementation.Start(deviceMacAddress, deviceName, firmwarezipPath, callbacks);
    }
    public void StopDfuService()
    {
        implementation?.Stop();
        implementation = null;
    }
}
public partial class NordicDfu : INordicDfu
{
    public INordicDfuEvents callbacks;

    NordicDfuAndroidImpl impl;

    public NordicDfu(){
        callbacks = new NordicDfuCallback();
        impl = new NordicDfuAndroidImpl();
    } 
    // Theese events can potentially be removed as the callbacks include them.
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

    public void StopDfuService()
    {
        impl.StopDfuService();
    }

    public void StartDfuService(string deviceMacAddress, string deviceName, byte[] firmwareZip)
    {
        impl.StartDfuService(deviceMacAddress, deviceName, firmwareZip, (NordicDfuCallback)callbacks);
    }
    public void StartDfuService(string deviceMacAddress, string deviceName, string firmwareZipPath)
    {
        impl.StartDfuService(deviceMacAddress, deviceName, firmwareZipPath, (NordicDfuCallback)callbacks);
    }

    // Theese can potentially be removed as the callbacks allready contains them. 
    public void OnDeviceConnecting(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDeviceConnected(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDfuProcessStarting(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDfuProcessStarted(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnEnablingDfuMode(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnProgressChanged(string deviceAddress, int percent, float speed, float avgSpeed, int currentPart, int partsTotal)
    {
        throw new NotImplementedException();
    }

    public void OnFirmwareValidating(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDeviceDisconnecting(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDeviceDisconnected(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDfuCompleted(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnDfuAborted(string deviceAddress)
    {
        throw new NotImplementedException();
    }

    public void OnError(string deviceAddress, int error, int errorType, string message)
    {
        throw new NotImplementedException();
    }
}