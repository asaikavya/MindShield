using MindShield.Core;
using MindShield.Web;
using Microsoft.EntityFrameworkCore;

namespace MindShield.Web.Workers
{
    public class LinkedInGuardianWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LinkedInGuardianWorker> _logger;
        private int _scanCount = 0; // We use this to trigger the "Fake Event"

        public LinkedInGuardianWorker(IServiceProvider serviceProvider, ILogger<LinkedInGuardianWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🛡️ MindShield Guardian Service: ONLINE");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    int scanCount = 0;
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        scanCount++;
                        try
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var context = scope.ServiceProvider.GetRequiredService<MindShieldDbContext>();
                                var users = await context.RealityProfiles.Where(u => u.IsGuardianActive).ToListAsync(stoppingToken);

                                foreach (var user in users)
                                {
                                    _logger.LogInformation($"[SCANNING] Checking LinkedIn feed for: {user.FullName}...");

                                    // --- THE DRAMATIC MOMENT FOR THE VIDEO ---
                                    // On the 3rd scan (after ~30 seconds), we PRETEND to find a bad post.
                                    if (scanCount % 3 == 0)
                                    {
                                        // Step 1: Detect gently
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        _logger.LogWarning($"[MINDSHIELD] 🛡️ Spotted a high-energy post on LinkedIn...");

                                        await Task.Delay(1000);

                                        // Step 2: Protect without deleting (Hide/Draft)
                                        _logger.LogInformation($"[ACTION] ⏸️ Moving post to 'Private Drafts' for safety.");

                                        await Task.Delay(1000);

                                        // Step 3: Bring in support
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        _logger.LogInformation($"[SUPPORT] 🤝 Pinging Trusted Guardian (Sarah) to check in on Alex.");
                                        Console.ResetColor();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Guardian error.");
                        }

                        await Task.Delay(10000, stoppingToken); // Scan every 10 seconds
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Guardian error.");
                }

                await Task.Delay(10000, stoppingToken); // Scan every 10 seconds
            }
        }
    }
}