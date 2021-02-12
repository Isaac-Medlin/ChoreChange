  using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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
        public Chore(int id, int parentID, string name, string description, float payout, choreStatus status, int completedID, string picturepath)
        {
            m_id = id;
            m_parentID = parentID;
            m_name = name;
            m_description = description;
            m_payout = payout;
            m_status = status;
            m_completedID = completedID;
            if (picturepath != null)
            {
                picturepath = picturepath.Substring(picturepath.LastIndexOf('/') + 1);
                GetBlobInContainer(picturepath);               

            }
        }
        private/* async*/ void GetBlobInContainer(string fileName)
        {
            ConnectionString conn = new ConnectionString();
            BlobServiceClient storageAccount = new BlobServiceClient(conn.storageConnString);
            BlobContainerClient container = storageAccount.GetBlobContainerClient("images");
            container.CreateIfNotExists(PublicAccessType.Blob);

            BlockBlobClient blockBlob = container.GetBlockBlobClient(fileName);
            MemoryStream memstream = new MemoryStream();
            
            blockBlob.DownloadTo(memstream);
            memstream.Position = 0;
            m_picBitmap = BitmapFactory.DecodeStream(memstream);

            ////use web.config appSetting to get connection setting0
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conn.storageConnString);
            ////create the blob client
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            ////retrieve a refernce to a container
            //CloudBlobContainer blobContainer = blobClient.GetContainerReference("images");
            //await blobContainer.FetchAttributesAsync();
            //foreach (var item in blobContainer.Metadata)
            //{
            //    Console.WriteLine($"{item.Key} : {item.Value}");
            //}

            ////set permission to show to public
            //await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            //// Retrieve reference to a blob
            //CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(fileName);
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

        public Bitmap PictureBitmap
        { 
            get { return m_picBitmap; }
            //set { m_picBitmap = value;  }        
        }
        private int m_id;
        private int m_parentID;
        private string m_name;
        private string m_description;
        private float m_payout;
        private choreStatus m_status;
        private int m_completedID;
        private Bitmap m_picBitmap;
    }
}