import {
    Card,
    CardContent,
    Typography,
    LinearProgress,
    Box,
    Grid
} from "@mui/material";

import type {
    Metric
} from "../types/metric";

type Props = {
    metrics: Metric[];
};

const getProgressColor = (
    value: number
) => {

    if (value >= 85)
        return "error";

    if (value >= 60)
        return "warning";

    return "success";
};

export default function MetricsCard({
    metrics
}: Props) {

    if (metrics.length === 0) {

        return (
            <Card sx={{ mt: 3 }}>
                <CardContent>

                    <Typography variant="h6">
                        Метрики
                    </Typography>

                    <Typography>
                        Метрики отсутствуют
                    </Typography>

                </CardContent>
            </Card>
        );
    }

    const latestMetric = metrics[0];

    return (

        <Card 
            elevation={2}
            sx={{ mt: 3, borderRadius: 3 }}
        >

            <CardContent>

                <Typography
                    variant="h6"
                    sx={{ mb: 3 }}
                >
                    Последние метрики
                </Typography>

                <Grid container spacing={4}>

                    <Grid size={12}>

                        <Typography>
                            CPU Usage
                            {" "}
                            ({latestMetric.cpuUsagePercent.toFixed(1)}%)
                        </Typography>

                        <LinearProgress
                            variant="determinate"
                            value={latestMetric.cpuUsagePercent}
                            color={
                                getProgressColor(
                                    latestMetric.cpuUsagePercent
                                )
                            }
                            sx={{
                                mt: 1,
                                height: 10,
                                borderRadius: 5
                            }}
                        />

                    </Grid>

                    <Grid size={12}>

                        <Typography>
                            RAM Usage
                            {" "}
                            ({latestMetric.ramUsagePercent.toFixed(1)}%)
                        </Typography>

                        <LinearProgress
                            variant="determinate"
                            value={latestMetric.ramUsagePercent}
                            color={
                                getProgressColor(
                                    latestMetric.ramUsagePercent
                                )
                            }
                            sx={{
                                mt: 1,
                                height: 10,
                                borderRadius: 5
                            }}
                        />

                    </Grid>

                    <Grid size={12}>

                        <Typography>
                            Disk Usage
                            {" "}
                            ({latestMetric.diskUsagePercent.toFixed(1)}%)
                        </Typography>

                        <LinearProgress
                            variant="determinate"
                            value={latestMetric.diskUsagePercent}
                            color={
                                getProgressColor(
                                    latestMetric.diskUsagePercent
                                )
                            }
                            sx={{
                                mt: 1,
                                height: 10,
                                borderRadius: 5
                            }}
                        />

                    </Grid>

                </Grid>

                <Box
                    sx={{
                        mt: 4,
                        display: "flex",
                        flexWrap: "wrap",
                        gap: 3
                    }}
                >

                    <Typography>
                        Free Disk:
                        {" "}
                        {latestMetric.freeDiskSpaceGb.toFixed(2)}
                        {" "}
                        GB
                    </Typography>

                    <Typography>
                        Temperature:
                        {" "}
                        {latestMetric.temperatureCelsius.toFixed(1)}
                        {" "}
                        °C
                    </Typography>

                    <Typography>
                        Errors:
                        {" "}
                        {latestMetric.errorCount}
                    </Typography>

                    <Typography>
                        Uptime:
                        {" "}
                        {latestMetric.uptimeHours.toFixed(1)}
                        {" "}
                        h
                    </Typography>

                </Box>

            </CardContent>

        </Card>
    );
}