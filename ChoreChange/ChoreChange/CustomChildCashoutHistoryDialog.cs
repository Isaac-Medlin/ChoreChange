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
    public class CustomChildCashoutHistoryDialog : Dialog
    {
        Button cancelButton;
        ListView cashoutHistory;
        public CustomChildCashoutHistoryDialog(Activity activity, ChildAccount creator) : base(activity)
        {
            m_creator = creator;
            m_activity = activity;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.CashoutHistoryDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            ChildDatabaseQueries database = new ChildDatabaseQueries(m_creator);
            database.GetCashoutHistory();

            BaseAdapter<Cashouts> adapter = null;
            cashoutHistory = FindViewById<ListView>(Resource.Id.CashoutHistoryList);
            adapter = new CashoutListAdapter(m_activity, m_creator.Cashouts);
            cashoutHistory.SetAdapter(adapter);
            cancelButton = FindViewById<Button>(Resource.Id.HistoryCancelButton);
            cancelButton.Click += delegate
            {
                base.Dismiss();
            };

        }
        ChildAccount m_creator;
        Activity m_activity;
    }
}