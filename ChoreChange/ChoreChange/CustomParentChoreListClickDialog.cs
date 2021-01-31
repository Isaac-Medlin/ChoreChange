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
    public class CustomParentChoreListClickDialog : Dialog
    {
        TextView title;
        TextView description;
        TextView payout;
        TextView child;
        Button cancelOrDeny;
        Button acceptOrDelete;
        Button delete;
        public CustomParentChoreListClickDialog(Activity activity, ParentAccount parent, List<Chore> chores, int chorePosition, BaseAdapter<Chore> adapter) : base(activity)
        {
            m_activity = activity;
            m_parent = parent;
            m_chores = chores;
            m_chorePosition = chorePosition;
            m_adapter = adapter;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.ParentChoreClickDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            if(m_chores[m_chorePosition].status == Chore.choreStatus.INCOMPLETE)
            {
                IncompleteDialog();
            }
            else if(m_chores[m_chorePosition].status == Chore.choreStatus.ACCEPTED)
            {
                AcceptedDialog();
            }
            else if(m_chores[m_chorePosition].status == Chore.choreStatus.AWAITING_APPROVAL)
            {
                AwaitingDialog();
            }
            else
            {
                CompletedDialog();
            }
        }
        public void IncompleteDialog()
        {
            ConnectionString conn = new ConnectionString();
            ParentDatabaseQueries database = new ParentDatabaseQueries(m_parent);
            bool deleteSuccessful = true;

            title = FindViewById<TextView>(Resource.Id.ParentClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ParentClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ParentClickChorePayout);
            cancelOrDeny = FindViewById<Button>(Resource.Id.ParentClickCancelButton);
            acceptOrDelete = FindViewById<Button>(Resource.Id.ParentClickConfirmButton);
            delete = FindViewById<Button>(Resource.Id.ParentClickDeleteButton);

            title.Text = m_chores[m_chorePosition].name;
            description.Text = "Description: " + m_chores[m_chorePosition].description;
            payout.Text = "Payout: $" + m_chores[m_chorePosition].payout.ToString();
            cancelOrDeny.Text = "Cancel";
            acceptOrDelete.Text = "Delete";
            acceptOrDelete.SetTextColor(Android.Graphics.Color.Red);
            delete.Visibility = ViewStates.Gone;

            cancelOrDeny.Click += delegate
            {
                base.Dismiss();
            };
            acceptOrDelete.Click += delegate
            {
                deleteSuccessful = database.DeleteChore(m_chores[m_chorePosition].id);
                if (deleteSuccessful)
                {
                    m_chores.RemoveAt(m_chorePosition);
                    m_adapter.NotifyDataSetChanged();
                    Toast.MakeText(m_activity, Resource.String.ChoreDeleted, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(m_activity, Resource.String.ChoreDeletedFailed, ToastLength.Short).Show();
                }
                base.Dismiss();
            };
        }
        public void AcceptedDialog()
        {
            ConnectionString conn = new ConnectionString();
            ParentDatabaseQueries database = new ParentDatabaseQueries(m_parent);
            bool deleteSuccessful = true;

            title = FindViewById<TextView>(Resource.Id.ParentClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ParentClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ParentClickChorePayout);
            cancelOrDeny = FindViewById<Button>(Resource.Id.ParentClickCancelButton);
            acceptOrDelete = FindViewById<Button>(Resource.Id.ParentClickConfirmButton);
            delete = FindViewById<Button>(Resource.Id.ParentClickDeleteButton);
            child = FindViewById<TextView>(Resource.Id.ParentClickChoreChild);

            string childName = database.GetChildNameFomChore(m_chores[m_chorePosition].id);
            title.Text = m_chores[m_chorePosition].name;
            description.Text = "Description: " + m_chores[m_chorePosition].description;
            payout.Text = "Payout: $" + m_chores[m_chorePosition].payout.ToString();
            child.Text = "Accepted By: " + childName;
            cancelOrDeny.Text = "Cancel";
            acceptOrDelete.Text = "Delete";
            acceptOrDelete.SetTextColor(Android.Graphics.Color.Red);
            child.Visibility = ViewStates.Visible;
            delete.Visibility = ViewStates.Gone;

            cancelOrDeny.Click += delegate
            {
                base.Dismiss();
            };
            acceptOrDelete.Click += delegate
            {
                deleteSuccessful = database.DeleteChore(m_chores[m_chorePosition].id);
                if (deleteSuccessful)
                {
                    m_chores.RemoveAt(m_chorePosition);
                    m_adapter.NotifyDataSetChanged();
                    Toast.MakeText(m_activity, Resource.String.ChoreDeleted, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(m_activity, Resource.String.ChoreDeletedFailed, ToastLength.Short).Show();
                }
                base.Dismiss();
            };
        }
        public void AwaitingDialog()
        {
            ConnectionString conn = new ConnectionString();
            ParentDatabaseQueries database = new ParentDatabaseQueries(m_parent);
            bool deleteSuccessful = true;

            title = FindViewById<TextView>(Resource.Id.ParentClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ParentClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ParentClickChorePayout);
            cancelOrDeny = FindViewById<Button>(Resource.Id.ParentClickCancelButton);
            acceptOrDelete = FindViewById<Button>(Resource.Id.ParentClickConfirmButton);
            delete = FindViewById<Button>(Resource.Id.ParentClickDeleteButton);
            child = FindViewById<TextView>(Resource.Id.ParentClickChoreChild);

            string childName = database.GetChildNameFomChore(m_chores[m_chorePosition].id);
            title.Text = m_chores[m_chorePosition].name;
            description.Text = "Description: " + m_chores[m_chorePosition].description;
            payout.Text = "Payout: $" + m_chores[m_chorePosition].payout.ToString();
            child.Text = "Accepted By: " + childName;
            cancelOrDeny.Text = "Deny";
            acceptOrDelete.Text = "Approve";
            delete.Text = "Delete";
            acceptOrDelete.SetTextColor(Android.Graphics.Color.Red);
            acceptOrDelete.SetTextColor(Android.Graphics.Color.DarkGreen);
            cancelOrDeny.SetTextColor(Android.Graphics.Color.Red);
            delete.SetTextColor(Android.Graphics.Color.Red);
            delete.Visibility = ViewStates.Visible;
            child.Visibility = ViewStates.Visible;

            
            cancelOrDeny.Click += delegate
            {
                database.ChangeChoreStatus(m_chores[m_chorePosition], Chore.choreStatus.ACCEPTED);
                m_adapter.NotifyDataSetChanged();
                database.GetChores();
                base.Dismiss();
            };

            acceptOrDelete.Click += delegate    
            {
                //update childs bank
                database.ChangeChoreStatus(m_chores[m_chorePosition], Chore.choreStatus.COMPLETED);
                m_adapter.NotifyDataSetChanged();
                database.GetChores();
                base.Dismiss();
            };

            delete.Click += delegate
            {
                deleteSuccessful = database.DeleteChore(m_chores[m_chorePosition].id);
                if (deleteSuccessful)
                {
                    m_adapter.NotifyDataSetChanged();
                    database.GetChores();
                    Toast.MakeText(m_activity, Resource.String.ChoreDeleted, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(m_activity, Resource.String.ChoreDeletedFailed, ToastLength.Short).Show();
                }
                base.Dismiss();
            };
        }
        public void CompletedDialog()
        {
            ConnectionString conn = new ConnectionString();
            ParentDatabaseQueries database = new ParentDatabaseQueries(m_parent);
            bool deleteSuccessful = true;

            title = FindViewById<TextView>(Resource.Id.ParentClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ParentClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ParentClickChorePayout);
            cancelOrDeny = FindViewById<Button>(Resource.Id.ParentClickCancelButton);
            acceptOrDelete = FindViewById<Button>(Resource.Id.ParentClickConfirmButton);
            delete = FindViewById<Button>(Resource.Id.ParentClickDeleteButton);
            child = FindViewById<TextView>(Resource.Id.ParentClickChoreChild);

            string childName = database.GetChildNameFomChore(m_chores[m_chorePosition].id);
            title.Text = m_chores[m_chorePosition].name;
            description.Text = "Description: " + m_chores[m_chorePosition].description;
            payout.Text = "Payout: $" + m_chores[m_chorePosition].payout.ToString();
            child.Text = "Completed By: " + childName;
            cancelOrDeny.Text = "Cancel";
            acceptOrDelete.Text = "Delete";
            acceptOrDelete.SetTextColor(Android.Graphics.Color.Red);
            delete.Visibility = ViewStates.Gone;
            child.Visibility = ViewStates.Visible;

            cancelOrDeny.Click += delegate
            {
                base.Dismiss();
            };
            acceptOrDelete.Click += delegate
            {
                deleteSuccessful = database.DeleteChore(m_chores[m_chorePosition].id);
                if (deleteSuccessful)
                {
                    m_chores.RemoveAt(m_chorePosition);
                    m_adapter.NotifyDataSetChanged();
                    Toast.MakeText(m_activity, Resource.String.ChoreDeleted, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(m_activity, Resource.String.ChoreDeletedFailed, ToastLength.Short).Show();
                }
                base.Dismiss();
            };
        }
        private Activity m_activity;
        private ParentAccount m_parent;
        private int m_chorePosition;
        private List<Chore> m_chores;
        private BaseAdapter<Chore> m_adapter;

    }
}