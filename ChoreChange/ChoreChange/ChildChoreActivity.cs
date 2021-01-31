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
    [Activity(Label = "ChildChoreActivity")]
    public class ChildChoreActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        TextView statusText;
        ListView choreList;
        Button toDobutton;
        Button acceptedButton;
        Button awaitingApprovalButton;
        Button completedButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ChildChore);
            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Child_Navigation_Chores;
            navigation.SetOnNavigationItemSelectedListener(this);
            
            Intent intent = this.Intent;
            ChildAccount child = JsonConvert.DeserializeObject<ChildAccount>(intent.GetStringExtra("account"));
            BaseAdapter<Chore> adapter = null;

            choreList = FindViewById<ListView>(Resource.Id.ChildChoreToDoList);
            adapter = new ChoreListAdapter(this, child.IncompleteChores);
            choreList.SetAdapter(adapter);

            statusText = FindViewById<TextView>(Resource.Id.Textbox);
            toDobutton = FindViewById<Button>(Resource.Id.ChildChoreToDOButton);
            acceptedButton = FindViewById<Button>(Resource.Id.ChildChoreAcceptedButton);
            awaitingApprovalButton = FindViewById<Button>(Resource.Id.ChildChoreAwaitingButton);
            completedButton = FindViewById<Button>(Resource.Id.ChildChoreCompletedButton);
            choreList = FindViewById<ListView>(Resource.Id.ChildChoreToDoList);

            ChildDatabaseQueries database = new ChildDatabaseQueries(child);
            database.GetChores();

            Chore.choreStatus lastChoreTabSelected = Chore.choreStatus.INCOMPLETE;
            toDobutton.Click += delegate
            {
                statusText.Text = "Incomplete Chores";
                toDobutton.Enabled = false;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = true;
                adapter = new ChoreListAdapter(this, child.IncompleteChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.INCOMPLETE;
            };
            acceptedButton.Click += delegate
            {
                statusText.Text = "Accepted Chores";
                toDobutton.Enabled = true;
                acceptedButton.Enabled = false;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = true;
                adapter = new ChoreListAdapter(this, child.AcceptedChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.ACCEPTED;
            };
            awaitingApprovalButton.Click += delegate
            {
                statusText.Text = "Awaiting Approval";
                toDobutton.Enabled = true;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = false;
                completedButton.Enabled = true;
                adapter = new ChoreListAdapter(this, child.AwaitingChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.AWAITING_APPROVAL;
            };
            completedButton.Click += delegate
            {
                statusText.Text = "Completed Chores";
                toDobutton.Enabled = true;
                acceptedButton.Enabled = true;
                awaitingApprovalButton.Enabled = true;
                completedButton.Enabled = false;
                adapter = new ChoreListAdapter(this, child.CompletedChores);
                choreList.SetAdapter(adapter);
                lastChoreTabSelected = Chore.choreStatus.COMPLETED;
            };

            choreList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs itemClicked)
            {
                if (lastChoreTabSelected == Chore.choreStatus.INCOMPLETE)
                {
                    CustomChildChoreClickDialog diag = new CustomChildChoreClickDialog(this, child, child.IncompleteChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
                else if (lastChoreTabSelected == Chore.choreStatus.ACCEPTED)
                {
                    CustomChildChoreClickDialog diag = new CustomChildChoreClickDialog(this, child, child.AcceptedChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
                else if (lastChoreTabSelected == Chore.choreStatus.AWAITING_APPROVAL)
                {
                    CustomChildChoreClickDialog diag = new CustomChildChoreClickDialog(this, child, child.AwaitingChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
                else if (lastChoreTabSelected == Chore.choreStatus.COMPLETED)
                {
                    CustomChildChoreClickDialog diag = new CustomChildChoreClickDialog(this, child, child.CompletedChores, itemClicked.Position, adapter);
                    diag.Window.SetSoftInputMode(SoftInput.AdjustResize);
                    diag.Show();
                }
            };

        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Intent intent = this.Intent;
            ChildAccount childAccount = JsonConvert.DeserializeObject<ChildAccount>(intent.GetStringExtra("account"));

            switch (item.ItemId)
            {
                case Resource.Id.Child_Navigation_Bank:
                    Intent childBank = new Intent(this, typeof(ChildBankActivity));
                    childBank.PutExtra("account", JsonConvert.SerializeObject(childAccount));
                    StartActivity(childBank);
                    return true;
                case Resource.Id.Child_Navigation_Chores:
                    //current page
                    return true;
                case Resource.Id.Child_Navigation_Settings:
                    Intent childSettings = new Intent(this, typeof(ChildSettingsActivity));
                    childSettings.PutExtra("account", JsonConvert.SerializeObject(childAccount));
                    StartActivity(childSettings);
                    return true;
            }
            return false;
        }
    }
}