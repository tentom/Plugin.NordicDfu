using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using AndroidX.Annotations;

namespace AndroidDfuSampleApplication;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = false, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class NotificationActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        // If This activity is the root activity of the task, then the app is not running
        if(IsTaskRoot){
            // start the app before finishing
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            intent.PutExtras(intent);
            StartActivity(intent);
        }
        Finish();
    }
}

public class DfuNotificationChannel
{
    static bool registered = false;
    static object registeredLock = new object();
    [RequiresApi()]
    static public void Register(ContextWrapper contextWrapper)
    {
        if(Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.O){
            bool registeredCopy;
            lock (registeredLock){
                registeredCopy = registered;
                registered = true;
            }
            if(registeredCopy) return;
#pragma warning disable CA1416 // Validate platform compatibility
            var channel = new NotificationChannel("dfu","Device firmware Update",NotificationImportance.Low);
            channel.SetShowBadge(false);
            channel.LockscreenVisibility = NotificationVisibility.Public;
            var notificationManager = contextWrapper.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager?.CreateNotificationChannel(channel);
#pragma warning restore CA1416 // Validate platform compatibility

        }
    }
}
