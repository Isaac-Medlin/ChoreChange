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
    public class CustomChildChoreClickDialog : Dialog
    {
        TextView title;
        TextView description;
        TextView payout;
        TextView parentName;
        TextView picTitle;
        ImageView image;
        Button closeButton;
        Button acceptButton;
        public CustomChildChoreClickDialog(Activity activity, ChildAccount account, List<Chore> chores, int position, BaseAdapter<Chore> adapter)
            : base(activity)
        {
            m_activity = activity;
            m_account = account;
            m_chores = chores;
            m_position = position;
            m_adapter = adapter;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.ChildChoreClickDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            if (m_chores[m_position].status == Chore.choreStatus.INCOMPLETE)
            {
                IncompleteDialog();
            }
            else if (m_chores[m_position].status == Chore.choreStatus.ACCEPTED)
            {
                AcceptedDialog();
            }
            else if (m_chores[m_position].status == Chore.choreStatus.AWAITING_APPROVAL)
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
            ChildDatabaseQueries database = new ChildDatabaseQueries(m_account);

            title = FindViewById<TextView>(Resource.Id.ChildClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ChildClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ChildClickChorePayout);
            closeButton = FindViewById<Button>(Resource.Id.ChildClickCloseButton);
            parentName = FindViewById<TextView>(Resource.Id.ChildParentName);
            acceptButton = FindViewById<Button>(Resource.Id.ChildClickConfirmButton);
            image = FindViewById<ImageView>(Resource.Id.ChildChorePicture);
            picTitle = FindViewById<TextView>(Resource.Id.PictureTitle);

            if (m_chores[m_position].PictureBitmap != null)
            {
                picTitle.Visibility = ViewStates.Visible;
                image.SetImageBitmap(m_chores[m_position].PictureBitmap);
            }
            else
            {
                picTitle.Visibility = ViewStates.Gone;
            }

            title.Text = m_chores[m_position].name;
            description.Text = "Description: " + m_chores[m_position].description;
            payout.Text = "Payout: $" + m_chores[m_position].payout.ToString();
            closeButton.Text = "Close";
            acceptButton.Text = "Accept";
            parentName.Visibility = ViewStates.Gone;

            acceptButton.SetTextColor(Android.Graphics.Color.DarkGreen);
            closeButton.SetTextColor(Android.Graphics.Color.Red);
            acceptButton.Visibility = ViewStates.Visible;

            closeButton.Click += delegate
            {
                base.Dismiss();
            };
            acceptButton.Click += delegate
            {
                database.ChangeChoreStatus(m_chores[m_position].id, Chore.choreStatus.ACCEPTED);
                m_adapter.NotifyDataSetChanged();
                database.GetChores();
                base.Dismiss();
            };
        }
        public void AcceptedDialog()
        {
            ChildDatabaseQueries database = new ChildDatabaseQueries(m_account);

            title = FindViewById<TextView>(Resource.Id.ChildClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ChildClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ChildClickChorePayout);
            closeButton = FindViewById<Button>(Resource.Id.ChildClickCloseButton);
            parentName = FindViewById<TextView>(Resource.Id.ChildParentName);
            acceptButton = FindViewById<Button>(Resource.Id.ChildClickConfirmButton);
            image = FindViewById<ImageView>(Resource.Id.ChildChorePicture);
            picTitle = FindViewById<TextView>(Resource.Id.PictureTitle);

            if (m_chores[m_position].PictureBitmap != null)
            {
                picTitle.Visibility = ViewStates.Visible;
                image.SetImageBitmap(m_chores[m_position].PictureBitmap);
            }
            else
            {
                picTitle.Visibility = ViewStates.Gone;
            }

            title.Text = m_chores[m_position].name;
            description.Text = "Description: " + m_chores[m_position].description;
            payout.Text = "Payout: $" + m_chores[m_position].payout.ToString();
            closeButton.Text = "Close";
            acceptButton.Text = "Complete";
            parentName.Visibility = ViewStates.Gone;

            acceptButton.SetTextColor(Android.Graphics.Color.DarkGreen);
            closeButton.SetTextColor(Android.Graphics.Color.Red);
            acceptButton.Visibility = ViewStates.Visible;
            closeButton.Click += delegate
            {
                base.Dismiss();
            };
            acceptButton.Click += delegate
            {
                database.ChangeChoreStatus(m_chores[m_position].id, Chore.choreStatus.AWAITING_APPROVAL);
                m_adapter.NotifyDataSetChanged();
                database.GetChores();
                base.Dismiss();
            };
        }
        public void AwaitingDialog()
        {
            ChildDatabaseQueries database = new ChildDatabaseQueries(m_account);
            string parentname = database.GetParentsName(m_chores[m_position].parentID);
            title = FindViewById<TextView>(Resource.Id.ChildClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ChildClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ChildClickChorePayout);
            closeButton = FindViewById<Button>(Resource.Id.ChildClickCloseButton);
            acceptButton = FindViewById<Button>(Resource.Id.ChildClickConfirmButton);
            parentName = FindViewById<TextView>(Resource.Id.ChildParentName);
            image = FindViewById<ImageView>(Resource.Id.ChildChorePicture);
            picTitle = FindViewById<TextView>(Resource.Id.PictureTitle);

            if (m_chores[m_position].PictureBitmap != null)
            {
                picTitle.Visibility = ViewStates.Visible;
                image.SetImageBitmap(m_chores[m_position].PictureBitmap);
            }
            else
            {
                picTitle.Visibility = ViewStates.Gone;
            }
            title.Text = m_chores[m_position].name;
            description.Text = "Description: " + m_chores[m_position].description;
            payout.Text = "Payout: $" + m_chores[m_position].payout.ToString();
            parentName.Visibility = ViewStates.Visible;
            parentName.Text = "Awaiting Approval From " + parentname;
            closeButton.Text = "Close";
            closeButton.SetTextColor(Android.Graphics.Color.Red);
            acceptButton.Visibility = ViewStates.Gone;

            closeButton.Click += delegate
            {
                base.Dismiss();
            };
        }
        public void CompletedDialog()
        {
            ChildDatabaseQueries database = new ChildDatabaseQueries(m_account);
            string parentname = database.GetParentsName(m_chores[m_position].parentID);

            title = FindViewById<TextView>(Resource.Id.ChildClickChoreTitle);
            description = FindViewById<TextView>(Resource.Id.ChildClickChoreDescription);
            payout = FindViewById<TextView>(Resource.Id.ChildClickChorePayout);
            closeButton = FindViewById<Button>(Resource.Id.ChildClickCloseButton);
            acceptButton = FindViewById<Button>(Resource.Id.ChildClickConfirmButton);
            parentName = FindViewById<TextView>(Resource.Id.ChildParentName);
            image = FindViewById<ImageView>(Resource.Id.ChildChorePicture);
            picTitle = FindViewById<TextView>(Resource.Id.PictureTitle);

            if (m_chores[m_position].PictureBitmap != null)
            {
                picTitle.Visibility = ViewStates.Visible;
                image.SetImageBitmap(m_chores[m_position].PictureBitmap);
            }
            else
            {
                picTitle.Visibility = ViewStates.Gone;
            }
            title.Text = m_chores[m_position].name;
            description.Text = "Description: " + m_chores[m_position].description;
            payout.Text = "Payout: $" + m_chores[m_position].payout.ToString();
            parentName.Visibility = ViewStates.Visible;
            parentName.Text = "Approved by " + parentname;
            closeButton.Text = "Close";

            closeButton.SetTextColor(Android.Graphics.Color.Red);
            acceptButton.Visibility = ViewStates.Gone;
            closeButton.Click += delegate
            {
                base.Dismiss();
            };
        }
        Activity m_activity;
        ChildAccount m_account;
        List<Chore> m_chores;
        int m_position;
        BaseAdapter<Chore> m_adapter;
    }
}