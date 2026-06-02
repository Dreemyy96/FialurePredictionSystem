import api from "./axios";

export const getLatestPrediction = async (
    equipmentId: string
) => {

    const response =
        await api.get(
            `/equipment/${equipmentId}/latest-prediction`
        );

    return response.data;
};