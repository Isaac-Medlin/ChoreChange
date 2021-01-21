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
    [Activity(Label = "ParentSettingsActivity")]
    public class ParentSettingsActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ParentSettings);
            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Parent_Navigation_Settings;
            navigation.SetOnNavigationItemSelectedListener(this);
            // Create your application here
            Intent intent = this.Intent;
            ParentAccount parent = JsonConvert.DeserializeObject<ParentAccount>(intent.GetStringExtra("account"));
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Intent intent = this.Intent;
            ParentAccount parent = JsonConvert.DeserializeObject<ParentAccount>(intent.GetStringExtra("account"));
            switch (item.ItemId)
            {
                case Resource.Id.Parent_Navigation_Children:
                    Intent parentChildren = new Intent(this, typeof(ParentChildrenActivity));
                    parentChildren.PutExtra("account", JsonConvert.SerializeObject(parent));
                    StartActivity(parentChildren);
                    return true;
                case Resource.Id.Parent_Navigation_Chores:
                    Intent parentChores = new Intent(this, typeof(ParentChoresActivity));
                    parentChores.PutExtra("account", JsonConvert.SerializeObject(parent));
                    StartActivity(parentChores);
                    return true;
                case Resource.Id.Parent_Navigation_Settings:
                    //Current page
                    return true;
            }
            return false;
        }
    }
}