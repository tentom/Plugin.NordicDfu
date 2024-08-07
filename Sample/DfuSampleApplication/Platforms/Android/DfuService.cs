
using Android.App;
using AndroidX.Core.App;
using Java.Lang;
using Kotlin.Reflect;
using NO.Nordicsemi.Android.Dfu;

namespace AndroidDfuSampleApplication;

[Service]
public class DfuService : DfuBaseService
{
    /// <summary>
    /// As a target activity the NotificationActivity is returned, not the MainActivity. This is because
    /// the notification must create a new task:
    /// intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
    /// 
    /// when you press it. You can use NotificationActivity to check whether the new activity
    /// is a root activity (that means no other activity was open earlier) or that some
    /// other activity is already open. In the latter case the NotificationActivity will just be
    /// closed. The system will restore the previous activity. However, if the application has been
    /// closed during upload and you click the notification, a NotificationActivity will
    /// be launched as a root activity. It will create and start the main activity and
    /// terminate itself.
    /// This method may be used to restore the target activity in case the application
    /// was closed or is open. It may also be used to recreate an activity history using
    /// startActivities(...).
    /// </summary>
    protected override Class? NotificationTarget => Java.Lang.Class.FromType(typeof(NotificationActivity));

    /// <summary>
    /// Return true if you want the service to print more logs in LogCat.
    /// </summary>
    /// <returns></returns>
    protected new bool IsDebug(){
        return BuildConfig.Debug;
    }

    /// <summary>
    /// This can be used to Customize the forground service notification
    /// </summary>
    /// <param name="builder"></param>
    protected override void UpdateForegroundNotification(NotificationCompat.Builder builder)
    {
        base.UpdateForegroundNotification(builder);
    }
}