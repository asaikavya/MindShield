namespace MindShield.Core
{
    public class SafetyResult
    {
        public string Status { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;

     
        public int ConfidenceScore { get; set; }

        public string Reason { get; set; } = string.Empty;
        public string Rewrite { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;

        
    }
}