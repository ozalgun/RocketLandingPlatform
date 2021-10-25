using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RocketLandingPlatform;


namespace RocketLanding
{
    public static class Program
    {

        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var landingChecker = provider.GetRequiredService<LandingChecker>();
            var message = landingChecker.CheckLandingPosition(5, 5);
            Console.WriteLine(message);
            message = landingChecker.CheckLandingPosition(2, 3);
            Console.WriteLine(message);
            message = landingChecker.CheckLandingPosition(10, 10);
            Console.WriteLine(message);
            message = landingChecker.CheckLandingPosition(10, 12);
            Console.WriteLine(message);
            Console.ReadLine();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .AddSingleton(_ => new LandingChecker(10, 10,10,10)));



    }

}
