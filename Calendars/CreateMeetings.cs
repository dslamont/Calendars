using BookGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calendars
{
    public class CreateMeetings
    {

        [FunctionName("CreateMeetings")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CreateMeetings/")] HttpRequest req, 
            ILogger log)
        {
            log.LogInformation($"{nameof(CreateMeetings)} trigger function processed a request."); 
            //Change

            string account_name = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process); 
            string account_key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);
            string container_name = Environment.GetEnvironmentVariable("ContainerName", EnvironmentVariableTarget.Process);
            string blob_name = "book_group.json";

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);
            CloudBlockBlob myblob = blobContainer.GetBlockBlobReference(blob_name);

            Meeting meeting = new Meeting();
            meeting.StateDate = new DateTime(2021, 01, 11, 19, 30, 00);
            meeting.Title = "Eleanor Oliphant Is Completely Fine";
            meeting.Author = "Gail Honeyman";
            meeting.BookUrl = "https://www.goodreads.com/book/show/34200289-eleanor-oliphant-is-completely-fine";
            meeting.CoverImageUrl = "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1540909460l/34200289._SY475_.jpg";

            meeting.OrganizerName = "Don Lamont";
            meeting.OrganizerEmail = "don.lamont@e-pict.net";

            meeting.Location = "Ferryhill Community Centre, Albury Rd, Aberdeen AB11 6TN, UK";
            meeting.UpdateCount = 1;

            Schedule schedule = new Schedule();
            schedule.Meetings = new List<Meeting> { meeting };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(schedule, options);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {

                await myblob.UploadFromStreamAsync(ms);
            }

            return new OkObjectResult($"Created {blob_name}");
        }

        private static string GetBlobSasUri(CloudBlobContainer container, string blobName, string policyName = null)
        {
            string sasBlobToken;
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
                };

                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

                Console.WriteLine("SAS for blob (ad hoc): {0}", sasBlobToken);
                Console.WriteLine();
            }
            else
            {
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

                Console.WriteLine("SAS for blob (stored access policy): {0}", sasBlobToken);
                Console.WriteLine();
            }

            // Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }
    }
}
