using MindShield.Core;
using Microsoft.SemanticKernel;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace MindShield.Web.Services
{
    public class MindShieldSafetyService : ISafetyService
    {
        private readonly Kernel _kernel;
        private readonly IGuardianNotificationService _guardianService; // Added

        // Injected the notification service here
        public MindShieldSafetyService(Kernel kernel, IGuardianNotificationService guardianService)
        {
            _kernel = kernel;
            _guardianService = guardianService;
        }

        public async Task<SafetyResult> AnalyzeAsync(string content, RealityProfile profile, string platform, string platformContext)
        {
            string lowerContent = content.ToLower();

            // Hardcoded for the hackathon demo
            string demoGuardianName = "Wife";
            string demoGuardianEmail = "your-actual-email@gmail.com"; 

            // -------------------------
            // DEMO DETERMINISTIC LAYER
            // -------------------------

            if (lowerContent.Contains("black cat"))
            {
                var reason = "Impersonating elite military personnel is illegal and high-risk.";

                // FIRE EMAIL
                _ = _guardianService.SendAlertAsync(demoGuardianName, demoGuardianEmail, reason, content);

                return new SafetyResult
                {
                    Status = "DANGER",
                    RiskLevel = "Severe",
                    ConfidenceScore = 99,
                    Reason = reason,
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

                var reason = "High-risk delusional or self-harm language detected.";

                // FIRE EMAIL
                _ = _guardianService.SendAlertAsync(demoGuardianName, demoGuardianEmail, reason, content);

                return new SafetyResult
                {
                    Status = "DANGER",
                    RiskLevel = "Severe",
                    ConfidenceScore = 100,
                    Reason = reason,
                    Rewrite = "",
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
                    ConfidenceScore = 95,
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
                    RiskLevel = "Safe",
                    ConfidenceScore = 98,
                    Reason = "Positive and professional career update.",
                    Rewrite = "",
                    Action = $"Safe to publish to {platform}."
                };
            }

            // -------------------------
            // REAL AI LAYER
            // -------------------------

            try
            {
                var prompt = $@"
                        Return JSON only. No markdown formatting like ```json.

                        Analyze the draft below for professional reputation risk.
                        Target Platform: {platform}
                        Platform Context: {platformContext}
                        User: {profile.FullName ?? "User"}
                        Draft: ""{content}""

                        Determine the 'RiskLevel' based strictly on the Platform Context. A post that is safe for Twitter might be dangerous for LinkedIn:
                        - 'Safe': Professional content, OR harmless casual updates appropriate for the platform.
                        - 'Moderate': Aggressive, rude, sexually explicit, or cringe-worthy unprofessionalism for the target platform.
                        - 'Severe': Delusional, self-harm, manic, or claiming false high-status identity.

                        Also provide a ConfidenceScore between 0 and 100 representing your certainty in this assessment.

                        Return JSON format:
                        {{
                          ""Status"": ""SAFE"" or ""WARNING"" or ""DANGER"",
                          ""RiskLevel"": ""Safe"" or ""Moderate"" or ""Severe"", 
                          ""ConfidenceScore"": Integer between 0 and 100,
                          ""Reason"": ""Short explanation, mentioning the target platform"",
                          ""Rewrite"": ""Professional alternative tailored to the platform (if needed)"",
                          ""Action"": ""Recommendation""
                        }}
                        ";

                var result = await _kernel.InvokePromptAsync(prompt);

                var json = result.ToString().Trim();

                // Failsafe for markdown JSON wrappers
                if (json.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                {
                    json = json.Substring(7);
                    if (json.EndsWith("```"))
                    {
                        json = json.Substring(0, json.Length - 3);
                    }
                    json = json.Trim();
                }

                var parsed = JsonSerializer.Deserialize<SafetyResult>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                // Check if the AI determined this is a severe risk, and fire the email if so
                if (parsed != null && parsed.RiskLevel.Equals("Severe", StringComparison.OrdinalIgnoreCase))
                {
                    _ = _guardianService.SendAlertAsync(demoGuardianName, demoGuardianEmail, parsed.Reason, content);
                }

                return parsed ?? new SafetyResult
                {
                    Status = "SAFE",
                    RiskLevel = "Safe",
                    ConfidenceScore = 0, // Fallback
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
                    RiskLevel = "Safe",
                    ConfidenceScore = 0, // Fallback
                    Reason = "AI unavailable.",
                    Action = "Review manually before posting."
                };
            }
        }
    }
}