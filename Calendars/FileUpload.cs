using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using BookGroup;
using System.Text;
using System.Text.Json;

namespace Calendars
{
    public static class FileUpload
    {
        [FunctionName("FileUpload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string account_name = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process);
            string account_key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);
            string container_name = Environment.GetEnvironmentVariable("ContainerName", EnvironmentVariableTarget.Process);
            string blob_name = name;

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);
            CloudBlockBlob myblob = blobContainer.GetBlockBlobReference(blob_name);

            await myblob.UploadFromStreamAsync(req.Body);

            string blobSasUrl = GetBlobSasUri(blobContainer, blob_name, null);
            Console.WriteLine(blobSasUrl);

            switch (name)
            {
                case "book_group.json":

                    req.Body.Seek(0, SeekOrigin.Begin);
                    Schedule schedule = await JsonSerializer.DeserializeAsync<Schedule>(req.Body);
                    BookGroupConvertor convertor = new BookGroupConvertor();

                    //Create the default Book Group Calendar

                    string cal_blob_name = "book_group.ics";
                    CloudBlockBlob calBlob = blobContainer.GetBlockBlobReference(cal_blob_name);
                    calBlob.Properties.ContentType = "text/calendar";

                    string calText = convertor.CreateCalendar(schedule, BookGroupCalTypeEnum.DEFAULT);

                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(calText)))
                    {
                        await calBlob.UploadFromStreamAsync(ms);
                    }


                    //Create the FerryHill Book Group Calendar

                    cal_blob_name = "book_group_web.ics";
                    calBlob = blobContainer.GetBlockBlobReference(cal_blob_name);
                    calBlob.Properties.ContentType = "text/calendar";

                    calText = convertor.CreateCalendar(schedule, BookGroupCalTypeEnum.WEBSITE);

                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(calText)))
                    {
                        await calBlob.UploadFromStreamAsync(ms);
                    }

                    break;
            }

            return name != null
                ? (ActionResult)new OkObjectResult($"Uploaded {name}")
                : new BadRequestObjectResult("Please pass a file name on the query string and upload in the request body");
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
