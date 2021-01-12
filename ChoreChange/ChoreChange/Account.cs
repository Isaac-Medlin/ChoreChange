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
    public class Account
    {
        public Account(int id, string displayname, string securityQuestion)
        {
            m_id = id;
            m_displayName = displayname;
            m_securityQuestion = securityQuestion;
        }
        public int id
        {
            get { return m_id; }
        }
        public string displayName
        {
            get { return m_displayName; }
        }
        public string securityQuestion
        {
            get { return m_securityQuestion; }
        }

        private string m_securityQuestion;
        private int m_id;
        private string m_displayName;
    }
}