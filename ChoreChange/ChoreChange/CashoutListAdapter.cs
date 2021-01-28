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
    public class CashoutListAdapter : BaseAdapter<Cashouts>
    {
        public CashoutListAdapter(Activity context, List<Cashouts> cashouts) : base()
        {
            m_cashouts = cashouts;
            m_context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Cashouts this[int position]
        {
            get { return m_cashouts[position]; }
        }
        public override int Count
        {
            get { return m_cashouts.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Cashouts cashout = m_cashouts[position];
            View view = convertView;

            if (view == null) // no view to re-use, create new
                view = m_context.LayoutInflater.Inflate(Resource.Layout.CustomCashoutList, null);

            view.FindViewById<TextView>(Resource.Id.CustomCashoutName).Text = cashout.ChildName;
            view.FindViewById<TextView>(Resource.Id.CustomCashoutAmount).Text = "Amount: $" + cashout.CashAmount;

            return view;
        }

        List<Cashouts> m_cashouts;
        Activity m_context;
    }

}