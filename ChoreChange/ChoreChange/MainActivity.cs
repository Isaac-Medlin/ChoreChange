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

namespace ChoreChange
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ChoreChangeLogo")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView navigation;
        TextView text;
        Button dataButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ParentChore);
            ConnectionString conn = new ConnectionString();

            navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SelectedItemId = Resource.Id.Parent_Navigation_Chores;
            navigation.SetOnNavigationItemSelectedListener(this);

            dataButton = FindViewById<Button>(Resource.Id.DatabaseButton);
            dataButton.Click += delegate
            {
                text = FindViewById<TextView>(Resource.Id.Textbox);
                System.Console.WriteLine("{0}");
                //string queryString =
                //        "INSERT INTO dbo.ParentAccounts Values('Isaac','isaac.medlin','password','Where did you got to highschool?','Yreka')";
                string queryString =
                        "SELECT * FROM dbo.ParentAccounts";
                using (SqlConnection connection = new SqlConnection(conn.connection))
                {
                    SqlCommand command = new SqlCommand(
                        queryString, connection);
                    connection.Open();
                    //command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        string name = "didn't";
                        string username = "work";
                        while (reader.Read())
                        {
                            System.Console.WriteLine(string.Format("{0}, {1}", reader[0], reader[1]));
                            name = (string)reader["DisplayName"];
                            username = (string)reader["UserName"];
                        }
                        text.Text = "Name: " + name + " UserName: " + username;
                    }
                }
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            text = FindViewById<TextView>(Resource.Id.Textbox);

            switch (item.ItemId)
            {
                case Resource.Id.Parent_Navigation_Children:
                    text.Text = "Child Page";
                    return true;
                case Resource.Id.Parent_Navigation_Chores:
                    text.Text = "Chore Page";
                    return true;
                case Resource.Id.Parent_Navigation_Settings:
                    text.Text = "settings Page";
                    return true;
            }
            return false;
        }
    }
}