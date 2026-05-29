using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystem.DataGenerator.Models;
using FailurePredictionSystem.DataGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var settings = configuration
    .GetSection("GeneratorSettings")
    .Get<GeneratorSettings>();

if (settings is null)
    throw new InvalidOperationException("Секция GeneratorSettings не найдена.");

var services = new ServiceCollection();

services.AddSingleton(settings);
services.AddSingleton<SyntheticMetricGenerator>();
services.AddHttpClient<MetricSender>();

var serviceProvider = services.BuildServiceProvider();

var generator = serviceProvider.GetRequiredService<SyntheticMetricGenerator>();
var sender = serviceProvider.GetRequiredService<MetricSender>();

Console.WriteLine("Генератор синтетических метрик запущен.");
Console.WriteLine($"AgentId: {settings.AgentId}");
Console.WriteLine($"EquipmentId: {settings.EquipmentId}");
Console.WriteLine($"ApiUrl: {settings.ApiUrl}");
Console.WriteLine($"TotalRecords: {settings.TotalRecords}");
Console.WriteLine($"Include state: {settings.IncludeState}");
Console.WriteLine();

var startTime = DateTime.UtcNow.AddDays(-30);
var interval = TimeSpan.FromMinutes(10);

var successCount = 0;
var failCount = 0;

using var cts = new CancellationTokenSource();

for (var i = 0; i < settings.TotalRecords; i++)
{
    var timestamp = startTime.Add(interval * i);

    var payload = generator.Generate(timestamp, i, settings.TotalRecords);

    var success = await sender.SendAsync(payload, cts.Token);

    if (success)
        successCount++;
    else
        failCount++;

    if (i % 100 == 0)
    {
        Console.WriteLine($"Сгенерировано: {i}/{settings.TotalRecords}. Успешно={successCount}, Ошибки={failCount}");
    }

    if (settings.DelayMs > 0)
        await Task.Delay(settings.DelayMs, cts.Token);
}

Console.WriteLine();
Console.WriteLine("Генерация завершена.");
Console.WriteLine($"Успешно отправлено: {successCount}");
Console.WriteLine($"Ошибок отправки: {failCount}");