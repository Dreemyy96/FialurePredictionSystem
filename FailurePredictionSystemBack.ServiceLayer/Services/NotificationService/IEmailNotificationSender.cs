using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;

public interface IEmailNotificationSender
{
    Task SendAsync(
        Notification notification,
        string recipientEmail,
        CancellationToken cancellationToken);
}