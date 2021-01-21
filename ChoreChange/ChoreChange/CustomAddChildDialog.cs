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
    class CustomAddChildDialog : Dialog
    {
        Button cancelButton;
        Button submitButton;

        TextView nameText;
        TextView warningText;
        TextView aesterikWarning;
        EditText nameEntry;
        

        public CustomAddChildDialog(Activity activity, ParentAccount creator) : base(activity)
        {
            m_creator = creator;
            m_activity = activity;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.AddChildDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            ParentDatabaseQueries database = new ParentDatabaseQueries(m_creator);

            cancelButton = FindViewById<Button>(Resource.Id.ChildAddCancelButton);
            submitButton = FindViewById<Button>(Resource.Id.ChildAddSubmitButton);

            nameEntry = FindViewById<EditText>(Resource.Id.ChildNameTextEntry);
            warningText = FindViewById<TextView>(Resource.Id.AddChildErrorText);
            bool completedForm = false;
            //Finding buttons for dialog

            cancelButton.Click += delegate
            {
                base.Dismiss();
            };

            submitButton.Click += delegate
            {
                completedForm = CheckFields();
                if (completedForm)
                {
                    bool successful = true;
                    string userName = nameEntry.Text;
                    successful = database.AddChild(userName);
                    if(successful == false)
                    {
                        warningText.Visibility = ViewStates.Visible;
                        nameEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                    }
                    else
                    {
                        database.GetChildren();
                        base.Dismiss();
                    }
                }
            };

        }

        /**************************************************************************************************
         * Purpose: Check if all required fields are filled out for a new chore 
         * returns: true or false depending on answer
         ***************************************************************************************************/
        private bool CheckFields()
        {
            ////Used to pull text from entry fields
            warningText = FindViewById<TextView>(Resource.Id.AddChildErrorText);
            nameText = FindViewById<TextView>(Resource.Id.AddChildBlankEntryError);
            aesterikWarning = FindViewById<TextView>(Resource.Id.AddChildBlankEntryError);
            nameEntry = FindViewById<EditText>(Resource.Id.ChildNameTextEntry);
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

            if (completedForm == false)
                aesterikWarning.SetTextColor(Android.Graphics.Color.Red);

            return completedForm;
        }
        private ParentAccount m_creator;
        private Activity m_activity;
    }
}