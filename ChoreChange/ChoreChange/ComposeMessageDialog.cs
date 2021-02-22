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
using Xamarin.Essentials;

namespace ChoreChange
{
    public class ComposeMessageDialog : Dialog
    {
        Button submit;
        Button cancel;
        EditText number;
        EditText message;
        public ComposeMessageDialog(Activity activity, Account creator) : base(activity)
        {
            m_creator = creator;
            m_activity = activity;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.ComposeMessageDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            number = FindViewById<EditText>(Resource.Id.Number);
            message = FindViewById<EditText>(Resource.Id.Message);
            cancel = FindViewById<Button>(Resource.Id.CancelMessage);
            submit = FindViewById<Button>(Resource.Id.SendMessage);
            cancel.Click += delegate
            {
                base.Dismiss();
            };
            submit.Click += delegate
            {
                Sms.ComposeAsync(new SmsMessage(message.Text, number.Text));
                base.Dismiss();
            };
        }

        Account m_creator;
        Activity m_activity;
    }
}