export interface EquipmentSubscription {
    id: string;

    equipmentId: string;

    userId: string;

    isInAppEnabled: boolean;

    isEmailEnabled: boolean;

    createdAtUtc: string;
}