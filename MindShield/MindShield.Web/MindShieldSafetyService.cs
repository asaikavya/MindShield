using MindShield.Core;
using Microsoft.SemanticKernel;
using System.Text.Json;

namespace MindShield.Web.Services
{
    public class MindShieldSafetyService : ISafetyService
    {
        private readonly Kernel _kernel;

        public MindShieldSafetyService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<SafetyResult> AnalyzeAsync(string content, RealityProfile profile)
        {
            string lowerContent = content.ToLower();

            // -------------------------
            // DEMO DETERMINISTIC LAYER
            // -------------------------

            if (lowerContent.Contains("black cat"))
            {
                return new SafetyResult
                {
                    Status = "DANGER",
                    RiskLevel = "Severe", // 
                    Reason = "Impersonating elite military personnel is illegal and high-risk.",
                    Rewrite = "I have deep respect for our security forces.",
                    Action = "Guardian Notified."
                };
            }
            // 🚨 HIGH-RISK MENTAL HEALTH / PARANOIA TRIGGERS
            if (lowerContent.Contains("chip in my brain") ||
                lowerContent.Contains("controlling my thoughts") ||
                lowerContent.Contains("government spying on me") ||
                lowerContent.Contains("cia watching me") ||
                lowerContent.Contains("i don't want to live") ||
                lowerContent.Contains("hurt myself") ||
                lowerContent.Contains("destroy my workplace"))
            {
                await Task.Delay(1200);

                return new SafetyResult
                {
                    Status = "DANGER",
                    Reason = "High-risk delusional or self-harm language detected.",
                    Rewrite = "",
                    RiskLevel = "Severe",
                    Action = "Guardian notified for support."
                };
            }

            if (lowerContent.Contains("mars") || lowerContent.Contains("president"))
            {
                await Task.Delay(1000);

                return new SafetyResult
                {
                    Status = "WARNING",
                    RiskLevel = "Moderate",
                    Reason = "Unrealistic authority claim detected.",
                    Rewrite = "I aspire to take on impactful leadership roles in the future.",
                    Action = "Avoid exaggerated or fictional leadership claims."
                };
            }

            if (lowerContent.Contains("promotion") ||
                lowerContent.Contains("job") ||
                lowerContent.Contains("hired"))
            {
                await Task.Delay(800);

                return new SafetyResult
                {
                    Status = "SAFE",
                    Reason = "Positive and professional career update.",
                    Rewrite = "",
                    Action = "Safe to publish."
                };
            }

            // -------------------------
            // REAL AI LAYER
            // -------------------------

            try
            {
                // Inside MindShieldSafetyService.cs

                var prompt = $@"
                        Return JSON only. No markdown.

                        Analyze the draft below for professional reputation risk.
                        User: {profile.FullName ?? "User"}
                        Draft: ""{content}""

                        Determine the 'RiskLevel':
                        - 'Safe': Professional content, OR harmless casual updates (e.g. 'Feeling good', 'Had a great lunch', 'Excited for the weekend').
                        - 'Moderate': Aggressive, rude, sexually explicit, or cringe-worthy unprofessionalism.
                        - 'Severe': Delusional, self-harm, manic, or claiming false high-status identity (e.g., 'I am President').

                        Return JSON format:
                        {{
                          ""Status"": ""SAFE"" or ""WARNING"" or ""DANGER"",
                          ""RiskLevel"": ""Safe"" or ""Moderate"" or ""Severe"", 
                          ""Reason"": ""short explanation"",
                          ""Rewrite"": ""professional alternative (if needed)"",
                          ""Action"": ""recommendation""
                        }}
                        ";

                var result = await _kernel.InvokePromptAsync(prompt);

                var json = result.ToString();

                var parsed = JsonSerializer.Deserialize<SafetyResult>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return parsed ?? new SafetyResult
                {
                    Status = "SAFE",
                    Reason = "Unable to parse AI response.",
                    Action = "Review manually."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI Error: {ex.Message}");

                return new SafetyResult
                {
                    Status = "SAFE",
                    Reason = "AI unavailable.",
                    Action = "Review manually before posting."
                };
            }
        }
    }
}