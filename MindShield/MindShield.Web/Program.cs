using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using MindShield.Core;
using MindShield.Web;
using MindShield.Web.Components;
using MindShield.Web.Services;
using MindShield.Web.Workers;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. BASIC SERVICES
// =========================================================================
builder.Services.AddLogging();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options => options.DetailedErrors = true);

// =========================================================================
// 2. RATE LIMITING — Protects Azure OpenAI tokens from being drained
// =========================================================================
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("scan-limit", opt =>
    {
        opt.PermitLimit = 15;                          // Max 15 scans
        opt.Window = TimeSpan.FromHours(1);            // Per hour per IP
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // Return 429 with a friendly message when limit hit
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.", cancellationToken);
    };
});

// =========================================================================
// 3. MULTI-AGENT SERVICES
// =========================================================================
builder.Services.AddScoped<IClassifierAgent, ClassifierAgent>();
builder.Services.AddScoped<ICoachingAgent, CoachingAgent>();
builder.Services.AddScoped<IGovernanceAgent, GovernanceAgent>();
builder.Services.AddScoped<ISafetyService, MindShieldSafetyService>();

// =========================================================================
// 4. DATABASE — Azure SQL in Production, LocalDB in Development
// =========================================================================
builder.Services.AddDbContext<MindShieldDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // FIX: Retry logic for Azure SQL transient failures
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null
            );
        }
    );
});

builder.Services.AddScoped<IGuardianNotificationService, EmailGuardianService>();

// =========================================================================
// 5. BACKGROUND WORKERS
// =========================================================================
builder.Services.AddHostedService<LinkedInGuardianWorker>();

// =========================================================================
// 6. SEMANTIC KERNEL — Azure AI Foundry Primary, Ollama Fallback
// =========================================================================
var kernelBuilder = builder.Services.AddKernel();

var azureEndpoint = builder.Configuration["AzureOpenAI:Endpoint"];
var azureKey = builder.Configuration["AzureOpenAI:ApiKey"];
var deployment = builder.Configuration["AzureOpenAI:DeploymentName"];

// PRIMARY: Azure OpenAI via Microsoft Foundry
if (!string.IsNullOrEmpty(azureEndpoint) && !string.IsNullOrEmpty(azureKey))
{
    Console.WriteLine("[Startup] Using Azure AI Foundry (GPT-4o)");
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: deployment ?? "gpt-4o-mini",
        endpoint: azureEndpoint,
        apiKey: azureKey
    );
}
// FALLBACK: Ollama/Phi-3 for offline/local development
else
{
    Console.WriteLine("[Startup] Azure keys not found — falling back to Ollama (Phi-3)");
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: "phi3",
        apiKey: "ignore",
        httpClient: new HttpClient { BaseAddress = new Uri("http://localhost:11434/v1") }
    );
}

// =========================================================================
// 7. BUILD APP
// =========================================================================
var app = builder.Build();

// =========================================================================
// 8. HTTP PIPELINE
// =========================================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// FIX: Rate limiting must be added before routing
app.UseRateLimiter();

app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// =========================================================================
// 9. AUTO-MIGRATE DATABASE ON STARTUP
//    Runs EF migrations automatically — no manual step needed on Azure
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<MindShieldDbContext>();
        db.Database.Migrate();
        Console.WriteLine("[Startup] Database migration applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup] Database migration failed: {ex.Message}");
    }
}

app.Run();