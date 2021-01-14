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
    public class Chore
    {
        public enum choreStatus
        {
            INCOMPLETE = 0,
            ACCEPTED,
            AWAITING_APPROVAL,
            COMPLETED
        }
        public Chore(int id, int parentID, string name, string description, float payout, choreStatus status, int completedID = -1 /*picture pic*/)
        {
            m_id = id;
            m_parentID = parentID;
            m_name = name;
            m_description = description;
            m_payout = payout;
            m_status = status;
            m_completedID = completedID;
        }
        public int id
        {
            get { return m_id; }
        }
        public int parentID
        {
            get { return m_parentID; }
        }
        public string name
        {
            get { return m_name; }
        }
        public string description
        {
            get { return m_description; }
        }
        public float payout
        {
            get { return m_payout; }
        }
        public choreStatus status
        {
            get { return m_status; }
            set { m_status = value; }
        }
        public int CompletedID
        {
            get { return m_completedID; }
        }
        private int m_id;
        private int m_parentID;
        private string m_name;
        private string m_description;
        private float m_payout;
        private choreStatus m_status;
        private int m_completedID;
        //private picture m_pic;
    }
}