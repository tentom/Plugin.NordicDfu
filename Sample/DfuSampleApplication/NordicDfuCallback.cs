namespace AndroidDfuSampleApplication;

public partial class NordicDfuCallback : INordicDfuCallbacks, INordicDfuEvents
{
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

    public void OnDeviceConnected()
    {
        DfuDeviceConnected?.Invoke(this,new OverskuddDfuDeviceConnectedEventArgs());
    }

    public void OnDeviceConnecting()
    {
        DfuDeviceConnecting?.Invoke(this,new OverskuddDfuDeviceConnectingEventArgs());
    }

    public void OnDeviceDisconnected()
    {
        DfuDeviceDisconnected?.Invoke(this,new OverskuddDfuDeviceDisconnectedEventArgs());
    }

    public void OnDeviceDisconnecting()
    {
        DfuDeviceDisconnecting?.Invoke(this,new OverskuddDfuDeviceDisconnectingEventArgs());
    }

    public void OnDfuAborted()
    {
        DfuAborted?.Invoke(this,new OverskuddDfuAbortedEventArgs());
    }

    public void OnDfuCompleted()
    {
        DfuCompleted?.Invoke(this,new OverskuddDfuCompletedEventArgs());
    }

    public void OnDfuProcessStarted()
    {
        DfuProcessStarted?.Invoke(this, new OverskuddDfuProcessStartedEventArgs());
    }

    public void OnDfuProcessStarting()
    {
        DfuProcessStarting?.Invoke(this,new OverskuddDfuProcessStartingEventArgs());
    }

    public void OnEnablingDfuMode()
    {
        DfuModeEnabling?.Invoke(this,new OverskuddDfuEnablingDfuModeEventArgs());
    }

    public void OnError(int error, int errorType, string message)
    {
        DfuError?.Invoke(this, new OverskuddDfuErrorEventArgs(){Error = error , ErrorType = errorType, Message = message});
    }

    public void OnFirmwareValidating()
    {
        DfuFirmwareValidating?.Invoke(this,new OverskuddDfuFirmwareValidatingEventArgs());
    }

    public void OnProgressChanged(int percent, float speed, float avgSpeed, int currentPart, int partsTotal)
    {
        DfuProgressChanged?.Invoke(this,new OverskuddDfuProgressChangedEventArgs()
                                    {
                                        Percent = percent,
                                        Speed = speed,
                                        AvgSpeed = avgSpeed,
                                        CurrentPart = currentPart,
                                        PartsTotal = partsTotal});
    }
}
