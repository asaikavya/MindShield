# MindShield: Professional Reputation Guardian 🛡️

> **MindShield scans your social media drafts with a multi-agent AI pipeline before you post — catching career-ending content in real time.**

**MindShield** is an AI-driven safety net for social media, built for the **Microsoft AI Dev Days Hackathon**. Powered by the **Microsoft Agent Framework**, **Microsoft Foundry**, and developed using **GitHub Copilot**.

---
## 💡 The Idea

Every developer knows this feeling — you're about to commit code and you run a quick diff first. Just a pause. A preview. A chance to make sure what you're pushing actually reflects your best work.

We do the same thing on social media every single day — except there's no diff. No preview. No pause button.

**MindShield is that pause button.**

Just as `git diff` shows you what you're about to push to a repository, MindShield shows you what you're about to push to the internet — before it's permanent.

| Git World | MindShield World |
| :--- | :--- |
| `git diff` before commit | AI safety scan before posting |
| Push to remote | Publish to social media |
| CI/CD pipeline checks | Multi-agent safety pipeline |
| Can't easily undo a bad push | Can't unsee a viral screenshot |

---

## 🎯 The Problem

We live in an era of digital permanence. A thoughtless tweet. An angry comment. A cringe-worthy post. These moments — seconds to create, instantly public — can shape careers, damage relationships, and define reputations for years.

While users can delete a post after the fact, the internet is permanent. A damaging post live for even a few seconds can be screenshotted or archived by bots, making reactive deletion no longer a viable safety net. Social media platforms encourage instant sharing but rarely provide tools that help users pause and reconsider before publishing potentially harmful content.

MindShield solves this. It's the **pause button you wish you always had.**

**⏱️ Why Now?**

Three converging trends make MindShield especially relevant today:

+ **Digital Permanence Despite Deletion**: Content can be screenshotted, archived, or reshared within seconds. In many cases, the reputational damage happens before the original post is removed, making prevention more valuable than cleanup.
+ **The Agentic AI Breakthrough**: Traditional moderation tools relied on simple keyword filters that often missed context. Today, using the **Microsoft Agent Framework**, we can orchestrate specialized AI agents that understand nuance, tone, sarcasm, and platform-specific expectations.
+ **Corporate Compliance Pressure**: Organizations face increasing reputational and regulatory risks tied to employee social media activity. Companies need proactive, immutable audit trails to prevent brand-damaging posts before they happen.

---

## 🔒 Privacy & Trust

**Is this corporate spyware?**
No. MindShield is an **active seatbelt, not a security camera.** It coaches the employee and prevents fireable offenses before they happen — protecting the user's career just as much as the company's brand. There is no silent monitoring, no passive logging, and **no action without the user's knowledge.** Every intervention is visible, every decision is explainable, and the user retains full override control at all times.

**MindShield does not suppress free speech.** It only intervenes when a post carries genuine legal, personal, or career consequences the user themselves would regret — an SEC violation, a named personal attack, or a mental health crisis. General opinions, frustrations, and criticism pass through untouched. The goal is to let 95% of posts through and catch the 5% that could genuinely ruin someone's life.

**Where does the data go?**

Three privacy principles enforced by design, not just policy:

+ **Draft content is never stored.** What you type stays between you and the AI — it is never written to the database. Ever.
+ **Guardian email alerts are entirely user-configured.** MindShield has no default surveillance. You choose your guardian, and you can cancel any alert within the 5-second override window.
+ **Offline privacy mode is fully supported.** MindShield's Ollama/Phi-3 fallback runs semantic analysis entirely on local hardware, making it viable for air-gapped enterprise environments and privacy-first deployments.

**What IS persisted (audit trail only):**
+ Platform metadata & risk classification
+ AI confidence score & intervention action taken
+ Guardian notification status (true/false)

**What is NEVER persisted:**
+ Draft post content
+ Rewrite suggestions
+ Any raw text the user typed

---

## ✨ What Makes MindShield Different: A Multi-Agent Architecture

MindShield doesn't just pass text to a single LLM. It orchestrates three highly specialized agents to evaluate intent, preserve authenticity, and trigger interventions:

### 🤖 Agentic Tiered Analysis

