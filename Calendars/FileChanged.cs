using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Calendars
{
    public static class FileChanged
    {
        [FunctionName("FileChanged")]
        public static void Run(
            [BlobTrigger("calendars/{name}", Connection = "StorageConnectionAppSetting")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
