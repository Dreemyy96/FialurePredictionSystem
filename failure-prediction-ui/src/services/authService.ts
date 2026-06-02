import type { AuthResponse } from "../types/auth";

export const saveAuth = (
    auth: AuthResponse
) => {

    localStorage.setItem(
        "token",
        auth.token
    );

    localStorage.setItem(
        "user",
        JSON.stringify(auth)
    );
};

export const logout = () => {

    localStorage.removeItem("token");
    localStorage.removeItem("user");
};

export const isAuthenticated = () => {
    return !!localStorage.getItem("token");
};