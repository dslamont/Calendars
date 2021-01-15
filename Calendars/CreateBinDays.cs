using BookGroup;
using Calendar;
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
    public class CreateBinDays
    {

        [FunctionName("CreateBinDays")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CreateBinDays/")] HttpRequest req, 
            ILogger log)
        {
            log.LogInformation($"{nameof(CreateBinDays)} trigger function processed a request."); 
            //Change

            string account_name = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process); 
            string account_key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);
            string container_name = Environment.GetEnvironmentVariable("ContainerName", EnvironmentVariableTarget.Process);
            string blob_name = "bins.ics";

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);
            CloudBlockBlob myblob = blobContainer.GetBlockBlobReference(blob_name);

            VCalendar calendar = new VCalendar();
            calendar.TimeZone = new VTimeZone();
            calendar.Events = CreateEvents();
            string calendarText = calendar.CreateCalendarText();

            CloudBlockBlob calBlob = blobContainer.GetBlockBlobReference(blob_name);
            calBlob.Properties.ContentType = "text/calendar";

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(calendarText)))
            {
                await calBlob.UploadFromStreamAsync(ms);
            }

            return new OkObjectResult($"Created {blob_name}");
        }

        protected List<VEvent> CreateEvents()
        {
            List<VEvent> events = new List<VEvent>();

            DateTime currentDate = new DateTime(2021, 01, 13, 0, 0, 0);
            DateTime endDate = new DateTime(2021, 08, 30, 0, 0, 0);
            int loopIndex = 0;
            while(currentDate<endDate)
            {
                bool isBlackBins = (loopIndex % 2) == 0;
                VEvent vEvent = CreateEvent(currentDate, isBlackBins);
                events.Add(vEvent);

                currentDate = currentDate.AddDays(7);
                loopIndex++;
            }

            return events;
        }

        protected VEvent CreateEvent(DateTime date, bool blackBins)
        {
            VEvent vEvent = null;

            vEvent = new VEvent();

            vEvent.Uid = CreateUID(date);
            vEvent.DateTimeStamp = $"DTSTAMP:{date.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")}";
            vEvent.Organiser = "ORGANIZER;CN=Don Lamont:MAILTO:don.lamont@e-pict.net";

            vEvent.StartTime = $"DTSTART;TZID=Europe/London:{date.ToUniversalTime().ToString("yyyyMMddTHHmmss")}";
            vEvent.EndTime = $"DTEND;TZID=Europe/London:{date.AddDays(1).ToUniversalTime().ToString("yyyyMMddTHHmmss")}";

            if (blackBins)
            {
                vEvent.Summary = "SUMMARY:Black Bins";
                vEvent.Description = "DESCRIPTION:Black Bins";
            }
            else
            {
                vEvent.Summary = "SUMMARY:Recycling Bins";
                vEvent.Description = "DESCRIPTION:Recycling Bins";
            }
            vEvent.Status = "STATUS:CONFIRMED";
            vEvent.Sequence = "SEQUENCE:1";
            vEvent.Transparency = "TRANSP:TRANSPARENT";
            vEvent.Categories = "CATEGORIES:Refuse,Recycling";
            vEvent.Class = "CLASS:PUBLIC";

            return vEvent;
        }

        protected string CreateUID(DateTime dateTime)
        {
            string uid = String.Empty;

            if (dateTime != null)
            {
                uid = $"UID:bins_{dateTime.Year}{dateTime.Month}{dateTime.Day}_@e-pict.net";
            }

            return uid;
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
