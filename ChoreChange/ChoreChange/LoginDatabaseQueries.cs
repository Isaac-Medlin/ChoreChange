using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    public class LoginDatabaseQueries
    {
        public enum AccountTypes
        {
            PARENT = 0,
            CHILD
        }
        public LoginDatabaseQueries()
        {
            m_connection = new ConnectionString();
        }

        //checks if the account exists in database
        public bool AccountExists(string username)
        {
            bool exists = true;
            string queryString =
                        "SELECT * FROM dbo.Accounts WHERE username = '"+ username +"'";

            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        exists = false;
                    }
                }

                connection.Close();
            }
            return exists;
        }
        //gets the account type 
        public AccountTypes GetAccountType(string username)
        {
            AccountTypes type = AccountTypes.CHILD;
            string queryString =
                        "SELECT * FROM dbo.Accounts WHERE username ='" + username + "'";

            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            type = (AccountTypes)reader["accountType"];
                        }
                    }
                }

                connection.Close();
            }
            return type;
        }
        //logins a parent account
        public ParentAccount ParentLogin(string password, string username)
        {
            ParentAccount account = null;
            string queryString =
                        "SELECT * FROM dbo.ParentAccounts WHERE username = '" + username + "'";

            int id = 0;
            string displayName = null;
            string retrievedPassword = null;
            string securityQ = null;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            id = (int)reader["ID"];
                            displayName = (string)reader["DisplayName"];
                            retrievedPassword = (string)reader["Password"];
                            securityQ = (string)reader["SecurityQuestion"];
                        }
                    }
                }

                connection.Close();
            }
            if (password == retrievedPassword)
            { 
                account = new ParentAccount(id, displayName, securityQ);
                StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();
                bool alreadyexists = false;
                foreach(ParentAccount storedAccount in acc.ParentAccounts)
                {
                    if (id == storedAccount.id)
                        alreadyexists = true;
                }
                if(alreadyexists == false)
                {
                    acc.AddParent(account);
                    var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "parentAccounts.txt");
                    try
                    {
                        using (var writer = File.AppendText(backingFile))
                        {
                            writer.WriteLine(account.id.ToString());
                            writer.WriteLine(account.displayName);
                            writer.WriteLine(account.securityQuestion);
                        }
                    }
                    catch (Exception r)
                    {
                        System.Console.WriteLine(r.Message);
                    }
                }
            }
            return account;
        }
        //logins a child account
        public ChildAccount ChildLogin(string password, string username)
        {
            ChildAccount account = null;
            string queryString =
                        "SELECT * FROM dbo.ChildAccounts WHERE username = '" + username + "'";

            int id = 0;
            string displayName = null;
            string retrievedPassword = null;
            string securityQ = null;
            int parentID = 0;
            float bank = 0;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            id = (int)reader["ID"];
                            displayName = (string)reader["DisplayName"];
                            retrievedPassword = (string)reader["Password"];
                            bank = (float)reader.GetDouble("Bank");
                            parentID = (int)reader["ParentID"];
                            securityQ = (string)reader["SecurityQuestion"];
                        }
                    }
                }

                connection.Close();
            }
            if (password == retrievedPassword)
            {
                account = new ChildAccount(id, displayName, securityQ, bank, parentID);
                StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();
                bool alreadyexists = false;
                foreach (ChildAccount storedAccount in acc.ChildAccounts)
                {
                    if (id == storedAccount.id)
                        alreadyexists = true;
                }
                if (alreadyexists == false)
                {
                    acc.AddChild(account);
                    var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "childAccounts.txt");
                    try
                    {
                        using (var writer = File.AppendText(backingFile))
                        {
                            writer.WriteLine(account.id.ToString());
                            writer.WriteLine(account.displayName);
                            writer.WriteLine(account.securityQuestion);
                            writer.WriteLine(account.Bank.ToString());
                            writer.WriteLine(account.ParentID.ToString());
                        }
                    }
                    catch (Exception r)
                    {
                        System.Console.WriteLine(r.Message);
                    }
                }
            }
            return account;
        }
        
        //logins a parent account
        public bool ParentRegister(string displayName, string username, string password, string securityQuestion, string securityAnswer)
        { 
            string queryString =
                        "INSERT INTO dbo.ParentAccounts Values('" + displayName + "','" + username + "','" + password + "','" + securityQuestion +"','" + securityAnswer + "') ";

            bool failed = false;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("{0}", exc.Message);
                    failed = true;
                }
                connection.Close();
            }
            queryString =
                        "INSERT INTO dbo.Accounts Values('" + username + "'," + (int)AccountTypes.PARENT + ") ";            
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("{0}", exc.Message);
                }
                connection.Close();
            }
            return failed;
        }
        //registers a child account
        public bool ChildRegister(string displayName, string username, string password, string securityQuestion, string securityAnswer)
        {
            string queryString =
                        "INSERT INTO dbo.ChildAccounts Values('" + displayName + "','" + username + "','" + password + "'," + 0 
                        +"," + 0 + ",'" + securityQuestion + "','" + securityAnswer + "') ";

            bool failed = false;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("{0}", exc.Message);
                    failed = true;
                }
                connection.Close();
            }
            queryString =
                        "INSERT INTO dbo.Accounts Values('" + username + "'," + (int)AccountTypes.CHILD + ") ";
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("{0}", exc.Message);
                }
                connection.Close();
            }
            return failed;
        }
        ConnectionString m_connection;
    }
}