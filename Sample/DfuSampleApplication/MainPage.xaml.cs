
namespace AndroidDfuSampleApplication;

public partial class MainPage : ContentPage
{
	int count = 0;

	INordicDfu nordicDfu;
	string firmwareZipPath;
	byte[] firmwareZip;
	public MainPage(INordicDfu nordicDfu)
	{
		this.nordicDfu = nordicDfu;
		InitializeComponent();
		
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		// Check for Android permissions:
		var bluetoothPermissionStatus = await CheckAndRequestBlePermission();
		if(bluetoothPermissionStatus is not PermissionStatus.Granted){
			return;
		}
		var notificationPermissionSatus = await CheckAndRequestNotificationPermission();
		if(notificationPermissionSatus is not PermissionStatus.Granted){
			// show state to user that the notifications while updating DFU will not be shown
		}
		
		count++;
		firmwareZipPath = GetFirmwareZipPath();
		firmwareZip = GetFirmwareZip();
		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		var callbacks = new NordicDfuCallback();
		var dfuTarg = "E0:4E:8D:75:CE:E1";
		nordicDfu.StartDfuService(dfuTarg,"dfuTarg",firmwareZip);
		SemanticScreenReader.Announce(CounterBtn.Text);
	}
	public string GetFirmwareZipPath()
    {
            string firmwareFileName = "firmware_dfu.zip";
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (string name in currentAssembly.GetManifestResourceNames())
            {
				System.Diagnostics.Debug.WriteLine(name);
                if (name.EndsWith(firmwareFileName, StringComparison.InvariantCultureIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine(name);
					return name;
                }
            }
            throw new Exception("Unable to find resource " + firmwareFileName);
    }
	public byte[] GetFirmwareZip(){
		string firmwareFileName = "firmware_dfu.zip";
		var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (string name in currentAssembly.GetManifestResourceNames())
            {
                if (name.EndsWith(firmwareFileName, StringComparison.InvariantCultureIgnoreCase))
                {
                    using (var stream = currentAssembly.GetManifestResourceStream(name))
                    {
                        var buffer = new byte[32768];
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            int bytesRead;
                            while ((bytesRead = stream?.Read(buffer, 0, buffer.Length) ?? 0) > 0)
                            {
                                memoryStream.Write(buffer, 0, bytesRead);
                            }
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            throw new Exception("Unable to find resource " + firmwareFileName);
	}

	public async Task<PermissionStatus> CheckAndRequestBlePermission()
	{
    	PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

    	if (status == PermissionStatus.Granted)
        	return status;

    	if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
    	{
        	// Prompt the user to turn on in settings
        	// On iOS once a permission has been denied it may not be requested again from the application
        	return status;
    	}

    	if (Permissions.ShouldShowRationale<Permissions.Bluetooth>())
    	{
        	// Prompt the user with additional information as to why the permission is needed
    	}

    	status = await Permissions.RequestAsync<Permissions.Bluetooth>();

    	return status;
	}
	public async Task<PermissionStatus> CheckAndRequestNotificationPermission(){
		PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

		if(status == PermissionStatus.Granted){
			return status;
		}
		if(Permissions.ShouldShowRationale<Permissions.PostNotifications>()){
			// promt user with additional information as to why the permission is needed
		}

		status = await Permissions.RequestAsync<Permissions.PostNotifications>();
		return status;
	}
}

