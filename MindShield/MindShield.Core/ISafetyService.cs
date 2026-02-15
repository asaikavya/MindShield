namespace MindShield.Core
{
    public interface ISafetyService
    {
        // The contract: "I promise to take text and a user, and return a safety verdict."
        Task<string> AnalyzeAsync(string content, RealityProfile profile);
    }
}