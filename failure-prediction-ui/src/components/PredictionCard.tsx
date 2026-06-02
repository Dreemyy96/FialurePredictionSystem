import {
    Card,
    CardContent,
    Typography,
    Chip,
    LinearProgress,
    Box
} from "@mui/material";

import type {
    Prediction
} from "../types/prediction";

import {
    getEquipmentStateName,
    getEquipmentStateColor
} from "../utils/equipmentState";

type Props = {
    prediction: Prediction | null;
};

export default function PredictionCard({
    prediction
}: Props) {

    if (!prediction) {

        return (
            <Card sx={{ mt: 3 }}>

                <CardContent>

                    <Typography variant="h6">
                        Прогноз состояния
                    </Typography>

                    <Typography>
                        Прогноз отсутствует
                    </Typography>

                </CardContent>

            </Card>
        );
    }

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
                    Прогноз состояния оборудования
                </Typography>

                <Chip
                    label={
                        getEquipmentStateName(
                            prediction.predictedState
                        )
                    }
                    color={
                        getEquipmentStateColor(
                            prediction.predictedState
                        ) as any
                    }
                    sx={{
                        mb: 3,
                        fontSize: 16,
                        height: 40
                    }}
                />

                <Box sx={{ mb: 3 }}>

                    <Typography>
                        Normal
                        {" "}
                        ({(
                            prediction.normalProbability * 100
                        ).toFixed(1)}%)
                    </Typography>

                    <LinearProgress
                        variant="determinate"
                        value={
                            prediction.normalProbability * 100
                        }
                        color="success"
                        sx={{
                            mt: 1,
                            height: 10,
                            borderRadius: 5
                        }}
                    />

                </Box>

                <Box sx={{ mb: 3 }}>

                    <Typography>
                        Warning
                        {" "}
                        ({(
                            prediction.warningProbability * 100
                        ).toFixed(1)}%)
                    </Typography>

                    <LinearProgress
                        variant="determinate"
                        value={
                            prediction.warningProbability * 100
                        }
                        color="warning"
                        sx={{
                            mt: 1,
                            height: 10,
                            borderRadius: 5
                        }}
                    />

                </Box>

                <Box>

                    <Typography>
                        Critical
                        {" "}
                        ({(
                            prediction.criticalProbability * 100
                        ).toFixed(1)}%)
                    </Typography>

                    <LinearProgress
                        variant="determinate"
                        value={
                            prediction.criticalProbability * 100
                        }
                        color="error"
                        sx={{
                            mt: 1,
                            height: 10,
                            borderRadius: 5
                        }}
                    />

                </Box>

                <Typography
                    variant="body2"
                    sx={{
                        mt: 3,
                        opacity: 0.7
                    }}
                >
                    Создан:
                    {" "}
                    {new Date(
                        prediction.createdAtUtc
                    ).toLocaleString()}
                </Typography>

            </CardContent>

        </Card>
    );
}