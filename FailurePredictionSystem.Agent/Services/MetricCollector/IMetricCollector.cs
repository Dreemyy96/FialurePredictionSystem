using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystem.Agent.Models;

namespace FailurePredictionSystem.Agent.Services.MetricCollector;

public interface IMetricCollector
{
    public Task<MetricPayload> CollectAsync(CancellationToken cancellationToken);
}