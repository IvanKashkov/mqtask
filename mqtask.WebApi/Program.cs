using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using mqtask.Application;
using mqtask.Persistence;

namespace mqtask.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    var snapshot = new DbSnapshotBuilder().Build(); // this method call is less than 50 ms
                    snapshot.Prepare(); // here we do some additional stuff in order to have optimized responses.
                    DbSnapshotHolder.Init(snapshot); // just store in memory our db snapshot
                });
    }
}
