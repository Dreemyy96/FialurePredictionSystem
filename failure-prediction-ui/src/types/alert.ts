export interface Alert {
    id: string;

    equipmentId: string;
    predictionId: string;

    severity: number;
    severityCode: number;
    severityName: string;

    title: string;
    message: string;

    isResolved: boolean;

    createdAtUtc: string;
    resolvedAtUtc?: string;
}