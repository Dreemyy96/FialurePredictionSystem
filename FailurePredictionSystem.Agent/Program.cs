using FailurePredictionSystem.Agent;
using FailurePredictionSystem.Agent.Models.ConfigModels;
using FailurePredictionSystem.Agent.Services;
using FailurePredictionSystem.Agent.Services.MetricCollector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<AgentSetting>(builder.Configuration.GetSection("AgentSettings"));

builder.Services.AddWindowsService(options => { options.ServiceName = "Failure Prediction Metric Agent"; });

builder.Services.AddHttpClient<MetricSender>();
builder.Services.AddSingleton<IMetricCollector, WindowsMetricCollector>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();