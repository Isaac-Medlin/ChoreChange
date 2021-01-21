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
    public class ChoreListAdapter : BaseAdapter<Chore>
    {
        public ChoreListAdapter(Activity context, List<Chore> chores) : base()
        {
            m_chores = chores;
            m_context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Chore this[int position]
        {
            get { return m_chores[position]; }
        }
        public override int Count
        {
            get { return m_chores.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Chore chore = m_chores[position];
            View view = convertView;

            if (view == null) // no view to re-use, create new
                view = m_context.LayoutInflater.Inflate(Resource.Layout.CustomChoreList, null);

                view.FindViewById<TextView>(Resource.Id.CustomChoreListTitle).Text = chore.name;
                view.FindViewById<TextView>(Resource.Id.CustomChoreListSubtitle).Text = "Description: " + chore.description;
                view.FindViewById<TextView>(Resource.Id.CustomChoreListPayout).Text = "Payout: $" + chore.payout;

            //var imageBitmap = GetImageBitmapFromUrl(item.ImageURI);
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(imageBitmap);
            return view;
        } 

        List<Chore> m_chores;
        Activity m_context;
    }
}