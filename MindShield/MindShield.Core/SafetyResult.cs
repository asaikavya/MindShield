using System;
using System.Collections.Generic;
using System.Text;

namespace MindShield.Core
{
    public class SafetyResult
    {
        public string Status { get; set; } = "SAFE";   // SAFE or DANGER
        public string Reason { get; set; } = "";
        public string Rewrite { get; set; } = "";
        public string Action { get; set; } = "";

        public string? RiskLevel { get; set; } // Mild / Moderate / Severe
    }
}
