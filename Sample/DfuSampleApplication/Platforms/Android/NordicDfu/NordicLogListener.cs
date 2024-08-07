
using NO.Nordicsemi.Android.Dfu;

namespace AndroidDfuSampleApplication;

public class NordicLogListener : Java.Lang.Object, IDfuLogListener
{
    public void OnLogEvent(string? deviceAddress, int level, string? message)
    {
        
    }
}