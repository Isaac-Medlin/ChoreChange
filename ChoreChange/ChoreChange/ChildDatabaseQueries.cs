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
using Java.Sql;

namespace ChoreChange
{
    public class ChildDatabaseQueries
    {
        public ChildDatabaseQueries(ChildAccount child)
        {
            m_child = child;
            m_connection = new ConnectionString();
        }

        //cashouts a child, reseting thier amount to 0 and adding it into the cashout history
        public bool Cashout()
        {
            bool success = true;
            string queryString = "INSERT INTO dbo.CashOutHistory Values(" + m_child.id + "," + m_child.ParentID +",'" + m_child.displayName + "'," + m_child.Bank + ",'" + DateTime.Now.Date.ToString() +  "')";
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
                    success = false;
                    Console.WriteLine("{0}", exc.Message);
                }
                connection.Close();
            }
            if(success)
            {
                queryString = "UPDATE dbo.ChildAccounts SET Bank= " + 0 + " WHERE ID=" + m_child.id;
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
                        success = false;
                        Console.WriteLine("{0}", exc.Message);
                    }
                    connection.Close();
                }
            }
            return success;
        }
        //gets cashout history for a child account
        public void GetCashoutHistory()
        {
            m_child.Cashouts.Clear();

            string queryString = "SELECT * FROM dbo.CashOutHistory WHERE ChildID=" + m_child.id;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string childName;
                    float cashoutAmount;
                    string date;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            childName = (string)reader["childName"];
                            cashoutAmount = (float)reader.GetDouble("cashoutAmount");
                            date = Convert.ToDateTime(reader["Date"]).ToString("MM/dd/yyyy");
                            m_child.AddCashout(new Cashouts(childName, cashoutAmount, date));
                        }
                        reader.NextResult();
                    }
                    connection.Close();
                }
            }
        }
        //gets bank amount for the child
        public void GetBankAmount()
        {
            string queryString = "SELECT Bank FROM dbo.ChildAccounts WHERE ID =" + m_child.id;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            m_child.Bank = (float)reader.GetDouble("Bank");
                        }
                        reader.NextResult();
                    }
                }
                connection.Close();
            }
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
            string picturepath;

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

                            if (!reader.IsDBNull(6))
                                picturepath = (string)reader["Picture"];
                            else
                                picturepath = null;

                            m_child.AddIncompleteChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, -1, picturepath));

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
                                if (!reader.IsDBNull(6))
                                    picturepath = (string)reader["Picture"];
                                else
                                    picturepath = null;

                                switch (ii)
                                {
                                    case 1:
                                        m_child.AddAcceptedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID, picturepath));
                                        break;
                                    case 2:
                                        m_child.AddAwaitingChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID, picturepath));
                                        break;
                                    case 3:
                                        m_child.AddCompletedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID, picturepath));
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