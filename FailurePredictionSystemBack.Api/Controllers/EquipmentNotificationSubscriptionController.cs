using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.ServiceLayer.Services.EquipmentNotificationSubscriptionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FailurePredictionSystemBack.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/equipment-notification-subscriptions")]
public class EquipmentNotificationSubscriptionController : ControllerBase
{
    private readonly IEquipmentNotificationSubscriptionService _subscriptionService;

    public EquipmentNotificationSubscriptionController(
        IEquipmentNotificationSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost("{equipmentId:guid}")]
    public async Task<IActionResult> Subscribe(
        [FromRoute] Guid equipmentId,
        [FromBody] UpdateEquipmentNotificationSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.SubscribeAsync(
            equipmentId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{equipmentId:guid}")]
    public async Task<IActionResult> Unsubscribe(
        [FromRoute] Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.UnsubscribeAsync(
            equipmentId,
            cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMySubscriptions(
        CancellationToken cancellationToken)
    {
        var result = await _subscriptionService.GetMySubscriptionsAsync(
            cancellationToken);

        return Ok(result);
    }
}