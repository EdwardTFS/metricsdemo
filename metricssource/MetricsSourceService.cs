using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace metricssource{
    class MetricsSourceService : IHostedService, IDisposable
    {
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly IConfiguration configuration;
        private readonly CancellationTokenSource stoppingCts = new();
        private Task? executingTask;

        public MetricsSourceService(IHostApplicationLifetime applicationLifetime, IConfiguration configuration)
        {
            this.applicationLifetime = applicationLifetime;
            this.configuration = configuration;
        }

        public void Dispose()
        {
            stoppingCts.Cancel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            executingTask = ExecuteAsync(stoppingCts.Token);
            

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (executingTask.IsCompleted)
            {
                return executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(executingTask, Task.Delay(Timeout.Infinite,
                                                              cancellationToken));
            }
        }

        async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Start");
            await Task.Delay(1000,stoppingToken);
            Console.WriteLine("End");
            applicationLifetime.StopApplication();

        }
    }
}