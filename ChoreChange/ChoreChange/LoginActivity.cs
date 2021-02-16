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
    [Activity(Label = "LoginActivity", NoHistory = true)]
    public class LoginActivity : Activity
    {
        Button registerButton;
        Button loginButton;
        EditText username;
        EditText password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginPage);
            registerButton = FindViewById<Button>(Resource.Id.RegisterButton);
            // Create your application here

            registerButton.Click += delegate
            {
                Intent regIntent = new Intent(this, typeof(RegisterActivity));
                StartActivity(regIntent);
            };
        }
        
    }
}