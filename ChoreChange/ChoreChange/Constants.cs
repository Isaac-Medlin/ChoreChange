using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ChoreChange
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://chorechange.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=CrojaMtz6kY1GN1T1RN/QbqcVox/6DrYQLtfcTFN8eg=";
        public const string NotificationHubName = "ChorechangNotifications";
    }
}