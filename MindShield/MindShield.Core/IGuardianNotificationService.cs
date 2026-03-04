using System;
using System.Collections.Generic;
using System.Text;

namespace MindShield.Web.Services
{
    public interface IGuardianNotificationService
    {
        Task SendAlertAsync(string guardianName, string guardianEmail, string riskReason, string originalText);
    }
}
