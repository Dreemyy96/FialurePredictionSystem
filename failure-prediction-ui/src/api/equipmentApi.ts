import api from "./axios";

export const getEquipment = async () => {

    const response =
        await api.get("/equipment");

    return response.data;
};

export const createEquipment = async (
    equipment: any
) => {

    const response = await api.post(
        "/equipment",
        equipment
    );

    return response.data;
};

export const getEquipmentById = async (
    id: string
) => {

    const response =
        await api.get(`/equipment/${id}`);

    return response.data;
};