using Android.App;
using Android.Content;
using Java.Lang;
using NO.Nordicsemi.Android.Dfu;

namespace AndroidDfuSampleApplication;

public class DfuImplementation
{
    private DfuServiceController? controller;
    private NordicProgressListener? progressListener;
    private NordicLogListener? logListener;
    public void Start(string deviceMacAddress, string deviceName, byte[] firmwareZip, INordicDfuCallbacks callbacks){
        
        var firmwareZipPath = WriteByteArrayToTempFile(firmwareZip);
        start(deviceMacAddress, deviceName, firmwareZipPath, callbacks);
    }
    public void Start(string deviceMacAddress, string deviceName, string firmwareZipPath, INordicDfuCallbacks callbacks){
        start(deviceMacAddress, deviceName, firmwareZipPath, callbacks);
    }

    private void start(string deviceMacAddress, string deviceName, string firmwareZipPath, INordicDfuCallbacks callbacks)
    {
        DfuNotificationChannel.Register(new ContextWrapper(Platform.CurrentActivity));
        RegisterListeners();

        var initiator = new DfuServiceInitiator(deviceMacAddress);
        initiator.SetDeviceName(deviceName);
        initiator.SetForceScanningForNewAddressInLegacyDfu(true);
        initiator.SetForeground(true);
        initiator.SetZip(firmwareZipPath);

        controller = initiator.Start(Platform.AppContext, Class.FromType(typeof(DfuService)));
    }

    private void RegisterListeners()
    {
        progressListener = new NordicProgressListener();
        logListener = new NordicLogListener();

        DfuServiceListenerHelper.RegisterProgressListener(Platform.AppContext, progressListener);
        DfuServiceListenerHelper.RegisterLogListener(Platform.AppContext, logListener);
    }
    private void unregisterListeners(){
        if(progressListener is not null){
            DfuServiceListenerHelper.UnregisterProgressListener(Platform.AppContext,progressListener);
            progressListener.Dispose();
            progressListener = null;
        }
        if(logListener is not null){
            DfuServiceListenerHelper.UnregisterLogListener(Platform.AppContext, logListener);
            logListener.Dispose();
            logListener = null;
        }
    }
    public void Stop(){
        controller?.Abort();
        unregisterListeners();
        controller?.Dispose();
    }
    /// <summary>
    /// Writes the byte array to a temporary file location
    /// </summary>
    /// <param name="byteArray"></param>
    /// <returns></returns>
    public string WriteByteArrayToTempFile(byte[] byteArray){
        var tempPath = System.IO.Path.GetTempPath();
        var firmwareZipPath = System.IO.Path.Combine(tempPath,"firmware_w_bootloader.zip");
        System.IO.File.WriteAllBytes(firmwareZipPath,byteArray);

        return firmwareZipPath;
    }
}