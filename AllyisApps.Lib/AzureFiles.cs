using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AllyisApps.Lib
{
    public static class AzureFiles
    {
        /// <summary>
        /// Gets a list of attachments used by the report.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public static List<string> GetReportAttachments(int reportId)
        {
            List<string> blobInfo = new List<string>();

            try
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudBlobClient client = new CloudBlobClient(account.BlobStorageUri.PrimaryUri);

                CloudBlobContainer blobContainer = client.GetContainerReference("ReportIdHere");

                foreach (CloudBlockBlob blob in blobContainer.ListBlobs().OfType<CloudBlockBlob>())
                {
                    blobInfo.Add(blob.Name);
                }
            }
            catch
            {
                blobInfo = new List<string>()
                {
                    "Test"
                };
            }

            return blobInfo;
        }

        /// <summary>
        /// Retrieve the selected attachment from the blob storage.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="attName">the attachment name.</param>
        /// <returns></returns>
        public static FileStream DownloadReportAttachment(int reportId, string attName)
        {

            CloudStorageAccount account = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient client = new CloudBlobClient(account.BlobStorageUri.PrimaryUri);

            CloudBlobContainer blobContainer = client.GetContainerReference(reportId.ToString());


            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(attName);

            return File.OpenWrite(blob.Uri.ToString());
        }

        /// <summary>
        /// Save an attachment to the blob storage.
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool SaveReportAttachments(int reportId, Stream stream, string fileName)
        {
            try
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudBlobClient client = new CloudBlobClient(account.BlobStorageUri.PrimaryUri);

                CloudBlobContainer blobContainer = client.GetContainerReference(reportId.ToString());

                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(fileName);

                using (var fileStream = stream)
                {
                    blockBlob.UploadFromStream(stream);
                }

                return blockBlob.Exists();
            }
            catch
            {
                return false;
            }
            
        }

        public static bool DeleteReportAttachment(int reportId, string fileName)
        {
            try
            {

                CloudStorageAccount account = CloudStorageAccount.Parse(
                  CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudBlobClient client = new CloudBlobClient(account.BlobStorageUri.PrimaryUri);

                CloudBlobContainer blobContainer = client.GetContainerReference(reportId.ToString());

                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(fileName);

                blockBlob.Delete();

                return !blockBlob.Exists();
            }
            catch
            {
                return false;
            }
        }

    }
}