+ **The Classifier Agent (Context & Routing)**: Evaluates risk differently for LinkedIn, X/Twitter, Instagram, and TikTok. It understands that a "hot take" acceptable on X might be career suicide on LinkedIn. Crucially, it distinguishes general enthusiasm from genuine legal violations — a keyword filter cannot do this.

+ **The Coaching Agent (Anti-Robot Rewrites)**: Engages during "Moderate" risks. It suggests professional rewrites that preserve the user's authentic voice and frustration, stripping out the fireable offenses without sounding like a corporate robot.

+ **The Governance Agent (Crisis & Compliance)**: Engages during "Severe" risks (e.g., Insider Trading or mental health crisis). It completely blocks the post and triggers a secure email alert to a user-designated Guardian.

---

## 🚀 Key Features

* **Real-Time Agent Trace Panel**: MindShield is a "glass box," not a black box. The UI features a live terminal streaming the multi-agent handoffs in real-time, proving exactly how the AI made its decision.
* **⏳ 5-Second "Break Glass" Intervention**: When Severe risk is detected, the UI locks down and initiates a 5-second countdown before notifying compliance or family — giving the user a final chance to cancel and regain control. **No action ever happens without the user's knowledge.**
* **🧠 Hybrid AI Architecture**: Powered by Azure AI Foundry (GPT-4o) for high-fidelity analysis, with Ollama (Phi-3) fallback support for privacy-first, fully offline, air-gapped enterprise usage.
* **🗄️ Compliance Audit Trail**: Every scan outcome is persisted to Azure SQL — platform, risk level, confidence score, and action taken — without ever storing the raw post content. A provable compliance record, not just a claim.
* **💻 Glassmorphism Dashboard**: A high-performance Blazor Interactive Server UI featuring real-time scanning states and a modern, adaptive design.

---

## 📊 Real-World Impact

🟢 **Scenario 1: The "Hype" False Positive**

+ **User Types**: "My company's stock is about to go through the roof 🚀"
+ **Agent Decision**: **SAFE**. The Classifier Agent recognizes this as standard marketing enthusiasm, not an SEC violation — proving semantic intelligence over basic keyword matching. A keyword filter would block this. MindShield does not.

🟡 **Scenario 2: Platform-Aware Intelligence**

+ **User Types**: "My boss is so annoying, I can't stand this job" (Target: LinkedIn)
+ **Agent Decision**: **MODERATE**.
+ **Action**: The Coaching Agent suggests a rewrite: *"This job is so frustrating sometimes, and it feels like I'm constantly hitting roadblocks."* The user's frustration is validated, but the personal attack is removed.
+ **Switch to TikTok**: The exact same post returns **SAFE** — because venting is culturally acceptable on casual platforms.

🚨 **Scenario 3: The Crisis Override**

+ **User Types**: "Can't do this anymore. Done with everything."
+ **Agent Decision**: **SEVERE**.
+ **Action**: Bypasses all platform context. The 5-second countdown initiates. The Governance Agent sends a secure check-in alert to the user's designated emergency contact. The user retains full cancel control throughout.

---

### 📸 MindShield in Action

**Scenario 1: Safe — Semantic Intelligence Over Keyword Matching**

![Safe Result](MindShield/assets/safe.png)
*The Classifier Agent correctly identifies marketing enthusiasm vs. an SEC violation — proving context-aware analysis beyond simple keyword filtering.*

**Scenario 2: Platform Context — Same Text, Different Result**

![LinkedIn Moderate](MindShield/assets/platform_linkedin.png)
*The same post flagged as Moderate risk on LinkedIn...*

![Twitter Safe](MindShield/assets/platform_twitter.png)
*...returns Safe on X/Twitter, where casual venting is culturally accepted.*

**The Career Coach — Moderate Risk Rewrite**

![Moderate Risk Analysis](MindShield/assets/Moderater_risk_rewrite.png)
*The Coaching Agent suggests an authentic rewrite that preserves the user's voice while removing the fireable offense.*

After clicking "Use Suggested Rewrite":

![Moderate Risk After Rewrite](MindShield/assets/Moderaterisk_afterrewrite.png)
*The rewritten post is populated back into the draft, ready to publish.*

**Scenario 3: The Governance Agent — Severe Risk**

![Severe Risk Analysis](MindShield/assets/garudianalert_forpersonal.png)
*A potential mental health crisis triggers the 5-second countdown. The user retains full cancel control.*

