using System.Diagnostics.Metrics;
using System.Reflection;

namespace metricssource
{
    class Metrics
    {
        public Metrics()
        {
            this.meter  = new Meter(Telemetry.ServiceName, Telemetry.Version);
            requestsProcessedCounter = meter.CreateCounter<int>("requests-processed");
            unusedCounter = meter.CreateCounter<int>("unused");
            unusedCounter.Add(0); // to powoduje że counter  pokazuje się w wynikach od razu z wartością 0
            requestTimeHistogram = meter.CreateHistogram<long>("request-duration", unit: "ms");
        }

        private readonly Meter meter;
        private readonly Counter<int> requestsProcessedCounter;
        private readonly Counter<int> unusedCounter;
        private readonly Histogram<long> requestTimeHistogram;

        public void RequestProcessed(long durationMS)
        {
            requestsProcessedCounter.Add(1);
            requestTimeHistogram.Record(durationMS);
        }

        public void UseUnused()
        {
            unusedCounter.Add(1);
        }
    }
    
}