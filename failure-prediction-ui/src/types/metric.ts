export interface Metric {
    id: string;

    timestampUtc: string;

    cpuUsagePercent: number;
    ramUsagePercent: number;

    diskUsagePercent: number;
    freeDiskSpaceGb: number;

    temperatureCelsius: number;

    errorCount: number;
    uptimeHours: number;

    state: number | null;

    predictionStatus: number;
}