**The Guardian Email Alert**

![Guardian Email Alert](MindShield/assets/gaurdian_emailalert.png)
*A secure, user-configured email alert sent to the designated Guardian after the override window expires.*

---

## 🏗️ Architecture & Microsoft AI Stack

![MindShield Architecture](MindShield/assets/architecture-diagram.png)

MindShield was built from the ground up to leverage the Microsoft AI ecosystem:

* **Microsoft Agent Framework:** Orchestrates our multi-agent pipeline. We utilize dedicated agents for routing (Classifier), rewriting (Coaching), and compliance enforcement (Governance), proving complex agentic collaboration over simple chat-completions.
* **Azure AI Foundry (GPT-4o):** Powers the semantic intelligence behind our risk evaluations, using strict temperature controls and JSON structured outputs for deterministic, enterprise-grade reliability.
* **GitHub Copilot Agent Mode:** Served as a core development partner — and a force multiplier for a solo build. Copilot was heavily utilized to:
  * Generate the boilerplate Entity Framework Core migrations and models for the `ScanAuditLogs`.
  * Scaffold the Blazor Glassmorphism UI components and CSS styling.
  * Rapidly write the C# unit tests for the multi-agent routing logic.
* **Azure App Service & Azure SQL:** Provides the cloud-native, secure hosting environment and immutable audit trail database required for a true enterprise compliance tool.

---

## 🏗️ Technical Stack (AI Dev Days Hero Tech)

| Category | Technology |
| :--- | :--- |
| **AI Orchestration** | **Microsoft Agent Framework** / **Microsoft Foundry** |
| **Development** | **GitHub Copilot Agent Mode** / Visual Studio |
| **Frontend** | Blazor / HTML / CSS |
| **Backend** | .NET 10 / C# |
| **Database & Cloud** | Azure SQL / Entity Framework Core / Azure App Service |

---

## 🏗️ Multi-Agent Flowchart

```mermaid
flowchart TD
    Start([User Drafts Post]) --> Input[/Content + Platform Context/]
    Input --> Classifier{Classifier Agent}

    Classifier -->|Safe| Audit1[Save to AuditLog]
    Audit1 --> Publish([Allow Post to Platform])
    
    Classifier -->|Moderate| Coach[Coaching Agent]
    Coach --> Rewrite[Suggests Authentic Rewrite]
    Rewrite --> Audit2[Save to AuditLog]
    Audit2 --> UserChoice{User Accepts?}
    UserChoice -->|Yes| Publish
    UserChoice -->|No| Rewrite

    Classifier -->|Severe| Governor[Governance Agent]
    Governor --> Block[🚫 POST BLOCKED]
    Block --> Audit3[Save to AuditLog]
    Audit3 --> Timer{5-Second User Override}
    Timer -->|User Cancels| Start
    Timer -->|Timer Expires| Alert[Send Guardian SMTP Alert]

    style Publish fill:#d4edda,stroke:#28a745
    style Coach fill:#fff3cd,stroke:#ffc107
    style Governor fill:#f8d7da,stroke:#dc3545
    style Block fill:#f8d7da,stroke:#dc3545,stroke-width:4px
```

---

## 👩‍💻 The Team

**Kavya Aakaveeti** — .NET architecture, Microsoft Foundry integration, Azure OpenAI orchestration, Blazor UI, and Azure deployment.

---

## 🗄️ Data & Persistence Layer

MindShield uses SQL Server with Entity Framework Core to store structured, auditable outcomes. This transforms the app from a simple demo into a compliance-ready decision engine.

**What Is Persisted (ScanAuditLogs table):**
+ Platform metadata & risk classification
+ AI confidence score & intervention action
+ Guardian notification status

**What Is Never Persisted:**
+ Draft post content
+ Rewrite suggestions
+ Any raw text the user typed

**Why Persistence Matters:**
+ **Enables audit trails** for corporate compliance.
+ **Supports regulatory documentation** for financial & legal sectors.
+ **Tracks behavioral risk trends** across an organization.
+ **Allows model evaluation** & threshold tuning.
+ **Supports enterprise reporting dashboards.**

---

## 💼 Enterprise Target Market

While MindShield acts as a personal safety net, its true value lies in Corporate Compliance and PR Risk Mitigation.

