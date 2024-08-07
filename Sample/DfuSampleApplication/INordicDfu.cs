    
    
namespace AndroidDfuSampleApplication;

public interface INordicDfu
{
    void StopDfuService();

    void StartDfuService(string deviceMacAddress, string deviceName, byte[] firmwareZip);
    void StartDfuService(string deviceMacAddress, string deviceName, string firmwareZipPath);
    event EventHandler<OverskuddDfuDeviceConnectingEventArgs> DfuDeviceConnecting;
    event EventHandler<OverskuddDfuDeviceConnectedEventArgs> DfuDeviceConnected;
    event EventHandler<OverskuddDfuProcessStartingEventArgs> DfuProcessStarting;
    event EventHandler<OverskuddDfuProcessStartedEventArgs> DfuProcessStarted;
    event EventHandler<OverskuddDfuEnablingDfuModeEventArgs> DfuModeEnabling;
    event EventHandler<OverskuddDfuProgressChangedEventArgs> DfuProgressChanged;
    event EventHandler<OverskuddDfuFirmwareValidatingEventArgs> DfuFirmwareValidating;
    event EventHandler<OverskuddDfuDeviceDisconnectingEventArgs> DfuDeviceDisconnecting;
    event EventHandler<OverskuddDfuDeviceDisconnectedEventArgs> DfuDeviceDisconnected;
    event EventHandler<OverskuddDfuCompletedEventArgs> DfuCompleted;
    event EventHandler<OverskuddDfuAbortedEventArgs> DfuAborted;
    event EventHandler<OverskuddDfuErrorEventArgs> DfuError;

    void OnDeviceConnecting(string deviceAddress);
    void OnDeviceConnected(string deviceAddress);
    void OnDfuProcessStarting(string deviceAddress);
    void OnDfuProcessStarted(string deviceAddress);
    void OnEnablingDfuMode(string deviceAddress);
    void OnProgressChanged(string deviceAddress, int percent, float speed, float avgSpeed, int currentPart, int partsTotal);
    void OnFirmwareValidating(string deviceAddress);
    void OnDeviceDisconnecting(string deviceAddress);
    void OnDeviceDisconnected(string deviceAddress);
    void OnDfuCompleted(string deviceAddress);
    void OnDfuAborted(string deviceAddress);
    void OnError(string deviceAddress, int error, int errorType, string message);
}

public interface INordicDfuEvents{
    event EventHandler<OverskuddDfuDeviceConnectingEventArgs>? DfuDeviceConnecting;
    event EventHandler<OverskuddDfuDeviceConnectedEventArgs>? DfuDeviceConnected;
    event EventHandler<OverskuddDfuProcessStartingEventArgs>? DfuProcessStarting;
    event EventHandler<OverskuddDfuProcessStartedEventArgs>? DfuProcessStarted;
    event EventHandler<OverskuddDfuEnablingDfuModeEventArgs>? DfuModeEnabling;
    event EventHandler<OverskuddDfuProgressChangedEventArgs>? DfuProgressChanged;
    event EventHandler<OverskuddDfuFirmwareValidatingEventArgs>? DfuFirmwareValidating;
    event EventHandler<OverskuddDfuDeviceDisconnectingEventArgs>? DfuDeviceDisconnecting;
    event EventHandler<OverskuddDfuDeviceDisconnectedEventArgs>? DfuDeviceDisconnected;
    event EventHandler<OverskuddDfuCompletedEventArgs>? DfuCompleted;
    event EventHandler<OverskuddDfuAbortedEventArgs>? DfuAborted;
    event EventHandler<OverskuddDfuErrorEventArgs>? DfuError;
}
public interface INordicDfuCallbacks
{   
    void OnDeviceConnecting();
    void OnDeviceConnected();
    void OnDfuProcessStarting();
    void OnDfuProcessStarted();
    void OnEnablingDfuMode();
    void OnProgressChanged(int percent, float speed, float avgSpeed, int currentPart, int partsTotal);
    void OnFirmwareValidating();
    void OnDeviceDisconnecting();
    void OnDeviceDisconnected();
    void OnDfuCompleted();
    void OnDfuAborted();
    void OnError(int error, int errorType, string message);
}
public interface INordicDfu2
{
    void StartDfuService(string deviceMacAddress, string deviceName, byte[] firmwareZip, INordicDfuCallbacks callbacks);
    void StartDfuService(string deviceMacAddress, string deviceName, string firmwareZipPath, INordicDfuCallbacks callbacks);
    void StopDfuService();

}