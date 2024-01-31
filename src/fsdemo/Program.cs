// Use mounted volume path for container
string? mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH");
// string? mediaPath = "C:/temp/videos";
if (mediaPath is not null)
{
    DateTime today = DateTime.Now.Date.AddDays(-1);
    while(true)
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