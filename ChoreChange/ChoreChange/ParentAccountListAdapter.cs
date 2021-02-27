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
    public class ParentAccountListAdapter : BaseAdapter<Account>
    {
        public ParentAccountListAdapter(Activity context, List<ParentAccount> children) : base()
        {
            m_account = children;
            m_context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Account this[int position]
        {
            get { return m_account[position]; }
        }
        public override int Count
        {
            get { return m_account.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ParentAccount account = m_account[position];
            View view = convertView;

            if (view == null) // no view to re-use, create new
                view = m_context.LayoutInflater.Inflate(Resource.Layout.CustomAccountList, null);

            view.FindViewById<TextView>(Resource.Id.AccountName).Text = account.displayName;

            return view;
        }

        List<ParentAccount> m_account;
        Activity m_context;
    }
}