using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System.Data.Sql;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using Android.Content;
using Newtonsoft.Json;

namespace ChoreChange
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ChoreChangeLogo")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            ParentAccount parent = new ParentAccount(1, "Isaac", "Where did you go to highschool?");
            Intent parentChore = new Intent(this, typeof(ParentChoresActivity));
            parentChore.PutExtra("account", JsonConvert.SerializeObject(parent));
            StartActivity(parentChore);
        }
    }
}