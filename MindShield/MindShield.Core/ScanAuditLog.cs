using System.ComponentModel.DataAnnotations;

namespace MindShield.Core
{
    public class ScanAuditLog
    {
        [Key]
        public int Id { get; set; }

        // Who posted
        public string UserId { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;

       
        public string RiskLevel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ConfidenceScore { get; set; }
        public string Reason { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;
        public bool RewriteSuggested { get; set; } = false;
        public bool GuardianAlerted { get; set; } = false;

        
        public DateTime ScannedAt { get; set; } = DateTime.UtcNow;
    }
}