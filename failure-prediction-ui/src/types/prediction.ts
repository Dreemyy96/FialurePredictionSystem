export interface Prediction {
    id: string;
    equipmentId: string;
    metricId: string;

    predictedState: number;
    predictedStateCode: number;

    normalProbability: number;
    warningProbability: number;
    criticalProbability: number;

    createdAtUtc: string;
}