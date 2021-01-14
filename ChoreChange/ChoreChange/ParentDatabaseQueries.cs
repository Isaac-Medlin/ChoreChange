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
    class ParentDatabaseQueries
    {
        public ParentDatabaseQueries(ParentAccount parent)
        {
            m_parent = parent;
            m_connection = new ConnectionString();
        }
        /****************************************************************************************************************
         * Purpose: Deletes all completed Chores off of parent id
         *****************************************************************************************************************/
        public void DeleteAllCompletedChores()
        {

        }

        public bool DeleteChore()
        {
            bool choreDeleted = true;

            return choreDeleted;
        }
        /****************************************************************************************************************
         * Adds chore to database based of of creator id
        ******************************************************************************************************************/
        public bool AddChore(string name, string description, float payout /*picture*/)
        {
            string pic = null; //switch with parameter once camera integration is done
            string queryString = null;
            bool choreAdded = true;

            if (pic == null)
            {
                queryString =
                        "INSERT INTO dbo.Chores Values(" + m_parent.id + " , '" + name + "' , '" + description + "' , " + payout + " , " + (int)Chore.choreStatus.INCOMPLETE + " , null , null) ";
            }
            else
            {
                //query string once picture integration is figured out
            }

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
                    choreAdded = false;
                }
                connection.Close();
            }
            return choreAdded;
        }
        /************************************************************************************************************
         * Purpose: Fills out account arrays with chores from databse
         * **********************************************************************************************************/
        public void GetChores()
        {
            m_parent.IncompleteChores.Clear();
            m_parent.AcceptedChores.Clear();
            m_parent.AwaitingChores.Clear();
            m_parent.CompletedChores.Clear();
            foreach (int status in Enum.GetValues(typeof(Chore.choreStatus)))
            {
                string queryString = "SELECT * FROM dbo.Chores WHERE ParentID=" + m_parent.id + " AND Status=" + (int)status;
                using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int choreID;
                        int parentID;
                        string choreName;
                        string choreDescription;
                        float payout;
                        int completedID = 0;
                        //picture pic
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                choreID = (int)reader["ChoreID"];
                                parentID = (int)reader["ParentID"];
                                choreName = (string)reader["Name"];
                                choreDescription = (string)reader["Description"];
                                payout = (float)reader.GetDouble("Payout");
                                Chore.choreStatus choreStatus = (Chore.choreStatus)reader["Status"];

                                if(choreStatus == Chore.choreStatus.COMPLETED)
                                {
                                    completedID = (int)reader["CompletedID"];
                                }
                                switch (choreStatus)
                                {
                                    case Chore.choreStatus.INCOMPLETE:
                                        m_parent.AddIncompleteChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus/*, null*/));
                                        break;
                                    case Chore.choreStatus.ACCEPTED:
                                        m_parent.AddAcceptedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus /*, null*/));
                                        break;
                                    case Chore.choreStatus.AWAITING_APPROVAL:
                                        m_parent.AddAwaitingChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus/*, null*/));
                                        break;
                                    case Chore.choreStatus.COMPLETED:
                                        m_parent.AddCompletedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID /*, null*/));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            reader.NextResult();
                        }
                    }
                    connection.Close();
                }

            }

        }

        ConnectionString m_connection;
        ParentAccount m_parent;
    }
}