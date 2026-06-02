export interface Equipment {
    id: string;
    agentId: string;
    name: string;
    hostname: string;
    type: number;
    location: string;
    isActive: boolean;
    createdAtUtc: string;
}