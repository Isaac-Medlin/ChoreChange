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
    public class ChildAccount : Account
    {
        public ChildAccount(int id, string displayName, string securityQuestion, float bank, int parentID) 
            : base(id, displayName, securityQuestion)
        {
            m_bank = bank;
            m_parentID = id;
            m_incompleteChores = new List<Chore>();
            m_acceptedChores = new List<Chore>();
            m_awaitingChores = new List<Chore>();
            m_completeChores = new List<Chore>();
            m_cashouts = new List<Cashouts>();
        }

        public float Bank
        {
            get { return m_bank; }
            set { m_bank = value; }
        }
        public int ParentID
        {
            get { return m_parentID; }
        }
        //functions for inclomplete chores
        public void AddIncompleteChore(Chore chore)
        {
            m_incompleteChores.Add(chore);
        }
        public List<Chore> IncompleteChores
        {
            get { return m_incompleteChores; }
        }
        //functions for accepted chores
        public void AddAcceptedChore(Chore chore)
        {
            m_acceptedChores.Add(chore);
        }
        public List<Chore> AcceptedChores
        {
            get { return m_acceptedChores; }
        }
        //functions for awaiting approval chores
        public void AddAwaitingChore(Chore chore)
        {
            m_awaitingChores.Add(chore);
        }
        public List<Chore> AwaitingChores
        {
            get { return m_awaitingChores; }
        }
        //function for Completed Chores
        public void AddCompletedChore(Chore chore)
        {
            m_completeChores.Add(chore);
        }
        public List<Chore> CompletedChores
        {
            get { return m_completeChores; }
        }

        //functions for cashout history
        public void AddCashout(Cashouts cashout)
        {
            m_cashouts.Add(cashout);
        }
        public List<Cashouts> Cashouts
        {
            get { return m_cashouts; }
        }
        private float m_bank;
        private int m_parentID;
        List<Chore> m_incompleteChores;
        List<Chore> m_acceptedChores;
        List<Chore> m_awaitingChores;
        List<Chore> m_completeChores;
        List<Cashouts> m_cashouts;
    }
}