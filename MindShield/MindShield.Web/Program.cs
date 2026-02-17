using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using MindShield.Core;
using MindShield.Web;
using MindShield.Web.Components;
using MindShield.Web.Services;
using MindShield.Web.Workers;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Basic Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Add Database
builder.Services.AddDbContext<MindShieldDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Add Semantic Kernel (THE AI BRAIN) 🧠
// We create the builder first
var kernelBuilder = builder.Services.AddKernel();

// Check if we are in Development (Local) or Production (Azure)
var azureEndpoint = builder.Configuration["AzureOpenAI:Endpoint"];
var azureKey = builder.Configuration["AzureOpenAI:ApiKey"];
var deployment = builder.Configuration["AzureOpenAI:DeploymentName"];

// 🚀 PRIMARY: Try to use Azure OpenAI (Foundry)
if (!string.IsNullOrEmpty(azureEndpoint) && !string.IsNullOrEmpty(azureKey))
{
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: deployment ?? "gpt-4o-mini",
        endpoint: azureEndpoint,
        apiKey: azureKey
    );
}
// 🏠 FALLBACK: Use Ollama only if Azure keys are missing
else
{
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: "phi3",
        apiKey: "ignore",
        httpClient: new HttpClient { BaseAddress = new Uri("http://localhost:11434/v1") }
    );
}

// 4. Add Background Workers
builder.Services.AddHostedService<LinkedInGuardianWorker>();
builder.Services.AddScoped<ISafetyService, MindShieldSafetyService>();

// 5. BUILD THE APP (Crucial: Do this AFTER adding services)
var app = builder.Build();

// --- HTTP Pipeline Setup ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();