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
    class CustomParentChildListClickDialog : Dialog
    {
        TextView childTitle;
        Button cancelButton;
        Button confirmButton;
        public CustomParentChildListClickDialog(Activity activity, ParentAccount creator, List<ChildAccount> children, int position, BaseAdapter<ChildAccount> adapter) : base(activity)
        {
            m_creator = creator;
            m_activity = activity;
            m_children = children;
            m_childPosition = position;
            m_adapter = adapter;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.ParentChildClickDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            ParentDatabaseQueries database = new ParentDatabaseQueries(m_creator);
            childTitle = FindViewById<TextView>(Resource.Id.ParentClickChildTitle);
            cancelButton = FindViewById<Button>(Resource.Id.ParentClickCancelButton);
            confirmButton = FindViewById<Button>(Resource.Id.ParentClickConfirmButton);

            childTitle.Text = "Do you want to remove " + m_children[m_childPosition].displayName + "?";
            cancelButton.Click += delegate
            {
                base.Dismiss();
            };
            confirmButton.Click += delegate
            {
                database.RemoveChild(m_children[m_childPosition]);
                m_children.RemoveAt(m_childPosition);
                m_adapter.NotifyDataSetChanged();
                base.Dismiss();
            };
            
        }
        private Activity m_activity;
        private ParentAccount m_creator;
        private int m_childPosition;
        private List<ChildAccount> m_children;
        private BaseAdapter<ChildAccount> m_adapter;
    }
}