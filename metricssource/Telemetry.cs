using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace metricssource
{
    class Telemetry
    {
        public const string ServiceName = "metricssource";
        public const string Version = "1.0.0";
        public static MeterProvider AddMeterProvider()
        {
            MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
                    .AddMeter(ServiceName)
                     .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: ServiceName, serviceVersion: Version))
                    .AddConsoleExporter()
                    .AddPrometheusExporter(opt =>
                    {
                        opt.StartHttpListener = true;
                        opt.HttpListenerPrefixes = new string[] { $"http://localhost:9184/" };
                    })
                    .Build();
                    return meterProvider;
        }
    }
}