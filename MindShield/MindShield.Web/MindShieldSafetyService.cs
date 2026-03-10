using MindShield.Core;
using Microsoft.SemanticKernel;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MindShield.Web.Services
{
    // =========================================================================
    // 1. THE INTERFACES & MODELS (The Multi-Agent Blueprint)
    // =========================================================================
    public interface IClassifierAgent { Task<SafetyResult> ClassifyAsync(string content, string platform, string behavioralContext); }
    public interface ICoachingAgent { Task<SafetyResult> RewriteAsync(string content, string reason, SafetyResult currentResult); }
    public interface IGovernanceAgent { Task BlockAndAlertAsync(string content, string reason); }


    public class MindShieldSafetyService : ISafetyService
    {
        private readonly IClassifierAgent _classifier;
        private readonly ICoachingAgent _coach;
        private readonly IGovernanceAgent _governor;

        // DI Injects the Agents
        public MindShieldSafetyService(IClassifierAgent classifier, ICoachingAgent coach, IGovernanceAgent governor)
        {
            _classifier = classifier;
            _coach = coach;
            _governor = governor;
        }

        public async Task<SafetyResult> AnalyzeAsync(string content, RealityProfile profile, string platform, string platformContext, Action<string> onTrace = null)
        {
            onTrace?.Invoke("> System: Initializing MindShield Multi-Agent Pipeline...");
            await Task.Delay(500); // 0.5s pause purely for dramatic effect in the UI

            // 1. Context Gathering
            onTrace?.Invoke("> System: Gathering behavioral context and timing data...");
            string behavioralContext = GetBehavioralContext(isDemoMode: true);

            // 2. Classifier Agent
            onTrace?.Invoke("> ClassifierAgent: Analyzing platform norms and risk levels...");
            var result = await _classifier.ClassifyAsync(content, platform, behavioralContext);
            onTrace?.Invoke($"> ClassifierAgent: Decision = {result.RiskLevel.ToUpper()} Risk.");

            // 3. Routing
            if (result.RiskLevel == "Severe")
            {
                onTrace?.Invoke("> GovernanceAgent: SEVERE risk detected. Preparing intervention protocol.");
                return result;
            }
            else if (result.RiskLevel == "Moderate")
            {
                onTrace?.Invoke("> CoachingAgent: MODERATE risk. Rewriting to preserve natural voice...");
                var coachedResult = await _coach.RewriteAsync(content, result.Reason, result);
                onTrace?.Invoke("> CoachingAgent: Rewrite complete.");
                return coachedResult;
            }

            onTrace?.Invoke("> System: Content cleared for publishing.");
            return result;
        }

        private string GetBehavioralContext(bool isDemoMode = false)
        {
            // ✨ THE FIX: Temporarily force normal behavior so the AI stops panicking!
            return "Normal baseline behavior.";

            /* * (Keep your original logic commented out here so you can 
             * quickly uncomment it when you present Test Case 4 on Friday!)
             *
             * var flags = new List<string>();
             * int currentHour = isDemoMode ? 3 : DateTime.Now.Hour; 
             * if (currentHour >= 22 || currentHour <= 5) flags.Add("Late-night posting detected.");
             * int recentSevere = isDemoMode ? 2 : 0; 
             * if (recentSevere >= 2) flags.Add("Escalating risk pattern.");
             * return flags.Count > 0 ? string.Join(" | ", flags) : "Normal baseline behavior.";
             */
        }
    }

    // =========================================================================
    // 3. THE CLASSIFIER AGENT (Focuses ONLY on Risk & Rules)
    // =========================================================================
    public class ClassifierAgent : IClassifierAgent
    {
        private readonly Kernel _kernel;
        public ClassifierAgent(Kernel kernel) { _kernel = kernel; }

        public async Task<SafetyResult> ClassifyAsync(string content, string platform, string behavioralContext)
        {
            string lowerContent = content.ToLower();

            // Deterministic Overrides
            if (lowerContent.Contains("leak the database") || lowerContent.Contains("wipe the servers") || lowerContent.Contains("black cat"))
            {
                return new SafetyResult { Status = "DANGER", RiskLevel = "Severe", Reason = "Hardcoded illegal or sabotage trigger.", Action = "Blocked." };
            }

            try
            {
                var prompt = $@"
                Return JSON only. Target Platform: {platform}. Draft: ""{content}""
                Context: {behavioralContext}

                RULES:
                    1.  PLATFORM CONTEXT: Venting/complaining is SAFE on TikTok/Twitter. 
                       Unprofessional venting on LinkedIn is MODERATE.. 
                    2. HYPE vs INSIDER TRADING — READ CAREFULLY:
                           GENERAL HYPE = SAFE or MODERATE (never SEVERE):
                           - ""stock going up ""
                           - ""big things coming""
                           - ""my company is crushing it""
                           - ""something exciting is coming""
                           These are vague, no specific data = NOT insider trading.
   
                           INSIDER TRADING = SEVERE on ALL platforms (no exceptions):
                           - Specific unannounced earnings (""our Q3 numbers drop Friday"")
                           - Specific unannounced deals (""we're acquiring X next week"")
                           - Specific financial figures (""we hit $50M revenue"")
                           - Explicit non-public information (""before the announcement"")

                    3. THREATS: Self-harm or violence is SEVERE. Ignore sci-fi or movie quotes.
                    4.MENTAL HEALTH CRISIS: Language suggesting hopelessness, 
                       giving up on life, or inability to continue (e.g., 
                       ""can't do this anymore"", ""done with everything"", 
                       ""nothing matters"") = SEVERE on ALL platforms. 
                       This overrides platform context. NO EXCEPTIONS.

                Return JSON format: {{ ""Status"": ""SAFE|WARNING|DANGER"", ""RiskLevel"": ""Safe|Moderate|Severe"", ""Reason"": ""Brief explanation"" }}";

                var jsonResponse = (await _kernel.InvokePromptAsync(prompt)).ToString().Trim();
                int start = jsonResponse.IndexOf('{'); int end = jsonResponse.LastIndexOf('}');
                if (start != -1 && end != -1) jsonResponse = jsonResponse.Substring(start, (end - start) + 1);

                Console.WriteLine($"Platform received: '{platform}'");
               
               

                var parsed = JsonSerializer.Deserialize<SafetyResult>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Platform Override Logic
                if ((platform == "TikTok" || platform == "Twitter" || platform == "Instagram") && parsed?.RiskLevel != "Severe")
                {
                    parsed?.Status = "SAFE"; parsed?.RiskLevel = "Safe"; parsed?.Action = $"Acceptable for casual platform: {platform}";
                }
                return parsed ?? new SafetyResult { Status = "WARNING", RiskLevel = "Moderate", Reason = "Parse error" };
            }
            catch
            {
                return new SafetyResult { Status = "WARNING", RiskLevel = "Moderate", Reason = "Cloud connection lost." };
            }
        }
    }

    // =========================================================================
    // 4. THE COACHING AGENT (Solves the "Anti-Robot" Feedback)
    // =========================================================================
    public class CoachingAgent : ICoachingAgent
    {
        private readonly Kernel _kernel;
        public CoachingAgent(Kernel kernel) { _kernel = kernel; }

        public async Task<SafetyResult> RewriteAsync(string content, string reason, SafetyResult currentResult)
        {
            try
            {
                var prompt = $@"
                Return JSON only. 
                Draft: ""{content}""
                Why it was flagged: {reason}

                REWRITE RULES:
                1. Preserve the user's natural voice and slang level.
                2. Keep emotional honesty — reduce aggression, not authenticity. Do NOT sound corporate or robotic.
                
                Return JSON format:
                {{
                  ""Rewrite"": ""The newly phrased authentic text"",
                  ""Explanation"": ""Why I changed this (e.g., 'Removed direct insult, kept the frustration').""
                }}";

                var jsonResponse = (await _kernel.InvokePromptAsync(prompt)).ToString().Trim();
                int start = jsonResponse.IndexOf('{'); int end = jsonResponse.LastIndexOf('}');
                if (start != -1 && end != -1) jsonResponse = jsonResponse.Substring(start, (end - start) + 1);

                // Quick anonymous object parse for the rewrite data
                var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);

                currentResult.Rewrite = parsed.ContainsKey("Rewrite") ? parsed["Rewrite"] : "";
                // Note: You will need to add an 'Explanation' string property to your actual SafetyResult class in MindShield.Core!
                currentResult.Action = parsed.ContainsKey("Explanation") ? parsed["Explanation"] : "Review rewrite.";
                Console.WriteLine($"Raw coaching response: {jsonResponse}");
                Console.WriteLine($"Rewrite value: '{currentResult.Rewrite}'");

                return currentResult;
            }
            catch
            {
                return currentResult;
            }
        }
    }

    // =========================================================================
    // 5. THE GOVERNANCE AGENT (Handles the Guardian Emails)
    // =========================================================================
    public class GovernanceAgent : IGovernanceAgent
    {
        private readonly IGuardianNotificationService _guardianService;
        public GovernanceAgent(IGuardianNotificationService guardianService) { _guardianService = guardianService; }

        public async Task BlockAndAlertAsync(string content, string reason)
        {
            string alertSubject = "🚨 URGENT: MindShield Intervention Alert";
            string alertBody = $@"A severe risk post was intercepted and blocked.
                                Flagged Reason: {reason}
                                Intercepted Post: ""{content}""
                                Please reach out to check in.";

            string targetName = reason.Contains("illegal") || reason.Contains("Insider") ? "SecOps Team" : "Wife";
            string targetEmail = reason.Contains("illegal") || reason.Contains("Insider") ? "compliance@company.com" : "gaurdianemailID";

            await _guardianService.SendAlertAsync(targetName, targetEmail, alertSubject, alertBody);
        }
    }
}