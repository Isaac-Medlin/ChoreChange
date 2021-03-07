using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using Newtonsoft.Json;
using System.IO;
using WindowsAzure.Messaging.NotificationHubs;
using Android.Support.V4.App;

namespace ChoreChange
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ChoreChangeLogo", NoHistory = true)]

    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CreateNotificationChannel();

            
            NotificationHub.SetListener(new AzureListener());
            NotificationHub.Start(Application, Constants.NotificationHubName, Constants.ListenConnectionString);

            //var intent = new Intent(this, typeof(MainActivity));
            //intent.AddFlags(ActivityFlags.ClearTop);
            //var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            //var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID);

            //notificationBuilder.SetContentTitle("test title")
            //            .SetSmallIcon(Resource.Drawable.ChoreChangeLogo)
            //            .SetContentText("test message")
            //            .SetAutoCancel(true)
            //            .SetShowWhen(false)
            //            .SetContentIntent(pendingIntent);

            //var notificationManager = NotificationManager.FromContext(this);

            //notificationManager.Notify(0, notificationBuilder.Build());


            RetrieveAccounts();
            StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();
            if (acc.ParentAccounts.Count > 0)
            {
                Intent parentChore = new Intent(this, typeof(ParentChoresActivity));
                parentChore.PutExtra("account", JsonConvert.SerializeObject(acc.ParentAccounts[0]));
                StartActivity(parentChore);
            }
            else if (acc.ChildAccounts.Count > 0)
            {
                Intent ChildChore = new Intent(this, typeof(ChildChoreActivity));
                ChildChore.PutExtra("account", JsonConvert.SerializeObject(acc.ChildAccounts[0]));
                StartActivity(ChildChore);
            }
            else
            {
                Intent LoginActivity = new Intent(this, typeof(LoginActivity));
                StartActivity(LoginActivity);
            }

        }

        public void RetrieveAccounts()
        {
            LoginDatabaseQueries db = new LoginDatabaseQueries();
            StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "parentAccounts.txt");
            if (backingFile == null || !File.Exists(backingFile)){}
            else
            {
                int returnedID = 0;
                string returneddisplay = null;
                string returnedsecurity = null;
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    int ii = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (ii == 0)
                        {
                            returnedID = int.Parse(line);
                            ii++;
                        }
                        else if (ii == 1)
                        {
                            returneddisplay = line;
                            ii++;
                        }
                        else
                        {
                            ii = 0;
                            returnedsecurity = line;
                            acc.AddParent(new ParentAccount(returnedID, returneddisplay, returnedsecurity));
                        };
                    }
                }
            }
            backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "childAccounts.txt");
            if (backingFile == null || !File.Exists(backingFile))
            { }
            else
            {
                int returnedID = 0;
                string returneddisplay = null;
                string returnedsecurity = null;
                float bank = 0; ;
                int parentID = 0;
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    int ii = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (ii == 0)
                        {
                            returnedID = int.Parse(line);
                            ii++;
                        }
                        else if (ii == 1)
                        {
                            returneddisplay = line;
                            ii++;
                        }
                        else if(ii == 2)
                        {
                            returnedsecurity = line;
                            ii++;
                        }
                        else if(ii == 3)
                        {
                            bank = (float)double.Parse(line);
                            ii++;
                        }
                        else
                        {
                            parentID = db.GetChildsParentID(returnedID);
                            ii = 0;
                            acc.AddChild(new ChildAccount(returnedID, returneddisplay, returnedsecurity, bank, parentID));
                        }                        
                    }
                }
            }
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var name = "name";
            var description = "description";
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        internal static readonly string CHANNEL_ID = "MY_CHANNEL";
    }
} 