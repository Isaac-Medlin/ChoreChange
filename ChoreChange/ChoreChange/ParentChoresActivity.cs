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
using Xamarin.Essentials;

namespace ChoreChange
{
    [Activity(Label = "ParentChores")]
    public class ParentChoresActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
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
        ImageButton chat;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ParentChore);

            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Parent_Navigation_Chores;
            navigation.SetOnNavigationItemSelectedListener(this);
            Intent intent = this.Intent;
            ParentAccount parent = JsonConvert.DeserializeObject<ParentAccount>(intent.GetStringExtra("account"));

            ParentDatabaseQueries database = new ParentDatabaseQueries(parent);
            database.GetChores();
            BaseAdapter<Chore> adapter = null;

            choreList = FindViewById<ListView>(Resource.Id.ParentChoreToDoList);
            adapter = new ChoreListAdapter(this, parent.IncompleteChores);
            choreList.SetAdapter(adapter);

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
            chat = FindViewById<ImageButton>(Resource.Id.ParentMessageButton);
            Chore.choreStatus lastChoreTabSelected = Chore.choreStatus.INCOMPLETE;

            toDobutton.Click += delegate
            {
                toDobutton.Enabled = false;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = true;
                deleteAllCompletedChores.Visibility = ViewStates.Gone;
                statusText.Text = "Incompleted Chores:";
                adapter = new ChoreListAdapter(this, parent.IncompleteChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.INCOMPLETE;
            };
            chat.Click += delegate
            {
                ComposeMessageDialog diag = new ComposeMessageDialog(this, parent);
                diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                diag.Show();
            };
            acceptedButton.Click += delegate
            {
                toDobutton.Enabled = true;
                acceptedButton.Enabled = false;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = true;
                deleteAllCompletedChores.Visibility = ViewStates.Gone;
                statusText.Text = "Accepted Chores:";
                adapter = new ChoreListAdapter(this, parent.AcceptedChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.ACCEPTED;
            };
            awaitingApprovalButton.Click += delegate
            {
                toDobutton.Enabled = true;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = false;
                completedButton.Enabled = true;
                deleteAllCompletedChores.Visibility = ViewStates.Gone;
                statusText.Text = "Chores Awaiting Approval:";
                adapter = new ChoreListAdapter(this, parent.AwaitingChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.AWAITING_APPROVAL;
            };
            completedButton.Click += delegate
            {
                toDobutton.Enabled = true;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = false;
                deleteAllCompletedChores.Visibility = ViewStates.Visible;
                statusText.Text = "Completed Chores:";
                adapter = new ChoreListAdapter(this, parent.CompletedChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.COMPLETED;
            };

            deleteAllCompletedChores.Click += delegate
            {
                bool allDeleted = true;
                if (parent.CompletedChores.Count > 0)
                {
                    allDeleted = database.DeleteAllCompletedChores();
                    if (allDeleted)
                    {
                        adapter.NotifyDataSetChanged();
                        database.GetChores();
                        Toast.MakeText(this, Resource.String.AllDeletedSuccess, ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, Resource.String.AllDeletedFailed, ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, Resource.String.CompletedChoresEmpty, ToastLength.Short).Show();
                }
            };

            choreList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs itemClicked)
            {
                if (lastChoreTabSelected == Chore.choreStatus.INCOMPLETE)
                {
                    CustomParentChoreListClickDialog diag = new CustomParentChoreListClickDialog(this, parent, parent.IncompleteChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
                else if (lastChoreTabSelected == Chore.choreStatus.ACCEPTED)
                {
                    CustomParentChoreListClickDialog diag = new CustomParentChoreListClickDialog(this, parent, parent.AcceptedChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
                else if (lastChoreTabSelected == Chore.choreStatus.AWAITING_APPROVAL)
                {
                    CustomParentChoreListClickDialog diag = new CustomParentChoreListClickDialog(this, parent, parent.AwaitingChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
                else if (lastChoreTabSelected == Chore.choreStatus.COMPLETED)
                {
                    CustomParentChoreListClickDialog diag = new CustomParentChoreListClickDialog(this, parent, parent.CompletedChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
                    //current page
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