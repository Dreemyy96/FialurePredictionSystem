using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.ServiceLayer.Services.DashboardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FailurePredictionSystemBack.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/equipment/{equipmentId:guid}")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetEquipmentDashboard(
        [FromRoute] Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var result = await _dashboardService.GetEquipmentDashboardAsync(
            equipmentId,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}