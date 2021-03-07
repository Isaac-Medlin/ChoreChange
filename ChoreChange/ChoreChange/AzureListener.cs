using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using WindowsAzure.Messaging.NotificationHubs;
using System.IO;
using Android.Support.V4.App;

namespace ChoreChange
{
    public class AzureListener : AppCompatActivity, INotificationListener
    {
        public void OnPushNotificationReceived(Context context, INotificationMessage message)
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(context, MainActivity.CHANNEL_ID);

            notificationBuilder.SetContentTitle(message.Title)
                        .SetSmallIcon(Resource.Drawable.ChoreChangeLogo)
                        .SetContentText(message.Body)
                        .SetAutoCancel(true)
                        .SetShowWhen(false)
                        .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(context);

            notificationManager.Notify(0, notificationBuilder.Build());

        }
 
    }
}