using System.Threading.Tasks;

namespace MindShield.Core
{
    public interface IGuardianNotificationService
    {
        Task SendAlertAsync(string targetName, string targetEmail, string subject, string body);
    }
}