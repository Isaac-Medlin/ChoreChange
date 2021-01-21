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
using Java.Interop;

namespace ChoreChange
{
    public class ParentAccount : Account
    {
        public ParentAccount(int id, string displayName, string securityQuestion) 
            : base(id, displayName, securityQuestion)
        {
            m_incompleteChores = new List<Chore>();
            m_acceptedChores = new List<Chore>();
            m_awaitingChores = new List<Chore>();
            m_completeChores = new List<Chore>();
            m_children = new List<ChildAccount>();
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
        //functions for children
        public void AddChild(ChildAccount child)
        {
            m_children.Add(child);
        }
        public List<ChildAccount> Children
        {
            get { return m_children; }
        }

        List<Chore> m_incompleteChores;
        List<Chore> m_acceptedChores;
        List<Chore> m_awaitingChores;
        List<Chore> m_completeChores;
        List<ChildAccount> m_children;
    }
}