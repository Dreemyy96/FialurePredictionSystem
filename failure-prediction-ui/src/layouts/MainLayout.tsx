import {
    AppBar,
    Toolbar,
    Typography,
    Button,
    Box,
    Badge
} from "@mui/material";

import {
    Outlet,
    useNavigate
} from "react-router-dom";

import {
    useEffect,
    useState
} from "react";

import ComputerIcon
    from "@mui/icons-material/Computer";

import NotificationsIcon
    from "@mui/icons-material/Notifications";

import LogoutIcon
    from "@mui/icons-material/Logout";

import SecurityIcon
    from "@mui/icons-material/Security";

import { logout }
    from "../services/authService";

import {
    getUnreadNotifications
} from "../api/notificationApi";

export default function MainLayout() {

    const navigate = useNavigate();

    const [unreadCount, setUnreadCount] =
        useState(0);

    useEffect(() => {

        loadUnreadNotifications();

    }, []);

    const loadUnreadNotifications =
        async () => {

            try {

                const result =
                    await getUnreadNotifications();

                const inAppUnread =
                    result.filter(
                        (x: any) =>
                            x.channelCode === 1
                    );

                setUnreadCount(
                    inAppUnread.length
                );

            } catch (error) {

                console.error(
                    "Failed to load notifications",
                    error
                );
            }
        };

    const handleLogout = () => {

        logout();

        navigate("/");
    };

    return (

        <>

            <AppBar position="static"
                sx={{
                    backgroundColor: "#2563EB",
                    boxShadow: 2
            }}>

                <Toolbar>

                    <SecurityIcon
                        sx={{
                            mr: 1
                        }}
                    />

                    <Typography
                        variant="h6"
                        sx={{
                            flexGrow: 1,
                            fontWeight: 600
                        }}
                    >
                        Failure Prediction System
                    </Typography>

                    <Button
                        color="inherit"
                        startIcon={
                            <ComputerIcon />
                        }
                        onClick={() =>
                            navigate("/equipment")
                        }
                    >
                        Оборудование
                    </Button>

                    <Button
                        color="inherit"
                        startIcon={
                            <NotificationsIcon />
                        }
                        onClick={() =>
                            navigate("/notifications")
                        }
                    >

                        <Badge
                            color="error"
                            badgeContent={
                                unreadCount > 0
                                    ? unreadCount
                                    : null
                            }
                        >
                            Уведомления
                        </Badge>

                    </Button>

                    <Button
                        color="inherit"
                        startIcon={
                            <LogoutIcon />
                        }
                        onClick={handleLogout}
                    >
                        Выход
                    </Button>

                </Toolbar>

            </AppBar>

            <Box
                sx={{
                    p: 4,
                    backgroundColor: "#F8FAFC",
                    minHeight: "100vh"
                }}
            >
                <Outlet />
            </Box>

        </>

    );
}