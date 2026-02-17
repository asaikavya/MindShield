# MindShield

MindShield is a Blazor-based professional reputation guardian built as a 
- hackathon proof‑of‑concept. It inspects user-generated content and external signals to 
- detect social engineering or reputation risks, explains why content is risky,
- and recommends safer rewrites and mitigation actions.

- ## Core capabilities

- AI Sentiment Check: Powered by Azure OpenAI (via Microsoft Foundry) with an optional local fallback to Ollama.
- Real-time content analysis and classification using Azure OpenAI (configured via `AzureOpenAI` in `appsettings.json`).
- Policy-driven scoring and orchestration using Microsoft Foundry pipelines.
- Persistent storage for events, alerts, and telemetry using Entity Framework Core (`MindShieldDbContext`).
- Background workers that continuously scan feeds and raise alerts (for example, `Workers/LinkedInGuardianWorker.cs`).
- Interactive Blazor UI for inspection, feedback, and mitigation guidance.

Architecture

The application hosts a Blazor front-end and an application service layer in the same process.
Services call Microsoft Foundry pipelines that orchestrate Azure OpenAI model invocations and deterministic rules. 
Outcomes are stored in the SQL database and surfaced in the UI.

Mermaid diagram


```mermaid
flowchart LR
  User[User (Browser)] -->|Interacts| UI[`MindShield.Web` (Blazor UI)]
  UI -->|API / SignalR| Services[Application Services]
  Services --> Foundry[Microsoft Foundry Pipelines]
  Services --> OpenAI[Azure OpenAI (configured deployment)]
  Services --> DB[`MindShieldDbContext` (EF Core / SQL Server LocalDB)]
  Foundry --> OpenAI
  OpenAI --> Services
  subgraph BackgroundWorkers
    LinkedIn[`LinkedInGuardianWorker`]
    Other[`Other Workers`]
    LinkedIn --> Services
    Other --> Services
  end
  Services -->|Alerts & Guidance| UI
```
- 


Team
- Kavya Aakaveeti — .net developer : architecture, Microsoft Foundry integration, and Azure OpenAI orchestration.

Setup (Developer)

Prerequisites
- .NET 10 SDK
- SQL Server LocalDB (or any SQL Server instance)
- Azure OpenAI access (endpoint, key, and a deployment name)
- (Optional) Microsoft Foundry access for running and versioning pipelines

Local setup
1. Clone the repository and open a terminal at the solution root.
2. Configure Azure OpenAI
   - Edit `MindShield/MindShield.Web/appsettings.json` and set `AzureOpenAI:Endpoint`, `AzureOpenAI:ApiKey`, and `AzureOpenAI:DeploymentName`.
   - Prefer environment variables or `dotnet user-secrets` for secrets in development. Use Key Vault for production.
3. Verify database connection
   - Ensure `ConnectionStrings:DefaultConnection` points to a reachable SQL instance.
4. Apply EF Core migrations (if included)

```bash
cd MindShield/MindShield.Web
dotnet ef database update
```

5. Run the application

```bash
dotnet run --project MindShield/MindShield.Web
```

6. Open the app in a browser at the URL printed by the host (typically `https://localhost:5xxx`). Background workers run inside the host and will log scanning activity.

Notes
- The project is configured to use Azure OpenAI exclusively. Remove or disable local LLM integrations if present.
- Keep API keys out of source control. Use environment variables, user-secrets, or Key Vault.

License & Contribution
- This repository is a hackathon demo. 
