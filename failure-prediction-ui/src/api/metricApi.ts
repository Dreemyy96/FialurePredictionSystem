import api from "./axios";

export const getMetrics = async (
    equipmentId: string
) => {

    const response =
        await api.get(
            `/metric/equipment/${equipmentId}`
        );

    return response.data;
};