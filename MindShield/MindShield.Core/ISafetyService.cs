namespace MindShield.Core
{
    public interface ISafetyService
    {
        Task<SafetyResult> AnalyzeAsync(string content, RealityProfile profile);
    }
}