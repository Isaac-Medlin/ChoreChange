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
    class ChildListAdapter : BaseAdapter<ChildAccount>
    {
        public ChildListAdapter(Activity context, List<ChildAccount> children) : base()
        {
            m_children = children;
            m_context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ChildAccount this[int position]
        {
            get { return m_children[position]; }
        }
        public override int Count
        {
            get { return m_children.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ChildAccount child = m_children[position];
            View view = convertView;

            if (view == null) // no view to re-use, create new
                view = m_context.LayoutInflater.Inflate(Resource.Layout.CustomChildList, null);

            view.FindViewById<TextView>(Resource.Id.CustomChildListName).Text = child.displayName;
            view.FindViewById<TextView>(Resource.Id.CustomChildListBank).Text = "Bank: $" + child.Bank;

            return view;
        }

        List<ChildAccount> m_children;
        Activity m_context;
    }
}