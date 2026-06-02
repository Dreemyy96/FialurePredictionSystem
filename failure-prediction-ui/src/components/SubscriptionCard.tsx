import {
    Card,
    CardContent,
    Typography,
    Checkbox,
    FormControlLabel,
    Button,
    Box,
    Divider,
    Chip
} from "@mui/material";

import {
    useEffect,
    useState
} from "react";

type Props = {
    initialInApp: boolean;
    initialEmail: boolean;

    onSave: (
        inApp: boolean,
        email: boolean
    ) => Promise<void>;
};

export default function SubscriptionCard({
    initialInApp,
    initialEmail,
    onSave
}: Props) {

    const [inApp, setInApp] =
        useState(initialInApp);

    const [email, setEmail] =
        useState(initialEmail);

    useEffect(() => {

        setInApp(initialInApp);
        setEmail(initialEmail);

    }, [
        initialInApp,
        initialEmail
    ]);

    return (

        <Card
            elevation={2}
            sx={{
                mt: 3,
                mb: 3,
                borderRadius: 3
            }}
        >

            <CardContent>

                <Box
                    sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        mb: 2
                    }}
                >

                    <Typography
                        variant="h6"
                    >
                        Настройки уведомлений
                    </Typography>

                    <Chip
                        color={
                            inApp || email
                                ? "success"
                                : "default"
                        }
                        label={
                            inApp || email
                                ? "Подписка активна"
                                : "Подписка отключена"
                        }
                    />

                </Box>

                <Divider sx={{ mb: 2 }} />

                <Typography
                    variant="body2"
                    sx={{
                        mb: 2,
                        opacity: 0.8
                    }}
                >
                    Выберите каналы получения уведомлений
                    о состоянии оборудования и прогнозируемых отказах.
                </Typography>

                <FormControlLabel
                    control={
                        <Checkbox
                            checked={inApp}
                            onChange={(e) =>
                                setInApp(
                                    e.target.checked
                                )
                            }
                        />
                    }
                    label="Получать уведомления в системе"
                />

                <FormControlLabel
                    control={
                        <Checkbox
                            checked={email}
                            onChange={(e) =>
                                setEmail(
                                    e.target.checked
                                )
                            }
                        />
                    }
                    label="Получать уведомления по Email"
                />

                <Box
                    sx={{
                        mt: 3
                    }}
                >

                    <Button
                        variant="contained"
                        onClick={() =>
                            onSave(
                                inApp,
                                email
                            )
                        }
                    >
                        Сохранить настройки
                    </Button>

                </Box>

            </CardContent>

        </Card>

    );
}