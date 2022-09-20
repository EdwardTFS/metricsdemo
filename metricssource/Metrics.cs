using System.Diagnostics.Metrics;
using System.Reflection;

namespace metricssource
{
    class Metrics
    {
        public Metrics()
        {
            var assemblyname = Assembly.GetExecutingAssembly().GetName();
            this.meter  = new Meter(assemblyname.Name ?? string.Empty, assemblyname.Version?.ToString());
            requestsProcessedCounter = meter.CreateCounter<int>("requests-processed");
            requestTimeHistogram = meter.CreateHistogram<long>("request-duration", unit: "ms");
        }

        private readonly Meter meter;
        private readonly Counter<int> requestsProcessedCounter;
        private readonly Histogram<long> requestTimeHistogram;

        public void RequestProcessed(long durationMS)
        {
            requestsProcessedCounter.Add(1);
            requestTimeHistogram.Record(durationMS);
        }
    }
    
}