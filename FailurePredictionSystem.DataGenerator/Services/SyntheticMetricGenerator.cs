using System;
using FailurePredictionSystem.DataGenerator.Enums;
using FailurePredictionSystem.DataGenerator.Models;

namespace FailurePredictionSystem.DataGenerator.Services;

public class SyntheticMetricGenerator
{
    private readonly GeneratorSettings _settings;
    private readonly Random _random = new();

    public SyntheticMetricGenerator(GeneratorSettings settings)
    {
        _settings = settings;
    }

    public MetricPayload Generate(DateTime timestampUtc, int index, int totalRecords)
    {
        var scenario = SelectScenario(index, totalRecords);

        return scenario switch
        {
            MetricScenario.Normal => GenerateNormal(timestampUtc, index),
            MetricScenario.HighLoad => GenerateHighLoad(timestampUtc, index),
            MetricScenario.Overheating => GenerateOverheating(timestampUtc, index),
            MetricScenario.DiskDegradation => GenerateDiskDegradation(timestampUtc, index, totalRecords),
            MetricScenario.PreFailure => GeneratePreFailure(timestampUtc, index),
            _ => GenerateNormal(timestampUtc, index)
        };
    }

    private MetricScenario SelectScenario(int index, int totalRecords)
    {
        var progress = (double)index / totalRecords;

        if (progress < 0.45)
            return MetricScenario.Normal;

        if (progress < 0.65)
            return MetricScenario.HighLoad;

        if (progress < 0.80)
            return MetricScenario.Overheating;

        if (progress < 0.93)
            return MetricScenario.DiskDegradation;

        return MetricScenario.PreFailure;
    }

    private MetricPayload GenerateNormal(DateTime timestampUtc, int index)
    {
        var cpu = RandomDouble(10, 55);
        var ram = RandomDouble(25, 65);
        var disk = RandomDouble(30, 72);
        var temp = RandomDouble(32, 62);
        var errors = RandomInt(0, 1);
        
        if (Chance(0.08))
        {
            cpu = RandomDouble(55, 72);
            ram = RandomDouble(55, 72);
            temp = RandomDouble(55, 68);
        }

        if (Chance(0.05))
        {
            errors = RandomInt(1, 2);
        }

        ApplyNoise(ref cpu, ref ram, ref disk, ref temp);

        return CreatePayload(
            timestampUtc,
            cpu,
            ram,
            disk,
            CalculateFreeDiskSpace(disk),
            temp,
            errorCount: errors,
            uptimeHours: index * 0.1,
            state: EquipmentState.Normal);
    }

    private MetricPayload GenerateHighLoad(DateTime timestampUtc, int index)
    {
        var cpu = RandomDouble(55, 88);
        var ram = RandomDouble(55, 84);
        var disk = RandomDouble(50, 82);
        var temp = RandomDouble(55, 78);
        var errors = RandomInt(0, 3);
        
        if (Chance(0.12))
        {
            cpu = RandomDouble(45, 62);
            ram = RandomDouble(45, 65);
            temp = RandomDouble(45, 62);
            errors = RandomInt(0, 1);
        }
        
        if (Chance(0.10))
        {
            cpu = RandomDouble(82, 94);
            ram = RandomDouble(75, 90);
            temp = RandomDouble(75, 86);
            errors = RandomInt(2, 5);
        }

        ApplyNoise(ref cpu, ref ram, ref disk, ref temp);

        return CreatePayload(
            timestampUtc,
            cpu,
            ram,
            disk,
            CalculateFreeDiskSpace(disk),
            temp,
            errorCount: errors,
            uptimeHours: index * 0.1,
            state: EquipmentState.Warning);
    }

    private MetricPayload GenerateOverheating(DateTime timestampUtc, int index)
    {
        var cpu = RandomDouble(60, 95);
        var ram = RandomDouble(50, 88);
        var disk = RandomDouble(50, 85);

        var temp = RandomDouble(72, 94);
        var errors = RandomInt(1, 5);
        
        var state = temp >= 88 || errors >= 4
            ? EquipmentState.Critical
            : EquipmentState.Warning;
        
        if (Chance(0.15))
        {
            temp = RandomDouble(78, 86);
            errors = RandomInt(0, 2);
            state = EquipmentState.Warning;
        }
        
        if (Chance(0.10))
        {
            cpu = RandomDouble(45, 70);
            ram = RandomDouble(45, 70);
            temp = RandomDouble(90, 100);
            errors = RandomInt(1, 4);
            state = EquipmentState.Critical;
        }

        ApplyNoise(ref cpu, ref ram, ref disk, ref temp);

        return CreatePayload(
            timestampUtc,
            cpu,
            ram,
            disk,
            CalculateFreeDiskSpace(disk),
            temp,
            errorCount: errors,
            uptimeHours: index * 0.1,
            state: state);
    }

