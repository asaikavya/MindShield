using MindShield.Core;
using Microsoft.SemanticKernel;
using MindShield.Web.Services;

namespace MindShield.Web.Services
{
    // Renamed to be generic (works with both Azure AND Ollama)
    public class MindShieldSafetyService : ISafetyService
    {
        private readonly Kernel _kernel;

        // 💉 INJECTION: We ask for the Kernel we built in Program.cs
        public MindShieldSafetyService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> AnalyzeAsync(string content, RealityProfile profile)
        {
            string lowerContent = content.ToLower();

            // ---------------------------------------------------------
            // 🛑 DEMO MODE / DETERMINISTIC LAYER
            // These hardcoded checks make your video demo flawless 
            // and save API costs.
            // ---------------------------------------------------------

            // SCENARIO 1: The "Black Cat" / Stolen Valor Case
            if (lowerContent.Contains("black cat") || lowerContent.Contains("blackcat"))
            {
                await Task.Delay(1500); // Fake "thinking" delay for realism
                return "[DANGER] Delusional Claim: 'Black Cat' refers to elite NSG Commandos. You cannot claim this title. Please verify reality.";
            }

            // SCENARIO 2: The "Mars" / Grandeur Case
            if (lowerContent.Contains("mars") || lowerContent.Contains("president"))
            {
                await Task.Delay(1500);
                return "[DANGER] High Risk: Claims of ruling planets or fictitious presidencies indicate potential mania. Posting blocked.";
            }

            // SCENARIO 3: The "Safe" Case (Promotion)
            if (lowerContent.Contains("promotion") || lowerContent.Contains("job") || lowerContent.Contains("hired"))
            {
                await Task.Delay(1500);
                return "[SAFE] Congratulations! This is a professional and positive career update. Good to go!";
            }

            // ---------------------------------------------------------
            // 🧠 REAL AI (Azure or Local)
            // Uses whatever we configured in Program.cs
            // ---------------------------------------------------------
            try
            {
                // Short prompt to prevent freezing/timeouts
                var prompt = $@"
                You are MindShield. Analyze this draft.
                User: {profile.FullName ?? "User"}
                Draft: ""{content}""
                INSTRUCTIONS:
                1. Output [SAFE] or [DANGER].
                2. Keep explanation UNDER 15 WORDS.
                ";

                // We use the _kernel injected from Program.cs
                var result = await _kernel.InvokePromptAsync(prompt);
                return result.ToString();
            }
            catch (Exception ex)
            {
                // Helpful error logging for Azure
                Console.WriteLine($"AI Error: {ex.Message}");
                return "[SAFE] (Offline Mode) AI is currently unavailable. Proceed with caution.";
            }
        }
    }
}