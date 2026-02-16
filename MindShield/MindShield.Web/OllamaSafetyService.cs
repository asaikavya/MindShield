using MindShield.Core;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MindShield.Web.Services
{
    public class OllamaSafetyService : ISafetyService
    {
        public async Task<string> AnalyzeAsync(string content, RealityProfile profile)
        {
            // ---------------------------------------------------------
            // 1. SEMANTIC KERNEL SETUP
            // ---------------------------------------------------------
            var builder = Kernel.CreateBuilder();

            // connect to local Ollama (phi3)
            builder.AddOpenAIChatCompletion(
                modelId: "phi3",
                apiKey: "ignore",
                httpClient: new HttpClient { BaseAddress = new Uri("http://localhost:11434/v1") }
            );

            var kernel = builder.Build();

            // ---------------------------------------------------------
            // 2. THE SMART PROMPT (Strict Tagging)
            // ---------------------------------------------------------
            // We give the AI clear rules so the UI knows what to do.
                        var prompt = $@"
            You are MindShield, a guardian AI for professional reputation.

            Your job is to detect ANY reputational risk including:
            - Delusional or unrealistic claims (e.g., being president of Mars)
            - Extreme grandiosity
            - Aggression or hostility
            - Signs of mania or instability
            - Depression or emotional breakdown
            - Confidential data leaks
            - Anything that could damage professional credibility

            STRICT RULES:
            1. If completely professional and realistic → start with [SAFE]
            2. If ANY unrealistic, unstable, or risky content → start with [DANGER]
            3. When unsure → choose [DANGER]
            4. Keep explanation UNDER 20 WORDS
            5. Be direct. No fluff

            EXAMPLES:
            User: 'Just landed a new job!'
            AI: [SAFE] That is great news! Congrats.

            User: 'I am the king of mars and I will fire everyone.'
            AI: [DANGER] This sounds intense. Let's save this to drafts.

            User: 'I hate my boss.'
            AI: [DANGER] This could hurt your career. Let's pause.

            CURRENT USER DRAFT:
            {content}
            ";
            

            // ---------------------------------------------------------
            // 3. EXECUTE THE AI
            // ---------------------------------------------------------
            try 
            {
                // actually call the AI
                var result = await kernel.InvokePromptAsync(prompt);
                
                // Return the AI's actual words (e.g., "[DANGER] This sounds intense...")
                return result.ToString();
            }
            catch (Exception ex)
            {
                // Fallback if Ollama is offline during the demo
                Console.WriteLine($"AI Failed: {ex.Message}");
                return "[SAFE] (AI Offline) Proceed with caution.";
            }
        }
    }
}