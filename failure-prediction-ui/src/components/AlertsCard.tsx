import {
    Card,
    CardContent,
    Typography,
    Chip,
    Divider,
    Box
} from "@mui/material";

import type {
    Alert
} from "../types/alert";

type Props = {
    alerts: Alert[];
};

const getSeverityColor = (
    severity: string
) => {

    switch (
        severity.toLowerCase()
    ) {

        case "critical":
            return "error";

        case "warning":
            return "warning";

        case "info":
            return "info";

        default:
            return "default";
    }
};

export default function AlertsCard({
    alerts
}: Props) {

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

                <Typography
                    variant="h6"
                    sx={{ mb: 3 }}
                >
                    Активные предупреждения
                </Typography>

                {alerts.length === 0 && (

                    <Typography>
                        Активные предупреждения отсутствуют
                    </Typography>

                )}

                {alerts.length > 0 && (

                    <Box
                        sx={{
                            maxHeight: 350,
                            overflowY: "auto",
                            pr: 1
                        }}
                    >

                        {alerts.map(alert => (

                            <Box
                                key={alert.id}
                                sx={{
                                    mb: 2
                                }}
                            >

                                <Box
                                    sx={{
                                        display: "flex",
                                        justifyContent: "space-between",
                                        alignItems: "center",
                                        mb: 1
                                    }}
                                >

                                    <Typography
                                        variant="subtitle1"
                                        sx={{
                                            fontWeight: "bold"
                                        }}
                                    >
                                        {alert.title}
                                    </Typography>

                                    <Chip
                                        color={
                                            getSeverityColor(
                                                alert.severityName
                                            ) as any
                                        }
                                        label={
                                            alert.severityName
                                        }
                                    />

                                </Box>

                                <Typography>
                                    {alert.message}
                                </Typography>

                                <Typography
                                    variant="body2"
                                    sx={{
                                        mt: 1,
                                        opacity: 0.7
                                    }}
                                >
                                    {new Date(
                                        alert.createdAtUtc
                                    ).toLocaleString()}
                                </Typography>

                                <Divider
                                    sx={{
                                        mt: 2
                                    }}
                                />

                            </Box>

                        ))}

                    </Box>

                )}

            </CardContent>

        </Card>
    );
}