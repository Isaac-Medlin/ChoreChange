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
    [Activity(Label = "RegisterActivity", NoHistory = true)]
    public class RegisterActivity : Activity
    {
        Button cancelButton;
        Button registerButton;
        EditText username;
        EditText password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterPage);
            cancelButton = FindViewById<Button>(Resource.Id.CancelButton);

            cancelButton.Click += delegate
            {
                Intent loginpage = new Intent(this, typeof(LoginActivity));
                StartActivity(loginpage);
            };

        }
    }
}