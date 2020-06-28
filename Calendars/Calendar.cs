using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Net;

namespace Calendars
{
    public class Calendar
    {

        [FunctionName("Calendar")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Calendar/{name}.ics")] HttpRequest req, string name,
            ILogger log)
        {
            log.LogInformation($"{nameof(Calendar)} trigger function processed a request.");

            string account_name = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process); 
            string account_key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);
            string container_name = Environment.GetEnvironmentVariable("ContainerName", EnvironmentVariableTarget.Process);
            string blob_name = $"{name}.ics";

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);
            CloudBlockBlob myblob = blobContainer.GetBlockBlobReference(blob_name);

            var ms = new MemoryStream();

            await myblob.DownloadToStreamAsync(ms);

            return new FileContentResult(ms.ToArray(), myblob.Properties.ContentType);
        }
    }
}
