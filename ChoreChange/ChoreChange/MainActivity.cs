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

namespace ChoreChange
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ChoreChangeLogo")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        TextView statusText;
        FloatingActionButton addChoreButton;
        ListView choreList;
        Button toDobutton;
        Button acceptedButton;
        Button awaitingApprovalButton;
        Button completedButton;
        Button deleteAllCompletedChores;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ParentChore);

            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Parent_Navigation_Chores;
            navigation.SetOnNavigationItemSelectedListener(this);

            ParentAccount parent = new ParentAccount(1, "Isaac", "Where did you go to highschool?");

            ParentDatabaseQueries database = new ParentDatabaseQueries(parent);
            database.GetChores();

            choreList = FindViewById<ListView>(Resource.Id.ParentChoreToDoList);
            choreList.Adapter = new ChoreListAdapter(this, parent.IncompleteChores);

            addChoreButton = FindViewById<FloatingActionButton>(Resource.Id.AddChoreButton);
            addChoreButton.Click += delegate
            {
                CustomAddChoreDialog diag = new CustomAddChoreDialog(this, parent);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };
            statusText = FindViewById<TextView>(Resource.Id.Textbox);

            toDobutton = FindViewById<Button>(Resource.Id.ParentChoreToDOButton);
            acceptedButton = FindViewById<Button>(Resource.Id.ParentChoreAcceptedButton);
            awaitingApprovalButton = FindViewById<Button>(Resource.Id.ParentChoreAwaitingButton);
            completedButton = FindViewById<Button>(Resource.Id.ParentChoreCompletedButton);
            deleteAllCompletedChores = FindViewById<Button>(Resource.Id.DeleteAllCompletedChores);

            List<Chore> lastChoreTabSelected = new List<Chore>();
            lastChoreTabSelected = parent.IncompleteChores;

            toDobutton.Click += delegate
            {
                toDobutton.Enabled = false;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = true;
                deleteAllCompletedChores.Visibility = ViewStates.Gone;
                statusText.Text = "Incompleted Chores:";
                choreList.Adapter = new ChoreListAdapter(this, parent.IncompleteChores);
                lastChoreTabSelected = parent.IncompleteChores;
            };
            acceptedButton.Click += delegate
            {
                toDobutton.Enabled = true;
                acceptedButton.Enabled = false;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = true;
                deleteAllCompletedChores.Visibility = ViewStates.Gone;
                statusText.Text = "Accepted Chores:";
                choreList.Adapter = new ChoreListAdapter(this, parent.AcceptedChores);
                lastChoreTabSelected = parent.AcceptedChores;
            };
            awaitingApprovalButton.Click += delegate
            {
                toDobutton.Enabled = true;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = false;
                completedButton.Enabled = true;
                deleteAllCompletedChores.Visibility = ViewStates.Gone;
                statusText.Text = "Chores Awaiting Approval:";
                choreList.Adapter = new ChoreListAdapter(this, parent.AwaitingChores);
                lastChoreTabSelected = parent.AwaitingChores;
            };
            completedButton.Click += delegate
            {
                toDobutton.Enabled = true;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = false;
                deleteAllCompletedChores.Visibility = ViewStates.Visible;
                statusText.Text = "Completed Chores:";                
                choreList.Adapter = new ChoreListAdapter(this, parent.CompletedChores);
                lastChoreTabSelected = parent.CompletedChores;
            };

            deleteAllCompletedChores.Click += delegate
            {
                database.DeleteAllCompletedChores();
            };

            choreList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs itemClicked)
            {
                Chore selectedChore = lastChoreTabSelected[itemClicked.Position];
                Console.WriteLine();
                Toast.MakeText(this, selectedChore.name, ToastLength.Short).Show();
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public bool OnNavigationItemSelected(IMenuItem item)
        {
            statusText = FindViewById<TextView>(Resource.Id.Textbox);

            switch (item.ItemId)
            {
                case Resource.Id.Parent_Navigation_Children:
                    statusText.Text = "Child Page";
                    return true;
                case Resource.Id.Parent_Navigation_Chores:
                    statusText.Text = "Chore Page";
                    return true;
                case Resource.Id.Parent_Navigation_Settings:
                    statusText.Text = "settings Page";
                    return true;
            }
            return false;
        }
    }
}