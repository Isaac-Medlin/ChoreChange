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
    class SwitchAccountDialog : Dialog
    {
        Button close;
        Button addAccount;

        Button childAccounts;
        Button parentAccounts;

        ListView accountList;
        public SwitchAccountDialog(Activity activity) : base(activity)
        {
            m_activity = activity;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.SwitchAccountDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            close = FindViewById<Button>(Resource.Id.SwitchAccountCloseButton);
            addAccount = FindViewById<Button>(Resource.Id.AddAccountButton);
            parentAccounts = FindViewById<Button>(Resource.Id.ParentAccountsButton);
            childAccounts = FindViewById<Button>(Resource.Id.ChildAccountsButton);

            StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();

            accountList = FindViewById<ListView>(Resource.Id.AccountsList);
            BaseAdapter<Account> adapter = new ParentAccountListAdapter(m_activity, acc.ParentAccounts);
            accountList.SetAdapter(adapter);

            parentAccounts.Enabled = false;
            string lastTabSelected = "parent";
            parentAccounts.Click += delegate
            {
                parentAccounts.Enabled = false;
                childAccounts.Enabled = true;
                lastTabSelected = "parent";
                adapter = new ParentAccountListAdapter(m_activity, acc.ParentAccounts);
                accountList.SetAdapter(adapter);
            };
            childAccounts.Click += delegate
            {
                parentAccounts.Enabled = true;
                childAccounts.Enabled = false;
                lastTabSelected = "child";
                adapter = new ChildAccountListAdapter(m_activity, acc.ChildAccounts);
                accountList.SetAdapter(adapter);
            };
            close.Click += delegate
            {
                base.Dismiss();
            };
            addAccount.Click += delegate
            {
                Intent intent = new Intent(m_activity, typeof(LoginActivity));
                m_activity.StartActivity(intent);
            };
            accountList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs itemClicked)
            {
                Console.WriteLine("got here");
                if (lastTabSelected == "parent")
                {
                    Intent parentChore = new Intent(m_activity, typeof(ParentChoresActivity));
                    parentChore.PutExtra("account", JsonConvert.SerializeObject(acc.ParentAccounts[itemClicked.Position]));
                    m_activity.StartActivity(parentChore);
                }
                else
                {
                    Intent childChore = new Intent(m_activity, typeof(ChildChoreActivity));
                    childChore.PutExtra("account", JsonConvert.SerializeObject(acc.ChildAccounts[itemClicked.Position]));
                    m_activity.StartActivity(childChore);
                }
            };
        }
        Activity m_activity;
    }
}