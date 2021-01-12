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
    class ConnectionString
    {
        public ConnectionString()
        {
            m_connectionString = "Server = tcp:chore-change-server.database.windows.net,1433; " +
                "Initial Catalog = ChoreChange; Persist Security Info = False; " +
                "User ID = isaac.medlin; Password = Trooper123%; MultipleActiveResultSets = False; Encrypt = True; " +
                "TrustServerCertificate = False;Connection Timeout = 30;";
        }
        public string connectionString
        {
            get { return m_connectionString; }
        }
        private string m_connectionString;

    }
}