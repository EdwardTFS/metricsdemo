using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace metricssource
{
    class MetricsSourceService : IHostedService, IDisposable
    {
        private readonly IHostApplicationLifetime applicationLifetime;
        readonly CancellationTokenSource stoppingCts = new();
        private Task? executingTask;

        public MetricsSourceService(IHostApplicationLifetime applicationLifetime)
        {
            this.applicationLifetime = applicationLifetime;
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
            Console.WriteLine("Stop requested: {0}",executingTask.Status);
            try
            {
                // Signal cancellation to the executing method
                stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
                Console.WriteLine("Stop wait completed: {0}",executingTask.Status);
            }
        }

        async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.WriteLine("Start");
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.Write("*");
                    await Task.Delay(1000, stoppingToken).ContinueWith(t => {},CancellationToken.None); // łyka exception
                }

                applicationLifetime.StopApplication();
                Console.WriteLine("Stop");

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

        }
    }
}