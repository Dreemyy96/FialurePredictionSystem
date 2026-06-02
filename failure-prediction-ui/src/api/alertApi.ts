import api from "./axios";

export const getAlerts = async (
    equipmentId: string
) => {

    const response =
        await api.get(
            `/alerts/equipment/${equipmentId}`
        );

    return response.data;
};