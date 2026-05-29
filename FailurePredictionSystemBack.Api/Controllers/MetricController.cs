using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.ServiceLayer.Services.EquipmentService;
using FailurePredictionSystemBack.ServiceLayer.Services.MetricService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FailurePredictionSystemBack.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/metric")]
public class MetricController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;
    private readonly IMetricService _metricService;

    public MetricController(
        IEquipmentService equipmentService,
        IMetricService metricService)
    {
        _equipmentService = equipmentService;
        _metricService = metricService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateMetricRequest request, CancellationToken cancellationToken)
    {
        if (!Request.Headers.TryGetValue("X-Agent-Token", out var tokenValues))
            return Unauthorized(new { message = "Отсутствует токен агента." });

        var token = tokenValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized(new { message = "Некорректный токен агента." });

        var isValidAgent = await _equipmentService.ValidateAgentAsync(request.AgentId, token, cancellationToken);

        if (!isValidAgent)
            return Unauthorized(new { message = "Агент не прошёл проверку." });

        await _metricService.AddMetricAsync(request, cancellationToken);
        return Ok(new { message = "Метрики сохранены." });
    }

    [HttpGet("equipment/{equipmentId:guid}")]
    public async Task<IActionResult> GetByEquipment([FromRoute] Guid equipmentId, [FromQuery] int limit = 100,
        CancellationToken cancellationToken = default)
    {
        var metrics = await _metricService.GetMetricsByEquipmentAsync(equipmentId, limit, cancellationToken);
        return Ok(metrics);
    }
}