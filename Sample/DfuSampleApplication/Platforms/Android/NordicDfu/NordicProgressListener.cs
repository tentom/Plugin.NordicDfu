using NO.Nordicsemi.Android.Dfu;

namespace AndroidDfuSampleApplication;

public class NordicProgressListener : DfuProgressListenerAdapter
{
    public override void OnDeviceConnected(string deviceAddress)
    {
        
    }
    public override void OnDeviceConnecting(string deviceAddress)
    {
        
    }
    public override void OnDeviceDisconnecting(string? deviceAddress)
    {
        
    }
    public override void OnDeviceDisconnected(string deviceAddress)
    {
        
    }
    public override void OnDfuAborted(string deviceAddress)
    {
        
    }
    public override void OnDfuCompleted(string deviceAddress)
    {
        
    }
    public override void OnDfuProcessStarted(string deviceAddress)
    {
        
    }
    public override void OnDfuProcessStarting(string deviceAddress)
    {
        
    }
    public override void OnEnablingDfuMode(string deviceAddress)
    {
        
    }
    public override void OnError(string deviceAddress, int error, int errorType, string? message)
    {
        
    }
    public override void OnFirmwareValidating(string deviceAddress)
    {
        
    }

    public override void OnProgressChanged(string deviceAddress, int percent, float speed, float avgSpeed, int currentPart, int partsTotal){

    }

    
}