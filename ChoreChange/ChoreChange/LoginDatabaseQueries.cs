using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            }
            return account;
        }
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
            }
            return account;
        }
        ConnectionString m_connection;
    }
}