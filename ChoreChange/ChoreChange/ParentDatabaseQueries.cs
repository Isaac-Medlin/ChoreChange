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
         * Purpose: Unlinks child from parent account
         *****************************************************************************************************************/
        public void RemoveChild(ChildAccount child)
        {
            string queryString =
                        "UPDATE dbo.ChildAccounts SET ParentID= null WHERE ID= " + child.id;

            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    int count = command.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }

                connection.Close();
            }
        }

        public void GetCashoutHistory()
        {
            m_parent.Cashouts.Clear();

            string queryString = "SELECT * FROM dbo.CashOutHistory WHERE ParentID=" + m_parent.id;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string childName;
                    float cashoutAmount;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            childName = (string)reader["childName"];
                            cashoutAmount = (float)reader.GetDouble("cashoutAmount");
                            m_parent.AddCashout(new Cashouts(childName, cashoutAmount));
                        }
                        reader.NextResult();
                    }
                    connection.Close();
                }
            }
        }

        /****************************************************************************************************************
* Purpose: Returns all children under a parent account
*****************************************************************************************************************/
        public void GetChildren()
        {
            m_parent.Children.Clear();
            
            string queryString = "SELECT * FROM dbo.ChildAccounts WHERE ParentID=" + m_parent.id;
            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int id;
                    string displayName;
                    string securityQuestion;
                    float bank;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = (int)reader["ID"];
                            displayName = (string)reader["DisplayName"];
                            securityQuestion = (string)reader["SecurityQuestion"];
                            bank = (float)reader.GetDouble("Bank");
                            m_parent.AddChild(new ChildAccount(id, displayName, securityQuestion, bank, m_parent.id));
                        }
                        reader.NextResult();
                    }
                    connection.Close();
                }
            }
        }
        /****************************************************************************************************************
         * Purpose: links a child to a parent account
         *****************************************************************************************************************/
        public bool AddChild(string childUsername)
        {
            bool successful = true;
            string queryString =
                        "UPDATE dbo.ChildAccounts SET ParentID= " + m_parent.id + " WHERE Username= '" + childUsername + "'";

            using (SqlConnection connection = new SqlConnection(m_connection.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    int count = command.ExecuteNonQuery();
                    if (count == 0)
                        successful = false;
                }
                catch (Exception exc)
                {
                    successful = false;
                    Console.WriteLine(exc.Message);
                }

                connection.Close();
            }
            return successful;
        }
        /****************************************************************************************************************
         * Purpose: Deletes all completed Chores off of parent id
         *****************************************************************************************************************/
        public bool DeleteAllCompletedChores()
        {
            bool choreDeleted = true;
            string queryString =
                        "DELETE FROM dbo.Chores WHERE Status= " + (int)Chore.choreStatus.COMPLETED + " AND ParentID=" + m_parent.id;

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
                    choreDeleted = false;
                    Console.WriteLine(exc.Message);
                }

                connection.Close();
            }
            return choreDeleted;
        }
        /****************************************************************************************************************
         * Purpose: Gets a childs name from chore id
         *****************************************************************************************************************/
        public string GetChildNameFomChore(int id)
        {
            string name = null;

            string queryString = "SELECT dbo.ChildAccounts.DisplayName FROM dbo.ChildAccounts INNER JOIN dbo.Chores ON dbo.ChildAccounts.ID = dbo.Chores.CompletedID WHERE dbo.Chores.ChoreID = " + id;
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
                    }
                }
                connection.Close();
            }
            return name;
        }
        /****************************************************************************************************************
         * Purpose: Deletes a chore 
         *****************************************************************************************************************/
        public bool DeleteChore(int choreID)
        {
            bool choreDeleted = true;
            string queryString =
                        "DELETE FROM dbo.Chores WHERE ChoreID=" + choreID;

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
                    choreDeleted = false;
                }

                connection.Close();
            }
            return choreDeleted;
        }
        /****************************************************************************************************************
         * Purpose: Changes chore status and if it gets changed to completed, adds money into childs bank
         *****************************************************************************************************************/
        public void ChangeChoreStatus(Chore chore, Chore.choreStatus status)
        {
            string queryString;
            //if chore was approved updating the childs bank
            if(status == Chore.choreStatus.COMPLETED)
            {
                float bank = 0;
                queryString = "SELECT Bank FROM dbo.ChildAccounts WHERE ID=" + chore.CompletedID;

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
                                bank = (float)reader.GetDouble("Bank");
                            }
                        }
                    }
                    connection.Close();
                }
                bank = bank + chore.payout;

                queryString = "UPDATE dbo.ChildAccounts SET Bank= " + bank + "WHERE ID = " + chore.CompletedID;
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
            //change chore status
            queryString = "UPDATE dbo.Chores SET Status= " + (int)status + " WHERE ChoreID= "+ chore.id;
            
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
                                
                                if(choreStatus != Chore.choreStatus.INCOMPLETE)
                                    completedID = (int)reader["CompletedID"];
                               
                                switch (choreStatus)
                                {
                                    case Chore.choreStatus.INCOMPLETE:
                                        m_parent.AddIncompleteChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus/*, null*/));
                                        break;
                                    case Chore.choreStatus.ACCEPTED:
                                        m_parent.AddAcceptedChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID /*, null*/));
                                        break;
                                    case Chore.choreStatus.AWAITING_APPROVAL:
                                        m_parent.AddAwaitingChore(new Chore(choreID, parentID, choreName, choreDescription, payout, choreStatus, completedID/*, null*/));
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