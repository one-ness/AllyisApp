using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

				CloudBlobClient client = account.CreateCloudBlobClient();

				CloudBlobContainer blobContainer = client.GetContainerReference(reportId.ToString());

				foreach (CloudBlockBlob blob in blobContainer.ListBlobs().OfType<CloudBlockBlob>())
				{
					blobInfo.Add(blob.Name);
				}
			}
			catch
			{
				blobInfo = new List<string>();
			}

			return blobInfo;
		}

		/// <summary>
		/// Retrieve the selected attachment from the blob storage.
		/// </summary>
		/// <param name="reportId">The report id.</param>
		/// <param name="attName">the attachment name.</param>
		/// <param name="fileStream"></param>
		/// <returns></returns>
		public static string DownloadReportAttachment(int reportId, string attName, Stream fileStream)
		{
			CloudStorageAccount account = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting("StorageConnectionString"));

			CloudBlobClient client = account.CreateCloudBlobClient();

			CloudBlobContainer blobContainer = client.GetContainerReference(reportId.ToString());

			CloudBlockBlob blob = blobContainer.GetBlockBlobReference(attName);

			blob.DownloadToStream(fileStream);

			return blob.Properties.ContentType;
		}

		public static Tuple<Stream, string, string> GetFile(int reportId, string attName)
		{
			Stream stream = new MemoryStream();
			string contentType = DownloadReportAttachment(reportId, attName, stream);
			return new Tuple<Stream, string, string>(stream, contentType, attName);
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

				CloudBlobClient client = account.CreateCloudBlobClient();

				CloudBlobContainer blobContainer = client.GetContainerReference(reportId.ToString());

				blobContainer.CreateIfNotExists();

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

				CloudBlobClient client = account.CreateCloudBlobClient();

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