    private MetricPayload GenerateDiskDegradation(DateTime timestampUtc, int index, int totalRecords)
    {
        var localProgress = (double)index / totalRecords;

        var disk = 70 + localProgress * 28 + RandomDouble(-5, 5);
        disk = Math.Clamp(disk, 65, 99);

        var cpu = RandomDouble(30, 78);
        var ram = RandomDouble(40, 82);
        var temp = RandomDouble(45, 78);

        var errors = (int)(localProgress * 12) + RandomInt(0, 4);

        var state = disk >= 92 || errors >= 10
            ? EquipmentState.Critical
            : EquipmentState.Warning;
        
        if (Chance(0.12))
        {
            disk = RandomDouble(85, 94);
            errors = RandomInt(0, 3);
            state = EquipmentState.Warning;
        }
        
        if (Chance(0.10))
        {
            disk = RandomDouble(70, 88);
            errors = RandomInt(10, 18);
            state = EquipmentState.Critical;
        }

        ApplyNoise(ref cpu, ref ram, ref disk, ref temp);

        return CreatePayload(
            timestampUtc,
            cpu,
            ram,
            disk,
            CalculateFreeDiskSpace(disk),
            temp,
            errorCount: errors,
            uptimeHours: index * 0.1,
            state: state);
    }

    private MetricPayload GeneratePreFailure(DateTime timestampUtc, int index)
    {
        var cpu = RandomDouble(75, 100);
        var ram = RandomDouble(70, 98);
        var disk = RandomDouble(75, 99);
        var temp = RandomDouble(75, 100);
        var errors = RandomInt(4, 20);
        
        if (Chance(0.25))
        {
            cpu = RandomDouble(45, 75);
            ram = RandomDouble(45, 75);
            disk = RandomDouble(50, 80);
            temp = RandomDouble(90, 100);
            errors = RandomInt(2, 8);
        }

        if (Chance(0.25))
        {
            cpu = RandomDouble(40, 75);
            ram = RandomDouble(40, 80);
            disk = RandomDouble(50, 85);
            temp = RandomDouble(50, 75);
            errors = RandomInt(12, 25);
        }

        if (Chance(0.25))
        {
            cpu = RandomDouble(35, 70);
            ram = RandomDouble(40, 75);
            disk = RandomDouble(94, 99);
            temp = RandomDouble(45, 75);
            errors = RandomInt(3, 12);
        }

        if (Chance(0.10))
        {
            cpu = RandomDouble(70, 85);
            ram = RandomDouble(65, 82);
            disk = RandomDouble(80, 92);
            temp = RandomDouble(78, 88);
            errors = RandomInt(4, 8);
        }

        ApplyNoise(ref cpu, ref ram, ref disk, ref temp);

        return CreatePayload(
            timestampUtc,
            cpu,
            ram,
            disk,
            CalculateFreeDiskSpace(disk),
            temp,
            errorCount: errors,
            uptimeHours: index * 0.1,
            state: EquipmentState.Critical);
    }

    private MetricPayload CreatePayload(
        DateTime timestampUtc,
        double cpu,
        double ram,
        double disk,
        double freeDiskSpaceGb,
        double temperature,
        int errorCount,
        double uptimeHours,
        EquipmentState state)
    {
        cpu = ClampPercent(cpu);
        ram = ClampPercent(ram);
        disk = ClampPercent(disk);
        temperature = Math.Clamp(temperature, 20, 110);

        return new MetricPayload
        {
            AgentId = _settings.AgentId,
            EquipmentId = _settings.EquipmentId,
            Hostname = _settings.Hostname,
            TimestampUtc = timestampUtc,

            CpuUsagePercent = Math.Round(cpu, 2),
            RamUsagePercent = Math.Round(ram, 2),
            DiskUsagePercent = Math.Round(disk, 2),
            FreeDiskSpaceGb = Math.Round(freeDiskSpaceGb, 2),
            TemperatureCelsius = Math.Round(temperature, 2),
            ErrorCount = Math.Max(errorCount, 0),
            UptimeHours = Math.Round(uptimeHours, 2),

            State = _settings.IncludeState ? state : null
        };
    }

    private double CalculateFreeDiskSpace(double diskUsagePercent)
    {
        const double totalDiskGb = 500;

        var usedGb = totalDiskGb * diskUsagePercent / 100;
        var freeGb = totalDiskGb - usedGb;

        return Math.Max(freeGb, 0);
    }

    private void ApplyNoise(ref double cpu, ref double ram, ref double disk, ref double temp)
    {
        cpu += RandomDouble(-4, 4);
        ram += RandomDouble(-4, 4);
        disk += RandomDouble(-3, 3);
        temp += RandomDouble(-3, 3);
    }

    private double ClampPercent(double value)
    {
        return Math.Clamp(value, 0, 100);
    }

    private bool Chance(double probability)
    {
        return _random.NextDouble() < probability;
    }

    private double RandomDouble(double min, double max)
    {
        return min + _random.NextDouble() * (max - min);
    }

    private int RandomInt(int min, int max)
    {
        return _random.Next(min, max + 1);
    }
}