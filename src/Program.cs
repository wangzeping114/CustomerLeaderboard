using CustomerLeaderboard.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        builder.Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}