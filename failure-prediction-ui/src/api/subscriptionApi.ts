import api from "./axios";

export const getMySubscriptions =
    async () => {

        const response =
            await api.get(
                "/equipment-notification-subscriptions/my"
            );

        return response.data;
    };

export const subscribeToEquipment =
    async (
        equipmentId: string,
        isInAppEnabled: boolean,
        isEmailEnabled: boolean
    ) => {

        const response =
            await api.post(
                `/equipment-notification-subscriptions/${equipmentId}`,
                {
                    isInAppEnabled,
                    isEmailEnabled
                }
            );

        return response.data;
    };

export const unsubscribeFromEquipment =
    async (
        equipmentId: string
    ) => {

        await api.delete(
            `/equipment-notification-subscriptions/${equipmentId}`
        );
    };