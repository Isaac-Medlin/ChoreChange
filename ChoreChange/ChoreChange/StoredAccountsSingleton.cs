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
    class StoredAccountsSingleton
    {
        private static StoredAccountsSingleton instance;
        private StoredAccountsSingleton()
        {
            m_childAccounts = new List<ChildAccount>();
            m_parentAccounts = new List<ParentAccount>();
        }

        public static StoredAccountsSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new StoredAccountsSingleton();
            }
            return instance;
        }
        public void AddChild(ChildAccount acc)
        {
            m_childAccounts.Add(acc);
        }
        public void AddParent(ParentAccount acc)
        {
            m_parentAccounts.Add(acc);
        }
        public List<ParentAccount> ParentAccounts
        {
            get { return m_parentAccounts; }
        }
        public List<ChildAccount> ChildAccounts
        {
            get { return m_childAccounts; }
        }
        private List<ChildAccount> m_childAccounts;
        private List<ParentAccount> m_parentAccounts;
    }

}