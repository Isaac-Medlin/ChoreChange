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
using Newtonsoft.Json;

namespace ChoreChange
{
    [Activity(Label = "LoginActivity", NoHistory = true)]
    public class LoginActivity : Activity
    {
        Button registerButton;
        Button loginButton;
        EditText username;
        EditText password;
        TextView errorText;
        TextView usernameText;
        TextView passwordText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginPage);
            registerButton = FindViewById<Button>(Resource.Id.RegisterButton);
            loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            username = FindViewById<EditText>(Resource.Id.Username);
            password = FindViewById<EditText>(Resource.Id.Password);
            usernameText = FindViewById<TextView>(Resource.Id.UsernameText);
            passwordText = FindViewById<TextView>(Resource.Id.PasswordText);
            errorText = FindViewById<TextView>(Resource.Id.errorWarning);

         
            LoginDatabaseQueries database = new LoginDatabaseQueries();
            registerButton.Click += delegate
            {
                Intent regIntent = new Intent(this, typeof(RegisterActivity));
                StartActivity(regIntent);
            };
            loginButton.Click += delegate
            {
                bool completed = CheckFields();
                if(completed)
                {
                    bool accountexists = database.AccountExists(username.Text);
                    if(!accountexists)
                    {
                        username.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                        usernameText.SetTextColor(Android.Graphics.Color.Red);
                        errorText.Visibility = ViewStates.Visible;
                        errorText.Text = "Username doesn't exist";
                    }
                    else
                    {
                        LoginDatabaseQueries.AccountTypes accountType = database.GetAccountType(username.Text);
                        if(accountType == LoginDatabaseQueries.AccountTypes.PARENT)
                        {
                            ParentAccount parent = database.ParentLogin(password.Text, username.Text);
                            if(parent == null)
                            {
                                password.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                                passwordText.SetTextColor(Android.Graphics.Color.Red);
                                errorText.Visibility = ViewStates.Visible;
                                errorText.Text = "Incorrect Password";
                            }
                            else
                            {
                                Intent parentchore = new Intent(this, typeof(ParentChoresActivity));
                                parentchore.PutExtra("account", JsonConvert.SerializeObject(parent));
                                StartActivity(parentchore);
                            }
                        }
                        if(accountType == LoginDatabaseQueries.AccountTypes.CHILD)
                        {
                            ChildAccount child = database.ChildLogin(password.Text, username.Text);
                            if (child == null)
                            {
                                password.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                                passwordText.SetTextColor(Android.Graphics.Color.Red);
                                errorText.Visibility = ViewStates.Visible;
                                errorText.Text = "Incorrect Password";
                            }
                            else
                            {
                                Intent childchore = new Intent(this, typeof(ChildChoreActivity));
                                childchore.PutExtra("account", JsonConvert.SerializeObject(child));
                                StartActivity(childchore);
                            }
                        }
                    }
                }
            };
        }
        private bool CheckFields()
        {
            username = FindViewById<EditText>(Resource.Id.Username);
            password = FindViewById<EditText>(Resource.Id.Password);
            usernameText = FindViewById<TextView>(Resource.Id.UsernameText);
            passwordText = FindViewById<TextView>(Resource.Id.PasswordText);
            errorText = FindViewById<TextView>(Resource.Id.errorWarning);

            bool completedForm = true;
            if (String.IsNullOrWhiteSpace(username.Text))
            {
                username.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                usernameText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                username.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                usernameText.SetTextColor(Android.Graphics.Color.Black);
            }

            if (String.IsNullOrWhiteSpace(password.Text))
            {
                password.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                passwordText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                password.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                passwordText.SetTextColor(Android.Graphics.Color.Black);
            }        

            if (completedForm == false)
            {
                errorText.Visibility = ViewStates.Visible;
                errorText.Text = "All Fields Must Be Filled Out";
            }
            else
            {
                errorText.Visibility = ViewStates.Gone;
            }             

            return completedForm;

        }

    }
}