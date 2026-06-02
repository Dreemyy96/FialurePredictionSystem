import api from "./axios";

export const login = async (
    email: string,
    password: string
) => {

    const response = await api.post(
        "/auth/login",
        {
            email,
            password
        });

    return response.data;
};

export const register = async (
    email: string,
    password: string,
    fullName: string
) => {

    const response =
        await api.post(
            "/auth/register",
            {
                email,
                password,
                fullName,
                role: 2
            }
        );

    return response.data;
};