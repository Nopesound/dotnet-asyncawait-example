using System.Diagnostics;

public class Program
{
    public static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        try
        {
            using var client = new HttpClient();
            //Warm up
            var warm = client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1", cts.Token).Result;

            var syncCount = 0;
            long syncTime = 0;
            while (syncCount <= 50)
            {
                Stopwatch stopwatchSync = new Stopwatch();
                stopwatchSync.Start();
                GetDataSync(client);
                stopwatchSync.Stop();
                syncTime = +stopwatchSync.ElapsedMilliseconds;
                syncCount++;
            }

            Stopwatch stopwatchAsync = new Stopwatch();
            var asyncCount = 0;
            long asyncTime = 0;
            while (asyncCount <= 50)
            {
                stopwatchAsync.Start();
                await GetDataAsync(client);
                stopwatchAsync.Stop();
                asyncTime = +stopwatchAsync.ElapsedMilliseconds;
                asyncCount++;
            }

            Console.WriteLine($"Esecuzione conclusa tempi medi: async {asyncTime / 50}, sync {syncTime / 50}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in esecuzione {ex.Message}");
            cts.Cancel();
            throw;
        }
    }

    // Versione sincrona
    private static string GetDataSync(HttpClient client)
    {
        return client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1").Result;
    }

    // Versione asincrona
    private static async Task<string> GetDataAsync(HttpClient client)
    {
        return await client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1");
    }
}