import {
    Card,
    CardContent,
    Typography,
    TextField,
    Button,
    Tabs,
    Tab,
    Box
} from "@mui/material";

import SecurityIcon
    from "@mui/icons-material/Security";

import {
    useState
} from "react";

import {
    useNavigate
} from "react-router-dom";

import {
    login,
    register
} from "../api/authApi";

import {
    saveAuth
} from "../services/authService";

export default function LoginPage() {

    const navigate = useNavigate();

    const [tab, setTab] =
        useState(0);

    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [fullName, setFullName] =
        useState("");

    const handleLogin = async () => {

        const result =
            await login(
                email,
                password
            );

        saveAuth(result);

        navigate("/equipment");
    };

    const handleRegister =
        async () => {

            await register(
                email,
                password,
                fullName
            );

            const result =
                await login(
                    email,
                    password
                );

            saveAuth(result);

            navigate("/equipment");
        };

    return (

        <Box
            sx={{
                minHeight: "100vh",

                display: "flex",

                justifyContent: "center",

                alignItems: "center",

                backgroundImage:
                    "url('/images/datacenter.jpg')",

                backgroundSize: "cover",

                backgroundPosition: "center",

                position: "relative",

                "&::before": {
                    content: '""',

                    position: "absolute",

                    inset: 0,

                    backdropFilter:
                        "blur(8px)",

                    backgroundColor:
                        "rgba(15,23,42,0.65)"
                }
            }}
        >

            <Card
                elevation={10}
                sx={{
                    width: 500,

                    position: "relative",

                    zIndex: 1,

                    borderRadius: 4,

                    backgroundColor:
                        "rgba(255,255,255,0.95)"
                }}
            >

                <CardContent
                    sx={{
                        p: 4
                    }}
                >

                    <Box
                        sx={{
                            display: "flex",

                            flexDirection:
                                "column",

                            alignItems:
                                "center",

                            mb: 2
                        }}
                    >

                        <SecurityIcon
                            sx={{
                                fontSize: 70,

                                color:
                                    "#2563EB",

                                mb: 1
                            }}
                        />

                        <Typography
                            variant="h3"
                            sx={{
                                fontWeight: 600
                            }}
                        >
                            Predictive
                        </Typography>

                        <Typography
                            variant="h4"
                            sx={{
                                mb: 1
                            }}
                        >
                            Maintenance
                        </Typography>

                        <Typography
                            color="text.secondary"
                        >
                            Equipment Monitoring System
                        </Typography>

                    </Box>

                    <Tabs
                        value={tab}
                        onChange={(
                            _,
                            value
                        ) =>
                            setTab(value)
                        }
                        centered
                        sx={{
                            mb: 3
                        }}
                    >

                        <Tab
                            label="Вход"
                        />

                        <Tab
                            label="Регистрация"
                        />

                    </Tabs>

                    {tab === 1 && (

                        <TextField
                            label="ФИО"
                            fullWidth
                            margin="normal"
                            value={fullName}
                            onChange={e =>
                                setFullName(
                                    e.target.value
                                )
                            }
                        />

                    )}

                    <TextField
                        label="Email"
                        fullWidth
                        margin="normal"
                        value={email}
                        onChange={e =>
                            setEmail(
                                e.target.value
                            )
                        }
                    />

                    <TextField
                        label="Пароль"
                        type="password"
                        fullWidth
                        margin="normal"
                        value={password}
                        onChange={e =>
                            setPassword(
                                e.target.value
                            )
                        }
                    />

                    {tab === 0 ? (

                        <Button
                            fullWidth
                            size="large"
                            variant="contained"
                            sx={{
                                mt: 3,
                                height: 50
                            }}
                            onClick={
                                handleLogin
                            }
                        >
                            Войти
                        </Button>

                    ) : (

                        <Button
                            fullWidth
                            size="large"
                            variant="contained"
                            sx={{
                                mt: 3,
                                height: 50
                            }}
                            onClick={
                                handleRegister
                            }
                        >
                            Зарегистрироваться
                        </Button>

                    )}

                </CardContent>

            </Card>

        </Box>
    );
}