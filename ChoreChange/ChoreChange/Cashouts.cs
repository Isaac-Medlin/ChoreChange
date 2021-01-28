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
    public class Cashouts
    {
        public Cashouts(string childName, float cashAmount)
        {
            m_childName = childName;
            m_cashAmount = cashAmount;
        }
        public float CashAmount
        {
            get { return m_cashAmount; }
        }

        public string ChildName
        {
            get { return m_childName; }
        }
        private string m_childName;
        private float m_cashAmount;
    }
}