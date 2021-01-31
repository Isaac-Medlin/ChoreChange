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
    [Activity(Label = "ParentChildrenActivity")]
    public class ParentChildrenActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        Button addChoreButton;
        Button viewChasoutHistory;
        ListView childList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ParentChildren);
            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Parent_Navigation_Children;
            navigation.SetOnNavigationItemSelectedListener(this);

            Intent intent = this.Intent;
            ParentAccount parent = JsonConvert.DeserializeObject<ParentAccount>(intent.GetStringExtra("account"));

            ParentDatabaseQueries database = new ParentDatabaseQueries(parent);
            BaseAdapter<ChildAccount> adapter = null;
            database.GetChildren();
            childList = FindViewById<ListView>(Resource.Id.ParentChildList);
            adapter = new ChildListAdapter(this, parent.Children);
            childList.SetAdapter(adapter);

            addChoreButton = FindViewById<Button>(Resource.Id.ParentAddChildButton);
            addChoreButton.Click += delegate
            {
                CustomAddChildDialog diag = new CustomAddChildDialog(this, parent);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };

            childList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs itemClicked)
            {
                CustomParentChildListClickDialog diag = new CustomParentChildListClickDialog(this, parent, parent.Children, itemClicked.Position, adapter);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };
            viewChasoutHistory = FindViewById<Button>(Resource.Id.ParentCashoutHistory);
            viewChasoutHistory.Click += delegate
            {
                CustomParentCashOutHistoryDialog diag = new CustomParentCashOutHistoryDialog(this, parent);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Intent intent = this.Intent;
            ParentAccount parent = JsonConvert.DeserializeObject<ParentAccount>(intent.GetStringExtra("account"));
            switch (item.ItemId)
            {
                case Resource.Id.Parent_Navigation_Children:
                    //current page
                    return true;
                case Resource.Id.Parent_Navigation_Chores:
                    Intent parentChores = new Intent(this, typeof(ParentChoresActivity));
                    parentChores.PutExtra("account", JsonConvert.SerializeObject(parent));
                    StartActivity(parentChores);
                    return true;
                case Resource.Id.Parent_Navigation_Settings:
                    Intent parentSettings = new Intent(this, typeof(ParentSettingsActivity));
                    parentSettings.PutExtra("account", JsonConvert.SerializeObject(parent));
                    StartActivity(parentSettings);
                    return true;
            }
            return false;
        }
    }
}