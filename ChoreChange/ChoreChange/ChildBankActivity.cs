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
    [Activity(Label = "ChildBankActivities")]
    public class ChildBankActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        TextView childBank;
        Button cashout;
        Button cashoutHistory;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ChildBank);
            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Child_Navigation_Bank;
            navigation.SetOnNavigationItemSelectedListener(this);

            Intent intent = this.Intent;
            ChildAccount child = JsonConvert.DeserializeObject<ChildAccount>(intent.GetStringExtra("account"));

            ChildDatabaseQueries database = new ChildDatabaseQueries(child);
            database.GetBankAmount();
            childBank = FindViewById<TextView>(Resource.Id.ChildBankAmount);
            cashout = FindViewById<Button>(Resource.Id.CashoutButton);
            cashoutHistory = FindViewById<Button>(Resource.Id.ChildCashoutHistory);

            childBank.Text = "$" + child.Bank;
            cashout.Click += delegate
            {
                if (child.Bank != 0)
                {
                    bool success = database.Cashout();
                    if (success)
                    {
                        Toast.MakeText(this, Resource.String.CashoutSuccessful, ToastLength.Short).Show();
                        childBank.Text = "$0";
                        child.Bank = 0;
                    }
                    else
                    {
                        Toast.MakeText(this, Resource.String.CashoutFailed, ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, Resource.String.InvalidAmountCashout, ToastLength.Short).Show();
                }
            };
            cashoutHistory.Click += delegate
            {
                CustomChildCashoutHistoryDialog diag = new CustomChildCashoutHistoryDialog(this, child);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Intent intent = this.Intent;
            ChildAccount child = JsonConvert.DeserializeObject<ChildAccount>(intent.GetStringExtra("account"));

            switch (item.ItemId)
            {
                case Resource.Id.Child_Navigation_Bank:
                    //cureent  page
                    return true;
                case Resource.Id.Child_Navigation_Chores:
                    Intent childChore = new Intent(this, typeof(ChildChoreActivity));
                    childChore.PutExtra("account", JsonConvert.SerializeObject(child));
                    StartActivity(childChore);
                    return true;
                case Resource.Id.Child_Navigation_Settings:
                    Intent childSettings = new Intent(this, typeof(ChildSettingsActivity));
                    childSettings.PutExtra("account", JsonConvert.SerializeObject(child));
                    StartActivity(childSettings);
                    return true;
            }
            return false;
        }
    }
}