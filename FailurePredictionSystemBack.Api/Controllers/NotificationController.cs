using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FailurePredictionSystemBack.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] bool? isRead,
        CancellationToken cancellationToken)
    {
        var result = await _notificationService.GetCurrentUserNotificationsAsync(
            isRead,
            cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isRead,
        CancellationToken cancellationToken)
    {
        var result = await _notificationService.GetAllAsync(
            isRead,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(
        [FromRoute] Guid notificationId,
        CancellationToken cancellationToken)
    {
        var result = await _notificationService.MarkAsReadAsync(
            notificationId,
            cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }
}