using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MindShield.Web.Workers // Notice the namespace is Workers!
{
    public class LinkedInGuardianWorker : BackgroundService
    {
        public LinkedInGuardianWorker()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Your logic to monitor LinkedIn goes here
                Console.WriteLine("LinkedIn Guardian Worker running...");

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}