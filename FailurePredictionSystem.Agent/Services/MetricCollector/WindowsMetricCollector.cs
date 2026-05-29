using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystem.Agent.Enums;
using FailurePredictionSystem.Agent.Models;
using FailurePredictionSystem.Agent.Models.ConfigModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FailurePredictionSystem.Agent.Services.MetricCollector;

public class WindowsMetricCollector : IMetricCollector
{
    private readonly AgentSetting _agentSetting;
    private readonly ILogger<WindowsMetricCollector> _logger;
    private readonly Random _random;

    public WindowsMetricCollector(IOptions<AgentSetting> agentSetting, ILogger<WindowsMetricCollector> logger)
    {
        _agentSetting = agentSetting.Value;
        _logger = logger;
        _random = new Random();
    }

    public async Task<MetricPayload> CollectAsync(CancellationToken cancellationToken)
    {
        var cpuUsage = await GetCpuUsageAsync(cancellationToken);
        var ramUsage = GetRamUsagePercent();
        var disk = GetDiskUsage();
        var uptimeHours = GetUptimeHours();
        return new MetricPayload()
        {
            AgentId = _agentSetting.AgentId,
            EquipmentId = _agentSetting.EquipmentId,
            Hostname = Environment.MachineName,
            TimestampUtc = DateTime.UtcNow,

            CpuUsagePercent = Math.Round(cpuUsage, 2),
            RamUsagePercent = Math.Round(ramUsage, 2),
            DiskUsagePercent = Math.Round(disk.diskUsagePercent, 2),
            FreeDiskSpaceGb = Math.Round(disk.freeDiskSpaceGb, 2),

            TemperatureCelsius = GenerateTemperature(cpuUsage),
            ErrorCount = GetRecentSystemErrorCount(),
            UptimeHours = Math.Round(uptimeHours, 2),
            State = EquipmentState.Normal
        };
    }

    private async Task<double> GetCpuUsageAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();

            await Task.Delay(1000, cancellationToken);

            var value = cpuCounter.NextValue();

            return Math.Clamp(value, 0, 100);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Не удается получить CPU через PerfomanceCounter. Используется fallback");
            return await GetCpuUsageFallbackAsync(cancellationToken);
        }
    }

    private async Task<double> GetCpuUsageFallbackAsync(CancellationToken cancellationToken)
    {
        var startTime = DateTime.UtcNow;
        var startCpuUsage = Process.GetProcesses().Sum(p =>
        {
            try
            {
                return p.TotalProcessorTime.TotalMilliseconds;
            }
            catch
            {
                return 0;
            }
        });

        await Task.Delay(500, cancellationToken);

        var endTime = DateTime.UtcNow;
        var endCpuUsage = Process.GetProcesses().Sum(p =>
        {
            try
            {
                return p.TotalProcessorTime.TotalMilliseconds;
            }
            catch
            {
                return 0;
            }
        });

        var cpuUsedMs = endCpuUsage - startCpuUsage;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;

        if (totalMsPassed <= 0)
            return 0;

        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed) * 100;
        return Math.Clamp(cpuUsageTotal, 0, 100);
    }

    private double GetRamUsagePercent()
    {
        var memoryStatus = new MemoryStatusEx
        {
            dwLength = (uint)Marshal.SizeOf<MemoryStatusEx>()
        };

        if (!GlobalMemoryStatusEx(ref memoryStatus))
        {
            _logger.LogWarning("Не удалось получить информацию об оперативной памяти.");
            return 0;
        }

        var totalMemory = memoryStatus.ullTotalPhys;
        var availableMemory = memoryStatus.ullAvailPhys;

        if (totalMemory == 0)
            return 0;

        var usedMemory = totalMemory - availableMemory;

        return (double)usedMemory / totalMemory * 100;
    }

    private (double diskUsagePercent, double freeDiskSpaceGb) GetDiskUsage()
    {
        var root = Path.GetPathRoot(_agentSetting.DiskPath);

        if (string.IsNullOrWhiteSpace(root))
            root = "C:\\";

        var drive = new DriveInfo(root);

        if (!drive.IsReady)
            return (0, 0);

        var totalSpace = drive.TotalSize;
        var freeSpace = drive.AvailableFreeSpace;
        var usedSpace = totalSpace - freeSpace;

        var diskUsagePercent = (double)usedSpace / totalSpace * 100;
        var freeDiskSpaceGb = freeSpace / 1024.0 / 1024.0 / 1024.0;

        return (diskUsagePercent, freeDiskSpaceGb);
    }

    private int GetRecentSystemErrorCount()
    {
        try
        {
            if (!OperatingSystem.IsWindows())
                return 0;

            using var eventLog = new EventLog("System");

            var fromTime = DateTime.Now.AddMinutes(-10);

            return eventLog.Entries.Cast<EventLogEntry>()
                .Count(e =>
                    e.EntryType == EventLogEntryType.Error &&
                    e.TimeGenerated >= fromTime);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Не удалось получить количество ошибок из журнала Windows.");
            return _random.Next(0, 2);
        }
    }

    private double GetUptimeHours()
    {
        var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
        return uptime.TotalHours;
    }

    private double GenerateTemperature(double cpuUsage)
    {
        var baseTemperature = 35;
        var temperature = baseTemperature + cpuUsage * 0.45 + _random.NextDouble() * 5;

        return Math.Clamp(Math.Round(temperature, 2), 30, 100);
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MemoryStatusEx
    {
        public uint dwLength;

        public uint dwMemoryLoad;

        public ulong ullTotalPhys;

        public ulong ullAvailPhys;

        public ulong ullTotalPageFile;

        public ulong ullAvailPageFile;

        public ulong ullTotalVirtual;

        public ulong ullAvailVirtual;

        public ulong ullAvailExtendedVirtual;
    }
}