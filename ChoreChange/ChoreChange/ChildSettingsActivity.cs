using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ChoreChange
{
    [Activity(Label = "ChildSettingsActivity")]
    public class ChildSettingsActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        Button changeName;
        Button changePassword;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ChildSettings);
            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Child_Navigation_Settings;
            navigation.SetOnNavigationItemSelectedListener(this);
            // Create your application here
            Intent intent = this.Intent;
            ChildAccount child = JsonConvert.DeserializeObject<ChildAccount>(intent.GetStringExtra("account"));
            changeName = FindViewById<Button>(Resource.Id.ChildSettingsChangeName);
            changePassword = FindViewById<Button>(Resource.Id.ChildSettingsChangePassword);

            changeName.Click += delegate
            {
                ChildChangeNameDialog diag = new ChildChangeNameDialog(this, child);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };
            changePassword.Click += delegate
            {

            };
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Intent intent = this.Intent;
            ChildAccount child = JsonConvert.DeserializeObject<ChildAccount>(intent.GetStringExtra("account"));
            switch (item.ItemId)
            {
                case Resource.Id.Child_Navigation_Bank:
                    Intent childBank = new Intent(this, typeof(ChildBankActivity)) ;
                    childBank.PutExtra("account", JsonConvert.SerializeObject(child));
                    StartActivity(childBank);
                    return true;
                case Resource.Id.Child_Navigation_Chores:
                    Intent childChore = new Intent(this, typeof(ChildChoreActivity));
                    childChore.PutExtra("account", JsonConvert.SerializeObject(child));
                    StartActivity(childChore);
                    return true;
                case Resource.Id.Child_Navigation_Settings:
                    //Current page
                    return true;
            }
            return false;
        }
    }
}