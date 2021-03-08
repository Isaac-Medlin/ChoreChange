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
    public class ChildChangeNameDialog : Dialog
    {
        Button cancel;
        Button submit;
        EditText newName;
        TextView errortext;
        public ChildChangeNameDialog(Activity activity, ChildAccount account) : base (activity)
        {
            m_activity = activity;
            m_account = account;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            ChildDatabaseQueries db = new ChildDatabaseQueries(m_account);
            SetContentView(Resource.Layout.ChangeNameDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            cancel = FindViewById<Button>(Resource.Id.ChangeNameCancel);
            submit = FindViewById<Button>(Resource.Id.ChangeNameSubmit);
            newName = FindViewById<EditText>(Resource.Id.NewDisplayName);
            errortext = FindViewById<TextView>(Resource.Id.ChangeNameError);
            submit.Click += delegate
            {
                if (string.IsNullOrWhiteSpace(newName.Text))
                {
                    newName.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                    errortext.Visibility = ViewStates.Visible;
                }
                else
                {
                    newName.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                    errortext.Visibility = ViewStates.Gone;
                    db.ChangeDisplayName(newName.Text);
                    base.Dismiss();
                }
            };
            cancel.Click += delegate
            {
                base.Dismiss();
            };

        }

        ChildAccount m_account;
        Activity m_activity;
    }
}