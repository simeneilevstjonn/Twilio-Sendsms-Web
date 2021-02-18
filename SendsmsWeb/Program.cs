using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SendsmsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Config = new ProgramConfig();
            TwilioSender = new TwilioSender(Config.AuthToken, Config.AccountSID, Config.ServiceSID);
            CreateHostBuilder(args).Build().Run();
        }

        public static ProgramConfig Config { get; private set; }
        public static TwilioSender TwilioSender { get; private set; }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
