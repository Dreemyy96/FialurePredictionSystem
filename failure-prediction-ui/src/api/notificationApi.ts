import api from "./axios";

export const getNotifications = async () => {

    const response =
        await api.get(
            "/notifications/my"
        );

    return response.data;
};

export const markNotificationAsRead =
    async (
        notificationId: string
    ) => {

        await api.post(
            `/notifications/${notificationId}/read`
        );
    };

export const getUnreadNotifications =
    async () => {

    const response =
        await api.get(
            "/notifications/my?isRead=false"
        );

    return response.data;
};