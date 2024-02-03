#region AzureFileShare SDK with Default Azure Creds Try
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// Example of Azure file share client SDK try with default Azure creds
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

using Azure.Identity;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares;
using Azure.Storage;
using Azure.ResourceManager;
using Azure.ResourceManager.Storage;
using Azure.Core;

// -------------This is using connection string we know it works--------------
//string connectionString = "DefaultEndpointsProtocol=https;AccountName=chazfsdeveuw001fsst;AccountKey=storagekey;EndpointSuffix=core.windows.net";
//string shareName = "aksfileshare";
//ShareClient share = new(connectionString, shareName);
// ---------------------------------------------------------------------------


//====================================================================
// Below is the only way to use default Azure creds to work with file share
// SMB Files cannot authenticate with a TokenCredential refer
// https://github.com/Azure/azure-sdk-for-net/issues/17000
//--------------------------------------------------------------------
ArmClient client = new ArmClient(new DefaultAzureCredential());

string resourceId = "/subscriptions/subscriptionid/resourceGroups/ch-azfs-dev-euw-001-rg/providers/Microsoft.Storage/storageAccounts/chazfsdeveuw001fsst";
StorageAccountResource storageAccount = client.GetStorageAccountResource(new ResourceIdentifier(resourceId));

string storageKey = storageAccount.GetKeys().FirstOrDefault().Value;

string fileShareUri = "https://chazfsdeveuw001fsst.file.core.windows.net/aksfileshare";
ShareClient share = new(new Uri(fileShareUri),
    new StorageSharedKeyCredential("chazfsdeveuw001fsst", storageKey));
//====================================================================


// Track the remaining directories to walk, starting from the root
var remaining = new Queue<ShareDirectoryClient>();
remaining.Enqueue(share.GetRootDirectoryClient());
while (remaining.Count > 0)
{
    // Get all of the next directory's files and subdirectories
    ShareDirectoryClient dir = remaining.Dequeue();
    foreach (ShareFileItem item in dir.GetFilesAndDirectories())
    {
        // Print the name of the item
        Console.WriteLine(item.Name);

        // Keep walking down directories
        if (item.IsDirectory)
        {
            remaining.Enqueue(dir.GetSubdirectoryClient(item.Name));
        }
    }
}
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endregion

// Use mounted volume path for container
string? mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH");
// string? mediaPath = "C:/temp/videos";
if (mediaPath is not null)
{
    DateTime today = DateTime.Now.Date.AddDays(-1);
    while (true)
    {
        string dateNow = DateTime.Now.ToString("ddMMMyyyy_HHmmss");
        if (today != DateTime.Now.Date)
        {
            today = DateTime.Now.Date;

            string fileName = $"{mediaPath}/{dateNow}.txt";
            Console.WriteLine(fileName);
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
                sw.WriteLine(fileName);
            }
        }

        Console.WriteLine(dateNow);
        Thread.Sleep(300000);
    }
}