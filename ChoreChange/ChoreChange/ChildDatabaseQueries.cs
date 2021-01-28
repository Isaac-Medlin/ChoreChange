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
    public class ChildDatabaseQueries
    {
        public ChildDatabaseQueries(ChildAccount child)
        {
            m_child = child;
            m_connection = new ConnectionString();
        }
        //Gets parents name from a chore
        public string GetParentsName(int parentID)
        {
            string name = null;
            string queryString = "SELECT DisplayName FROM dbo.ParentAccounts WHERE ID="+ parentID;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //picture pic
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            name = (string)reader["DisplayName"];
                        }
                        reader.NextResult();
                    }
                }
                connection.Close();
            }
            return name;
        }
        //changes the chore status
        public void ChangeChoreStatus(int choreID, Chore.choreStatus status)
        {
            string queryString = "UPDATE dbo.Chores SET Status= " + (int)status + " WHERE ChoreID= " + choreID;

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
            queryString = "UPDATE dbo.Chores SET CompletedID= " + m_child.id + " WHERE ChoreID= " + choreID;

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
        }
        //gets all chores for a child 
        public void GetChores()
        {
            m_child.IncompleteChores.Clear();
            m_child.AcceptedChores.Clear();
            m_child.AwaitingChores.Clear();
            m_child.CompletedChores.Clear();
            int choreID;
            int parentID;
            string choreName;
            string choreDescription;
            float payout;
            int completedID = 0;
            string queryString = "SELECT * FROM dbo.Chores WHERE ParentID=" + m_child.ParentID + " AND Status=" + 0;

            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
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

                            if (choreStatus != Chore.choreStatus.INCOMPLETE)
                                completedID = (int)reader["CompletedID"];

                            m_child.AddIncompleteChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus/*, null*/));

                        }
                        reader.NextResult();
                    }
                }
                connection.Close();
            }

            for (int ii = 1; ii <= 3; ii++)
            {
                queryString = "SELECT * FROM dbo.Chores WHERE ParentID=" + m_child.ParentID + " AND Status=" + ii + "AND CompletedID=" + m_child.id;
                using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                                completedID = (int)reader["CompletedID"];

                                switch (ii)
                                {
                                    case 1:
                                        m_child.AddAcceptedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID/*, null*/));
                                        break;
                                    case 2:
                                        m_child.AddAwaitingChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID/*, null*/));
                                        break;
                                    case 3:
                                        m_child.AddCompletedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID/*, null*/));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            reader.NextResult();
                        }
                        connection.Close();
                    }
                }
            }


        }
        ConnectionString m_connection;
        ChildAccount m_child;
    }
}