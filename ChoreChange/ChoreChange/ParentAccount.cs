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
    public class ParentAccount : Account
    {
        public ParentAccount(int id, string displayName, string securityQuestion) 
            : base(id, displayName, securityQuestion)
        {
            m_chores = new List<Chore>();
        }

        public void AddChore(Chore chore)
        {
            m_chores.Add(chore);
        }
        List<Chore> m_chores;

    }
}