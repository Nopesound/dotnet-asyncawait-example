using System.Diagnostics;

public class Program
{
    public static async Task Main(string[] args)
    {

        using var client = new HttpClient();
        //Warm up 
        var warm = client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1").Result;
        Stopwatch stopwatchAsync = new Stopwatch();
        stopwatchAsync.Start();
        await GetDataAsync(client);
        stopwatchAsync.Stop();
        var asyncTime = stopwatchAsync.ElapsedMilliseconds;

        Stopwatch stopwatchSync = new Stopwatch();
        stopwatchSync.Start();
        GetDataSync(client);
        stopwatchSync.Stop();
        var syncTime = stopwatchSync.ElapsedMilliseconds;
        Console.WriteLine($"Esecuzione conclusa async {asyncTime}, sync {syncTime}");



        stopwatchSync.Restart();
        GetDataSync(client);
        stopwatchSync.Stop();
        var syncTime2 = stopwatchSync.ElapsedMilliseconds;

        stopwatchAsync.Restart();
        await GetDataAsync(client);
        stopwatchAsync.Stop();
        var asyncTime2 = stopwatchAsync.ElapsedMilliseconds;

        Console.WriteLine($"Esecuzione conclusa async {asyncTime2}, sync {syncTime2}");
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