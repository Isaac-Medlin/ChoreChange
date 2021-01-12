using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ChoreChange
{
    class CustomAddChoreDialog : Dialog
    {
        Button submitButton;
        Button cancelButton;

        //Finding entry fields for dialog
        EditText nameEntry;
        EditText descriptionEntry;
        EditText payoutEntry;

        //Finding textFields for dialog -- used if theres no entered text
        TextView nameText;
        TextView descriptionText;
        TextView payoutText;
        TextView moneyText;
        TextView aesterikWarningText;

        //For the loading screen 
        TextView loadingText;
        ProgressBar loadingBar;
        public CustomAddChoreDialog(Activity activity, ParentAccount creator) : base(activity)
        {
            m_creator = creator;
            m_activity = activity;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.AddChoreDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            bool completedForm = false;
            //Finding buttons for dialog
            submitButton = FindViewById<Button>(Resource.Id.ChoreSubmitCreationButton);
            cancelButton = FindViewById<Button>(Resource.Id.ChoreCreationCancelButton);

            //Finding entry fields for dialog
            nameEntry = FindViewById<EditText>(Resource.Id.ChoreNameTextEntry);
            descriptionEntry = FindViewById<EditText>(Resource.Id.ChoreDescriptionTextEntry);
            payoutEntry = FindViewById<EditText>(Resource.Id.ChorePayoutFloatEntry);

            //Finding textFields for dialog -- used if theres no entered text
            nameText = FindViewById<TextView>(Resource.Id.ChoreNameDialogText);
            descriptionText = FindViewById<TextView>(Resource.Id.ChoreDescriptionDialogText);
            payoutText = FindViewById<TextView>(Resource.Id.ChorePayoutDialogText);
            moneyText = FindViewById<TextView>(Resource.Id.ChoreMoneyDialogText);
            aesterikWarningText = FindViewById<TextView>(Resource.Id.Chore_Creation_Aesterik_Warning_Text);

            cancelButton.Click += delegate
            {
                base.Dismiss();
            };

            submitButton.Click += delegate
            {
                completedForm = CheckFields();
                Console.WriteLine("is it completed: {0}", completedForm);
                if (completedForm)
                {
                    string name = nameEntry.Text;
                    string description = descriptionEntry.Text;
                    float payout = float.Parse(payoutEntry.Text);
                    string pic = null;
                    bool choreAdded = true;
                    int id = m_creator.id;

                    ConnectionString conn = new ConnectionString();
                    string queryString = null;
                    if (pic == null)
                    {
                        queryString =
                                "INSERT INTO dbo.Chores Values(" + m_creator.id + " , '" + name + "' , '" + description + "' , " + payout + " , " + (int)Chore.choreStatus.INCOMPLETE + " , null ) ";
                    }
                    else
                    {
                        //query string once picture integration is figured out
                    }
                    
                    using (SqlConnection connection = new SqlConnection(conn.connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("{0}", exc.Message);
                            choreAdded = false;
                        }
                        connection.Close();
                    }

                    //toast to let user know if creation was a faiilure of success
                    if (choreAdded)
                        Toast.MakeText(m_activity, Resource.String.ChoreAdded, ToastLength.Short).Show();
                    else
                        Toast.MakeText(m_activity, Resource.String.ChoreAddedFailure, ToastLength.Short).Show();
                    base.Dismiss();
                }
            };

        }
        /**************************************************************************************************
         * Purpose, make everything invisible and display a "loading screen"
         **************************************************************************************************/
        //private void Loading()
        //{
        //    //Used to make everything invisible for loading screen
        //    nameEntry = FindViewById<EditText>(Resource.Id.ChoreNameTextEntry);
        //    descriptionEntry = FindViewById<EditText>(Resource.Id.ChoreDescriptionTextEntry);
        //    payoutEntry = FindViewById<EditText>(Resource.Id.ChorePayoutFloatEntry);
            
        //    nameText = FindViewById<TextView>(Resource.Id.ChoreNameDialogText);
        //    descriptionText = FindViewById<TextView>(Resource.Id.ChoreDescriptionDialogText);
        //    payoutText = FindViewById<TextView>(Resource.Id.ChorePayoutDialogText);
        //    moneyText = FindViewById<TextView>(Resource.Id.ChoreMoneyDialogText);
        //    aesterikWarningText = FindViewById<TextView>(Resource.Id.Chore_Creation_Aesterik_Warning_Text);

        //    loadingText = FindViewById<TextView>(Resource.Id.addChoreLoadingMessage);
        //    loadingBar = FindViewById<ProgressBar>(Resource.Id.AddChoreLoadingBar);

        //    //make everything else invisible
        //    nameEntry.Visibility = ViewStates.Gone;
        //    descriptionEntry.Visibility = ViewStates.Gone;
        //    payoutEntry.Visibility = ViewStates.Gone;
        //    descriptionText.Visibility = ViewStates.Gone;
        //    payoutText.Visibility = ViewStates.Gone;
        //    moneyText.Visibility = ViewStates.Gone;
        //    aesterikWarningText.Visibility = ViewStates.Gone;

        //    //make loading visible
        //    loadingBar.Visibility = ViewStates.Visible;
        //    loadingText.Visibility = ViewStates.Visible;
        //    Thread.Sleep(1000);
        //}
        /**************************************************************************************************
         * Purpose: Check if all required fields are filled out for a new chore 
         * returns: true or false depending on answer
         ***************************************************************************************************/
        private bool CheckFields()
        {
            //Used to pull text from entry fields
            nameEntry = FindViewById<EditText>(Resource.Id.ChoreNameTextEntry);
            descriptionEntry = FindViewById<EditText>(Resource.Id.ChoreDescriptionTextEntry);
            payoutEntry = FindViewById<EditText>(Resource.Id.ChorePayoutFloatEntry);

            //Finding textFields for dialog -- used if theres no entered text
            nameText = FindViewById<TextView>(Resource.Id.ChoreNameDialogText);
            descriptionText = FindViewById<TextView>(Resource.Id.ChoreDescriptionDialogText);
            payoutText = FindViewById<TextView>(Resource.Id.ChorePayoutDialogText);
            moneyText = FindViewById<TextView>(Resource.Id.ChoreMoneyDialogText);
            aesterikWarningText = FindViewById<TextView>(Resource.Id.Chore_Creation_Aesterik_Warning_Text);

            bool completedForm = true;

            if (String.IsNullOrWhiteSpace(nameEntry.Text))
            {
                nameEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                nameText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                nameEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                nameText.SetTextColor(Android.Graphics.Color.Black);
            }

            if (String.IsNullOrWhiteSpace(descriptionEntry.Text))
            {
                descriptionEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                descriptionText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                descriptionEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                descriptionText.SetTextColor(Android.Graphics.Color.Black);
            }
            if (String.IsNullOrWhiteSpace(payoutEntry.Text))
            {
                payoutEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                payoutText.SetTextColor(Android.Graphics.Color.Red);
                moneyText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                payoutEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                payoutText.SetTextColor(Android.Graphics.Color.Black);
                moneyText.SetTextColor(Android.Graphics.Color.Black);
            }

            if (completedForm == false)
                aesterikWarningText.SetTextColor(Android.Graphics.Color.Red);
            else
                aesterikWarningText.SetTextColor(Android.Graphics.Color.Black);

            return completedForm;

        }
        private ParentAccount m_creator;
        private Activity m_activity;
    }
}