using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System.Data.Sql;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using Android.Content;
using Newtonsoft.Json;
using System.IO;

namespace ChoreChange
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ChoreChangeLogo", NoHistory = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetrieveAccounts();
            StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();
            if(acc.ParentAccounts.Count > 0)
            {
                Intent parentChore = new Intent(this, typeof(ParentChoresActivity));
                parentChore.PutExtra("account", JsonConvert.SerializeObject(acc.ParentAccounts[0]));
                StartActivity(parentChore);
            }
            else if(acc.ChildAccounts.Count > 0)
            {
                Intent ChildChore = new Intent(this, typeof(ChildChoreActivity));
                ChildChore.PutExtra("account", JsonConvert.SerializeObject(acc.ChildAccounts[0]));
                StartActivity(ChildChore);
            }
            else
            {
                Intent LoginActivity = new Intent(this, typeof(LoginActivity));
                StartActivity(LoginActivity);
            }

        }

        public void RetrieveAccounts()
        {
            StoredAccountsSingleton acc = StoredAccountsSingleton.GetInstance();
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "parentAccounts.txt");
            if (backingFile == null || !File.Exists(backingFile)){}
            else
            {
                int returnedID = 0;
                string returneddisplay = null;
                string returnedsecurity = null;
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    int ii = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (ii == 0)
                        {
                            returnedID = int.Parse(line);
                            ii++;
                        }
                        else if (ii == 1)
                        {
                            returneddisplay = line;
                            ii++;
                        }
                        else
                        {
                            ii = 0;
                            returnedsecurity = line;
                            acc.AddParent(new ParentAccount(returnedID, returneddisplay, returnedsecurity));
                        };
                    }
                }
            }
            backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "childAccounts.txt");
            if (backingFile == null || !File.Exists(backingFile))
            { }
            else
            {
                int returnedID = 0;
                string returneddisplay = null;
                string returnedsecurity = null;
                float bank = 0; ;
                int parentID = 0;
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    int ii = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (ii == 0)
                        {
                            returnedID = int.Parse(line);
                            ii++;
                        }
                        else if (ii == 1)
                        {
                            returneddisplay = line;
                            ii++;
                        }
                        else if(ii == 2)
                        {
                            returnedsecurity = line;
                            ii++;
                        }
                        else if(ii == 3)
                        {
                            bank = (float)double.Parse(line);
                            ii++;
                        }
                        else
                        {
                            parentID = int.Parse(line);
                            ii = 0;
                            acc.AddChild(new ChildAccount(returnedID, returneddisplay, returnedsecurity, bank, parentID));
                        }                        
                    }
                }
            }
        }
    }
} 