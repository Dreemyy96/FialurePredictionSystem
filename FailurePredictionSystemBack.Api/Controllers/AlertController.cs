using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.ServiceLayer.Services.AlertService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FailurePredictionSystemBack.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/alerts")]
public class AlertController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isResolved,
        CancellationToken cancellationToken)
    {
        var result = await _alertService.GetAllAsync(
            isResolved,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{alertId:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid alertId,
        CancellationToken cancellationToken)
    {
        var result = await _alertService.GetByIdAsync(
            alertId,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("equipment/{equipmentId:guid}")]
    public async Task<IActionResult> GetByEquipmentId(
        [FromRoute] Guid equipmentId,
        [FromQuery] bool? isResolved,
        CancellationToken cancellationToken)
    {
        var result = await _alertService.GetByEquipmentIdAsync(
            equipmentId,
            isResolved,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{alertId:guid}/resolve")]
    public async Task<IActionResult> Resolve(
        [FromRoute] Guid alertId,
        CancellationToken cancellationToken)
    {
        var result = await _alertService.ResolveAsync(
            alertId,
            cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }
}