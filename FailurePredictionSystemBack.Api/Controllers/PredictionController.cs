using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.ServiceLayer.Services.PredictionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FailurePredictionSystemBack.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/equipment/{equipmentId:guid}")]
public class PredictionController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    public PredictionController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpGet("predictions")]
    public async Task<IActionResult> GetByEquipmentId([FromRoute] Guid equipmentId, CancellationToken cancellationToken)
    {
        var result = await _predictionService.GetByEquipmentIdAsync(equipmentId, cancellationToken);

        return Ok(result);
    }

    [HttpGet("latest-prediction")]
    public async Task<IActionResult> GetLatestByEquipmentId([FromRoute] Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var result = await _predictionService.GetLatestByEquipmentIdAsync(
            equipmentId,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}