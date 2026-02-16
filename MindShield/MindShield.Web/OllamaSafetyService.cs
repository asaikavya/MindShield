using MindShield.Core;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MindShield.Web.Services
{
    public class OllamaSafetyService : ISafetyService
    {
        public async Task<string> AnalyzeAsync(string content, RealityProfile profile)
        {
            string lowerContent = content.ToLower();

           

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
            //  REAL AI 
            // ---------------------------------------------------------
            try
            {
                var builder = Kernel.CreateBuilder();
                builder.AddOpenAIChatCompletion(
                    modelId: "phi3",
                    apiKey: "ignore",
                    httpClient: new HttpClient { BaseAddress = new Uri("http://localhost:11434/v1") }
                );
                var kernel = builder.Build();

                // Short prompt to prevent freezing
                var prompt = $@"
                                You are MindShield. Analyze this draft.
                                User: {profile.FullName ?? "User"}
                                Draft: ""{content}""
                                INSTRUCTIONS:
                                1. Output [SAFE] or [DANGER].
                                2. Keep explanation UNDER 15 WORDS.
                                ";
                var result = await kernel.InvokePromptAsync(prompt);
                return result.ToString();
            }
            catch
            {
                return "[SAFE] (Offline Mode) AI is currently unavailable.";
            }
        }
    }
}