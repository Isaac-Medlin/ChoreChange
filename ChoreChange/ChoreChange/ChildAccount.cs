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
    public class ChildAccount : Account
    {
        public ChildAccount(int id, string displayName, string securityQuestion, float bank) 
            : base(id, displayName, securityQuestion)
        {
            m_bank = bank;
        }

        public float Bank
        {
            get { return m_bank; }
        }
        private float m_bank;
    }
}