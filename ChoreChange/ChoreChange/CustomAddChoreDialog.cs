using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media;

namespace ChoreChange
{
    class CustomAddChoreDialog : Dialog
    {
        Button submitButton;
        Button cancelButton;
        Button addPicture;
        //Finding entry fields for dialog
        EditText nameEntry;
        EditText descriptionEntry;
        EditText payoutEntry;

        //Finding textFields for dialog -- used if theres no entered text
        TextView nameText;
        TextView descriptionText;
        TextView payoutText;
        TextView moneyText;
        TextView aesterikWarningText;

        ImageView image;



        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };
        //For the loading screen 
       // TextView loadingText;
        //ProgressBar loadingBar;
        public CustomAddChoreDialog(Activity activity, ParentAccount creator) : base(activity)
        {
            m_creator = creator;
            m_activity = activity;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.AddChoreDialog);
            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            bool completedForm = false;
            //Finding buttons for dialog
            submitButton = FindViewById<Button>(Resource.Id.ChoreSubmitCreationButton);
            cancelButton = FindViewById<Button>(Resource.Id.ChoreCreationCancelButton);
            addPicture = FindViewById<Button>(Resource.Id.ChoreAddPicture);

            //Finding entry fields for dialog
            nameEntry = FindViewById<EditText>(Resource.Id.ChoreNameTextEntry);
            descriptionEntry = FindViewById<EditText>(Resource.Id.ChoreDescriptionTextEntry);
            payoutEntry = FindViewById<EditText>(Resource.Id.ChorePayoutFloatEntry);

            //Finding textFields for dialog -- used if theres no entered text
            nameText = FindViewById<TextView>(Resource.Id.ChoreNameDialogText);
            descriptionText = FindViewById<TextView>(Resource.Id.ChoreDescriptionDialogText);
            payoutText = FindViewById<TextView>(Resource.Id.ChorePayoutDialogText);
            moneyText = FindViewById<TextView>(Resource.Id.ChoreMoneyDialogText);
            aesterikWarningText = FindViewById<TextView>(Resource.Id.Chore_Creation_Aesterik_Warning_Text);

            image = FindViewById<ImageView>(Resource.Id.ReturnedPicture);

            m_activity.RequestPermissions(permissionGroup, 0);
            cancelButton.Click += delegate
            {
                base.Dismiss();
            };
            addPicture.Click += delegate
            {
                TakePhoto();
            };
            submitButton.Click += delegate
            {
                completedForm = CheckFields();
                if (completedForm)
                {
                    string name = nameEntry.Text;
                    string description = descriptionEntry.Text;
                    float payout = float.Parse(payoutEntry.Text);

                    bool choreAdded;
                    ParentDatabaseQueries database = new ParentDatabaseQueries(m_creator);
                    if (m_inputStream != null)
                    {
                        Upload(m_inputStream);
                    }
                    choreAdded = database.AddChore(name, description, payout, URL);
                    database.GetChores();

                    if (choreAdded)
                    {
                        Toast.MakeText(m_activity, Resource.String.ChoreAdded, ToastLength.Short).Show();
                    }
                    else
                        Toast.MakeText(m_activity, Resource.String.ChoreAddedFailure, ToastLength.Short).Show();
                    base.Dismiss();
                }
            };

        }
        private async void Upload(Stream stream)
        {
            ConnectionString conn = new ConnectionString();
            try
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(conn.storageConnString);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("images");
                //await container.CreateIfNotExistsAsync();
                string name = Guid.NewGuid().ToString();
                CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}.png");
                blockBlob.Properties.ContentType = "image/png";
                URL = blockBlob.Uri.OriginalString;
                await blockBlob.UploadFromStreamAsync(stream);
                //Toast.MakeText(m_activity, "Image uploaded to Blob Storage Successfully!", ToastLength.Short).Show();                
            }
            catch (Exception e)
            {
                Toast.MakeText(m_activity, "" + e.ToString(), ToastLength.Short);
            }
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                CompressionQuality = 40,
                Name = "chorepic.jpg",
                Directory = "sample"
            }) ;
            image = FindViewById<ImageView>(Resource.Id.ReturnedPicture);
            if (file == null)
                return;

            
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            image.SetImageBitmap(bitmap);

            byte[] bitmapData;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                bitmapData = stream.ToArray();
            }
            m_inputStream = new MemoryStream(bitmapData);
        }
        /**************************************************************************************************
         * Purpose: Check if all required fields are filled out for a new chore 
         * returns: true or false depending on answer
         ***************************************************************************************************/
        private bool CheckFields()
        {
            //Used to pull text from entry fields
            nameEntry = FindViewById<EditText>(Resource.Id.ChoreNameTextEntry);
            descriptionEntry = FindViewById<EditText>(Resource.Id.ChoreDescriptionTextEntry);
            payoutEntry = FindViewById<EditText>(Resource.Id.ChorePayoutFloatEntry);

            //Finding textFields for dialog -- used if theres no entered text
            nameText = FindViewById<TextView>(Resource.Id.ChoreNameDialogText);
            descriptionText = FindViewById<TextView>(Resource.Id.ChoreDescriptionDialogText);
            payoutText = FindViewById<TextView>(Resource.Id.ChorePayoutDialogText);
            moneyText = FindViewById<TextView>(Resource.Id.ChoreMoneyDialogText);
            aesterikWarningText = FindViewById<TextView>(Resource.Id.Chore_Creation_Aesterik_Warning_Text);

            bool completedForm = true;

            if (String.IsNullOrWhiteSpace(nameEntry.Text))
            {
                nameEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                nameText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                nameEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                nameText.SetTextColor(Android.Graphics.Color.Black);
            }

            if (String.IsNullOrWhiteSpace(descriptionEntry.Text))
            {
                descriptionEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                descriptionText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                descriptionEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                descriptionText.SetTextColor(Android.Graphics.Color.Black);
            }
            if (String.IsNullOrWhiteSpace(payoutEntry.Text))
            {
                payoutEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
                payoutText.SetTextColor(Android.Graphics.Color.Red);
                moneyText.SetTextColor(Android.Graphics.Color.Red);
                completedForm = false;
            }
            else
            {
                payoutEntry.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
                payoutText.SetTextColor(Android.Graphics.Color.Black);
                moneyText.SetTextColor(Android.Graphics.Color.Black);
            }

            if (completedForm == false)
                aesterikWarningText.SetTextColor(Android.Graphics.Color.Red);
            else
                aesterikWarningText.SetTextColor(Android.Graphics.Color.Black);

            return completedForm;

        }
        private ParentAccount m_creator;
        private Activity m_activity;
        public string URL 
        {
            get { return m_url; }
            set { m_url = value; }
        }
        private string m_url;
        private MemoryStream m_inputStream;
    }
}