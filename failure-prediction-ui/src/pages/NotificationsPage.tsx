import {
    Card,
    CardContent,
    Typography,
    Button,
    Chip,
    Container,
    Box
} from "@mui/material";

import {
    useEffect,
    useState
} from "react";

import NotificationsIcon
    from "@mui/icons-material/Notifications";

import DoneIcon
    from "@mui/icons-material/Done";

import {
    getNotifications,
    markNotificationAsRead
} from "../api/notificationApi";

import type {
    Notification
} from "../types/notification";

export default function NotificationsPage() {

    const [notifications, setNotifications] =
        useState<Notification[]>([]);

    useEffect(() => {

        loadNotifications();

    }, []);

    const loadNotifications = async () => {

        const result =
            await getNotifications();

        const inAppNotifications =
            result.filter(
                (x: Notification) =>
                    x.channelCode === 1
            );

        setNotifications(
            inAppNotifications
        );
    };

    const markAsRead = async (
        id: string
    ) => {

        await markNotificationAsRead(id);

        await loadNotifications();
    };

    const unreadCount =
        notifications.filter(
            x => !x.isRead
        ).length;

    return (

        <Container maxWidth="xl">

            <Box
                sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: 2,
                    mb: 3
                }}
            >

                <NotificationsIcon
                    fontSize="large"
                />

                <Typography
                    variant="h4"
                >
                    Уведомления
                </Typography>

                <Chip
                    color="error"
                    label={
                        `${unreadCount} непрочитанных`
                    }
                />

            </Box>

            {notifications.length === 0 && (

                <Typography>
                    Уведомления отсутствуют
                </Typography>

            )}

            {notifications.map(notification => (

                <Card
                    elevation={2}
                    key={notification.id}
                    sx={{
                        mb: 2,
                        borderRadius: 3,
                        borderLeft:
                            notification.isRead
                                ? "4px solid #9e9e9e"
                                : "4px solid #1976d2"
                    }}
                >

                    <CardContent>

                        <Box
                            sx={{
                                display: "flex",
                                justifyContent:
                                    "space-between",
                                alignItems:
                                    "center",
                                mb: 1
                            }}
                        >

                            <Typography
                                variant="h6"
                            >
                                {notification.subject}
                            </Typography>

                            <Chip
                                color={
                                    notification.isRead
                                        ? "default"
                                        : "primary"
                                }
                                label={
                                    notification.isRead
                                        ? "Прочитано"
                                        : "Новое"
                                }
                            />

                        </Box>

                        <Typography
                            sx={{ mb: 2 }}
                        >
                            {notification.message}
                        </Typography>

                        <Typography
                            variant="body2"
                            sx={{
                                opacity: 0.7
                            }}
                        >
                            Создано:
                            {" "}
                            {new Date(
                                notification.createdAtUtc
                            ).toLocaleString()}
                        </Typography>

                        <Typography
                            variant="body2"
                            sx={{
                                opacity: 0.7,
                                mb: 2
                            }}
                        >
                            Статус:
                            {" "}
                            {notification.statusName}
                        </Typography>

                        {!notification.isRead && (

                            <Button
                                variant="contained"
                                startIcon={
                                    <DoneIcon />
                                }
                                onClick={() =>
                                    markAsRead(
                                        notification.id
                                    )
                                }
                            >
                                Отметить как прочитанное
                            </Button>

                        )}

                    </CardContent>

                </Card>

            ))}

        </Container>
    );
}