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
using System.Text;
using System.Threading.Tasks;

using OpenUniversity;
using System.Text.Json;
using System.IO;

namespace Calendars
{
    public class CreateOUEvents
    {


        [FunctionName("CreateOUEvents")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CreateOUEvents/")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(CreateMeetings)} trigger function processed a request.");
            //Change

            string account_name = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process);
            string account_key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);
            string container_name = Environment.GetEnvironmentVariable("ContainerName", EnvironmentVariableTarget.Process);
            string blob_name = "MST224.json";

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);
            CloudBlockBlob myblob = blobContainer.GetBlockBlobReference(blob_name);

            OUFeed feed = new OUFeed();

            OUModule moduleDetails = new OUModule();
            moduleDetails.Code = "MST224";
            moduleDetails.Title = "Mathematical methods";
            feed.Module = moduleDetails;

            List<OUEvent> events = new List<OUEvent>();

            OUEvent event1 = new OUEvent();
            event1.Id = "20211002";
            event1.StateDate = new DateTime(2021, 10, 02, 00, 00, 00);
            event1.Title = "Book 1 Unit 1: Getting started";
            event1.OrganizerName = "Don Lamont";
            event1.OrganizerEmail = "don.lamont@e-pict.net";
            event1.UpdateCount = 1;
            events.Add(event1);

            OUEvent event2 = new OUEvent();
            event2.Id = "20211016";
            event2.StateDate = new DateTime(2021, 10, 16, 00, 00, 00);
            event2.Title = "Book 1 Unit 2: First-order differential equations";
            event2.OrganizerName = "Don Lamont";
            event2.OrganizerEmail = "don.lamont@e-pict.net";
            event2.UpdateCount = 1;
            events.Add(event2);

            feed.Events = events;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(feed, options);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {

                await myblob.UploadFromStreamAsync(ms);
            }

            return new OkObjectResult($"Created {blob_name}");
        }

    }
}
