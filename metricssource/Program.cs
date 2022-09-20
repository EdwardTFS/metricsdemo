using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using System;
using System.Threading.Tasks;

namespace metricssource
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var x = Telemetry.AddMeterProvider();
            await CreateHostBuilder(args).RunConsoleAsync();
            x.Shutdown();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MetricsSourceService>();
                    services.AddSingleton<Metrics>();
                });
        }
    }
}