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
        TextView usernameText;
        TextView passwordText;
        TextView secAnswerText;
        TextView accountType;
        TextView errorText;
        TextView displayText;

        EditText securityQuestion;
        EditText username;
        EditText password;
        EditText displayName;

        RadioButton parentAcc;
        RadioButton childAcc;
        Spinner securityQuestionSpinner;

        string securityQ = null;
        bool noAccountTypeSelected = true;
        bool parentAccSelected = false;
        bool childAccSelected = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterPage);

            cancelButton = FindViewById<Button>(Resource.Id.CancelButton);
            registerButton = FindViewById<Button>(Resource.Id.RegisterButton);

            securityQuestion = FindViewById<EditText>(Resource.Id.SecurtyQAnswer);
            username = FindViewById<EditText>(Resource.Id.Username);
            password = FindViewById<EditText>(Resource.Id.Password);
            displayName = FindViewById<EditText>(Resource.Id.DisplayName);

            parentAcc = FindViewById<RadioButton>(Resource.Id.ParentButton);
            childAcc = FindViewById<RadioButton>(Resource.Id.ChildButton);

            securityQuestionSpinner = FindViewById<Spinner>(Resource.Id.SecurityQuestion);
            securityQuestionSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.QuestionArray, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            securityQuestionSpinner.Adapter = adapter;

            LoginDatabaseQueries database = new LoginDatabaseQueries();

            registerButton.Click += delegate
            {
                bool completed = CheckFields();
                if (completed)
                {
                    bool accountexists = database.AccountExists(username.Text);
                    if (accountexists)
                    {
                        username.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                        usernameText.SetTextColor(Android.Graphics.Color.Red);
                        errorText.Visibility = ViewStates.Visible;
                        errorText.Text = "Username already exists";
                    }
                    else
                    {
                        if (securityQ == null)
                        {
                            securityQ = securityQuestionSpinner.GetItemAtPosition(0).ToString();
                        }
                        if (parentAccSelected)
                        {
                            bool failed = database.ParentRegister(displayName.Text, username.Text, password.Text, securityQ, securityQuestion.Text);
                            if (!failed)
                            {
                                Toast.MakeText(this, "Account Created", ToastLength.Short).Show();
                                Intent loginpage = new Intent(this, typeof(LoginActivity));
                                StartActivity(loginpage);
                            }
                            else
                            {
                                errorText.Text = "Something Went Wrong!";
                            }
                        }
                        if (childAccSelected)
                        {
                            bool failed = database.ChildRegister(displayName.Text, username.Text, password.Text, securityQ, securityQuestion.Text);
                            if (!failed)
                            {
                                Toast.MakeText(this, "Account Created", ToastLength.Short).Show();
                                Intent loginpage = new Intent(this, typeof(LoginActivity));
                                StartActivity(loginpage);
                            }
                            else
                            {
                                errorText.Text = "Something Went Wrong!";
                            }
                        }
                    }
                }
            };
            parentAcc.Click += delegate
            {
                parentAccSelected = true;
                childAccSelected = false;
                noAccountTypeSelected = false;
            };
            childAcc.Click += delegate
            {
                parentAccSelected = false;
                childAccSelected = true;
                noAccountTypeSelected = false;
            };
            cancelButton.Click += delegate
            {
                Intent loginpage = new Intent(this, typeof(LoginActivity));
                StartActivity(loginpage);
            };

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            securityQ = spinner.GetItemAtPosition(e.Position).ToString();
        }
        private bool CheckFields()
        {
            usernameText = FindViewById<TextView>(Resource.Id.UsernameText);
            passwordText = FindViewById<TextView>(Resource.Id.PasswordText);
            secAnswerText = FindViewById<TextView>(Resource.Id.SecQText);
            accountType = FindViewById<TextView>(Resource.Id.AccTypeText);
            errorText = FindViewById<TextView>(Resource.Id.errorWarning);
            displayText = FindViewById<TextView>(Resource.Id.DisplayNameText);

            securityQuestion = FindViewById<EditText>(Resource.Id.SecurtyQAnswer);
            username = FindViewById<EditText>(Resource.Id.Username);
            password = FindViewById<EditText>(Resource.Id.Password);
            displayName = FindViewById<EditText>(Resource.Id.DisplayName);

            parentAcc = FindViewById<RadioButton>(Resource.Id.ParentButton);
            childAcc = FindViewById<RadioButton>(Resource.Id.ChildButton);

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

            if (String.IsNullOrWhiteSpace(displayName.Text))
            {
                displayName.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                displayText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                displayName.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                displayText.SetTextColor(Android.Graphics.Color.Black);
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

            if (String.IsNullOrWhiteSpace(securityQuestion.Text))
            {
                securityQuestion.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                secAnswerText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                securityQuestion.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                secAnswerText.SetTextColor(Android.Graphics.Color.Black);
            }

            if (noAccountTypeSelected)
            {
                parentAcc.SetTextColor(Android.Graphics.Color.Red);
                childAcc.SetTextColor(Android.Graphics.Color.Red);
                accountType.SetTextColor(Android.Graphics.Color.Red);
            }
            else
            {
                parentAcc.SetTextColor(Android.Graphics.Color.Black);
                childAcc.SetTextColor(Android.Graphics.Color.Black);
                accountType.SetTextColor(Android.Graphics.Color.Black);
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