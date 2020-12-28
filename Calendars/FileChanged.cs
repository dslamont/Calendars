using System;
using System.IO;
using System.Text;
using System.Text.Json;
using BookGroup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Calendars
{
    public static class FileChanged
    {
        [FunctionName("FileChanged")]
        public async static void Run(
            [BlobTrigger("calendars/{name}", Connection = "StorageConnectionAppSetting")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");


            string account_name = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process);
            string account_key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);
            string container_name = Environment.GetEnvironmentVariable("ContainerName", EnvironmentVariableTarget.Process);

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(account_name, account_key), true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(container_name);

            switch (name)
            {
                case "book_group.json":

                    string blob_name = "book_group.ics";
                    CloudBlockBlob calBlob = blobContainer.GetBlockBlobReference(blob_name);

                    Schedule schedule = await JsonSerializer.DeserializeAsync<Schedule>(myBlob);
                    Convertor convertor = new Convertor();
                    string calText = convertor.CreateCalendar(schedule);


                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(calText)))
                    {

                        await calBlob.UploadFromStreamAsync(ms);
                    }
                    break;
            }
        }
    }
}
