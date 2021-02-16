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

        EditText sequrityQuestion;
        EditText username;
        EditText password;

        RadioButton parentAcc;
        RadioButton childAcc;
        Spinner securityQuestionSpinner;

        bool noAccountTypeSelected = true;
        bool parentAccSelected = false;
        bool childAccSelected = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterPage);

            cancelButton = FindViewById<Button>(Resource.Id.CancelButton);
            registerButton = FindViewById<Button>(Resource.Id.RegisterButton);

            sequrityQuestion = FindViewById<EditText>(Resource.Id.SecurtyQAnswer);
            username = FindViewById<EditText>(Resource.Id.Username);
            password = FindViewById<EditText>(Resource.Id.Password);

            parentAcc = FindViewById<RadioButton>(Resource.Id.ParentButton);
            childAcc = FindViewById<RadioButton>(Resource.Id.ChildButton);

            securityQuestionSpinner = FindViewById<Spinner>(Resource.Id.SecurityQuestion);
            securityQuestionSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.QuestionArray, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            securityQuestionSpinner.Adapter = adapter;


            registerButton.Click += delegate
            {
                bool completed = CheckFields();
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
            string toast = string.Format("Selected question is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
        private bool CheckFields()
        {
            usernameText = FindViewById<TextView>(Resource.Id.UsernameText);
            passwordText = FindViewById<TextView>(Resource.Id.PasswordText);
            secAnswerText = FindViewById<TextView>(Resource.Id.SecQText);
            accountType = FindViewById<TextView>(Resource.Id.AccTypeText);
            errorText = FindViewById<TextView>(Resource.Id.errorWarning);

            sequrityQuestion = FindViewById<EditText>(Resource.Id.SecurtyQAnswer);
            username = FindViewById<EditText>(Resource.Id.Username);
            password = FindViewById<EditText>(Resource.Id.Password);

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

            if (String.IsNullOrWhiteSpace(sequrityQuestion.Text))
            {
                sequrityQuestion.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                secAnswerText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                sequrityQuestion.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                secAnswerText.SetTextColor(Android.Graphics.Color.Black);
            }

            if(noAccountTypeSelected)
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