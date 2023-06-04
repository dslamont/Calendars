using Bins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
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
            
            
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);

            //Create the Black Bins Calendar
            string black_bins_blob_name = "black_bins.ics";
            CloudBlockBlob blackBinsBlob = blobContainer.GetBlockBlobReference(black_bins_blob_name);
            string calendarText = Bins.BinDays.CreateBlackBinDays();

            CloudBlockBlob calBlob = blobContainer.GetBlockBlobReference(black_bins_blob_name);
            calBlob.Properties.ContentType = "text/calendar";

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(calendarText)))
            {
                await calBlob.UploadFromStreamAsync(ms);
            }

            //Create the Recycling Bins Calendar
            string recycling_bins_blob_name = "recycling_bins.ics";
            CloudBlockBlob recyclingBinsBlob = blobContainer.GetBlockBlobReference(recycling_bins_blob_name);
            recyclingBinsBlob.Properties.ContentType = "text/calendar";
            calendarText = BinDays.CreateRecyclingBinDays();
            
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(calendarText)))
            {
                await recyclingBinsBlob.UploadFromStreamAsync(ms);
            }

            return new OkObjectResult("Created calendars");
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