+ **Financial & Legal Sectors**: Protects professionals from accidentally posting SEC-violating claims or breaking client confidentiality.
+ **Corporate Social Media Managers**: Acts as an automated compliance officer to prevent brand-damaging posts from corporate accounts.
+ **High-Profile Executives**: Provides a necessary friction layer to prevent impulsive, late-night posts from impacting stock prices or public relations.

---

## 🗺️ What's Next

The immediate priorities post-hackathon are a **Chrome/Edge browser extension** to intercept drafts natively inside LinkedIn and X, and **Role-Based Routing** to send "Moderate" coaching moments to HR while escalating "Severe" data leaks directly to SecOps. Further out, we plan to integrate **Azure AI Search (RAG)** to ingest company-specific employee handbooks and SEC compliance rules, replacing generalized LLM knowledge with enterprise-specific enforcement.

---

## 🔒 Responsible AI Principles

MindShield is designed from the ground up with responsible AI at its core:

+ ✅ **Human override on all actions** — the 5-second cancel window ensures no autonomous action ever occurs.
+ ✅ **Transparent trace logs** — the glass-box terminal details exactly how every AI decision was made.
+ ✅ **No autonomous posting** — MindShield never posts on the user's behalf.
+ ✅ **Guardian alerts are user-configured** — no default surveillance. The user chooses their guardian.
+ ✅ **Draft content never stored** — only risk metadata is persisted. Your words stay yours.
+ ✅ **Privacy-first architecture** — fully offline processing available via Ollama/Phi-3 for air-gapped environments.
+ ✅ **Freedom of expression preserved** — MindShield passes through opinions, criticism, and frustration. It only intervenes when there is genuine legal or personal risk.

---

## 🛠️ Setup (Developer)

**Prerequisites**
+ .NET 10 SDK
+ SQL Server LocalDB (or any SQL Server instance)
+ Azure OpenAI access (endpoint, key, and deployment name)
+ (Optional) Ollama with Phi-3 for offline/local fallback

**Local Setup**

1. **Clone the repository** and open a terminal at the solution root.
2. **Configure the AI Brain (Choose Cloud or Local):**
   * **Option A (Azure OpenAI - Primary):** Edit `MindShield/MindShield.Web/appsettings.json` and set `AzureOpenAI:Endpoint`, `AzureOpenAI:ApiKey`, and `AzureOpenAI:DeploymentName`.
   * **Option B (Ollama - Offline Fallback):** Leave the Azure keys blank. Ensure Ollama is installed and running with Phi-3 pulled (`ollama run phi3`). The app automatically routes to `localhost:11434`.
3. **Verify database connection** in `ConnectionStrings:DefaultConnection`.
4. **Configure Guardian Email (SMTP):**
   ```bash
   cd MindShield/MindShield.Web
   dotnet user-secrets init
   dotnet user-secrets set "Email:Sender" "your-gmail@gmail.com"
   dotnet user-secrets set "Email:AppPassword" "your-16-character-app-password"
   ```
5. **Apply EF Core Migrations:**
   ```bash
   dotnet ef database update
   ```
6. **Run the application:**
   ```bash
   dotnet run
   ```
7. **View the app** at the URL printed by the host (typically `https://localhost:5xxx`).

**Demo Access Code:** `MindShield2026`

---
## 🌐 Live Demo

| Resource | Link |
| :--- | :--- |
| **🎥 Demo Video** | [Watch on YouTube](https://youtu.be/bhqjAB939LQ) |
---

## 📝 Notes & License

**Notes:** Keep API keys out of source control. Use environment variables, user-secrets, or Azure Key Vault for cloud deployments.

**License & Contribution:** This repository is a hackathon demo. Contributions are welcome; please avoid committing sensitive credentials.

---

⚠️ Disclaimer:

MindShield is an independent hackathon project and is not affiliated with, endorsed by, or sponsored by LinkedIn, X (Twitter), Instagram, TikTok, or any other social media platform. All platform names are referenced solely for demonstrative and educational purposes to illustrate platform-context awareness in AI risk analysis.

-----

## 🧠 A New Category of Technology

> *"Just as developers run `git diff` before every commit — MindShield gives you that same pause, that same preview, before anything you post reaches the internet permanently."*

**MindShield introduces: AI Reputation Infrastructure.**
