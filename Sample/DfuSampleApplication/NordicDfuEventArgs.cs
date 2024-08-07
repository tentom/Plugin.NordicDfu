namespace AndroidDfuSampleApplication;
public class OverskuddDfuDeviceConnectingEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuDeviceConnectedEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuProcessStartingEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuProcessStartedEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuEnablingDfuModeEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuProgressChangedEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
    public int Percent { get; set; }
    public float Speed { get; set; }
    public float AvgSpeed { get; set; }
    public float CurrentPart { get; set; }
    public float PartsTotal { get; set; }
}
public class OverskuddDfuFirmwareValidatingEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuDeviceDisconnectingEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuDeviceDisconnectedEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuCompletedEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuAbortedEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
}
public class OverskuddDfuErrorEventArgs : EventArgs
{
    public string DeviceAddress { get; set; } = string.Empty;
    public int Error { get; set; }
    public int ErrorType { get; set; }
    public string Message { get; set; } = string.Empty;
}