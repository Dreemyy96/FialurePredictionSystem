import {
    Card,
    CardContent,
    Chip,
    Container,
    Typography,
    CircularProgress,
    Box
} from "@mui/material";

import {
    useEffect,
    useState
} from "react";

import {
    useParams
} from "react-router-dom";

import {
    getEquipmentById
} from "../api/equipmentApi";

import {
    getLatestPrediction
} from "../api/predictionApi";

import {
    getMetrics
} from "../api/metricApi";

import {
    getAlerts
} from "../api/alertApi";

import {
    getMySubscriptions,
    subscribeToEquipment,
    unsubscribeFromEquipment
} from "../api/subscriptionApi";

import type {
    Equipment
} from "../types/equipment";

import type {
    Prediction
} from "../types/prediction";

import type {
    Metric
} from "../types/metric";

import type {
    Alert
} from "../types/alert";

import type {
    EquipmentSubscription
} from "../types/subscription";

import {
    getEquipmentTypeName
} from "../utils/equipmentType";

import {
    getEquipmentIcon
} from "../utils/equipmentIcon";

import {
    getEquipmentStateName,
    getEquipmentStateColor
} from "../utils/equipmentState";

import PredictionCard
    from "../components/PredictionCard";

import MetricsCard
    from "../components/MetricsCard";

import AlertsCard
    from "../components/AlertsCard";

import SubscriptionCard
    from "../components/SubscriptionCard";

export default function EquipmentDetailsPage() {

    const { id } = useParams();

    const [equipment, setEquipment] =
        useState<Equipment | null>(null);

    const [prediction, setPrediction] =
        useState<Prediction | null>(null);

    const [metrics, setMetrics] =
        useState<Metric[]>([]);

    const [alerts, setAlerts] =
        useState<Alert[]>([]);

    const [subscription, setSubscription] =
        useState<EquipmentSubscription | null>(
            null
        );

    const [loading, setLoading] =
        useState(true);

    useEffect(() => {

        loadEquipment();

    }, []);

    const loadEquipment = async () => {

        if (!id) return;

        try {

            const equipmentResult =
                await getEquipmentById(id);

            setEquipment(equipmentResult);

            const predictionResult =
                await getLatestPrediction(id);

            setPrediction(predictionResult);

            const metricResult =
                await getMetrics(id);

            setMetrics(metricResult);

            const alertResult =
                await getAlerts(id);

            setAlerts(alertResult);

            const subscriptions =
                await getMySubscriptions();

            const currentSubscription =
                subscriptions.find(
                    (x: EquipmentSubscription) =>
                        x.equipmentId === id
                );

            setSubscription(
                currentSubscription ?? null
            );

        } finally {

            setLoading(false);
        }
    };

    const saveSubscription =
        async (
            inApp: boolean,
            email: boolean
        ) => {

            if (!id) return;

            if (!inApp && !email) {

                await unsubscribeFromEquipment(
                    id
                );

                setSubscription(null);

                return;
            }

            const result =
                await subscribeToEquipment(
                    id,
                    inApp,
                    email
                );

            setSubscription(result);
        };

    if (loading) {

        return (
            <Container>
                <CircularProgress />
            </Container>
        );
    }

    if (!equipment) {

        return (
            <Container>

                <Typography>
                    Equipment not found
                </Typography>

            </Container>
        );
    }

    return (

        <Container maxWidth="xl">

            <Card
                elevation={2}
                sx={{
                    mb: 3,
                    borderRadius: 3
                }}
            >

                <CardContent>

                    <Box
                        sx={{
                            display: "flex",
                            gap: 4,
                            alignItems: "center"
                        }}
                    >

                        <Box>

                            {getEquipmentIcon(
                                equipment.type
                            )}

                        </Box>

                        <Box
                            sx={{
                                flexGrow: 1
                            }}
                        >

                            <Typography
                                variant="h4"
                            >
                                {equipment.name}
                            </Typography>

                            <Typography
                                variant="h6"
                                color="text.secondary"
                            >
                                {getEquipmentTypeName(
                                    equipment.type
                                )}
                            </Typography>

                            <Typography
                                sx={{ mt: 2 }}
                            >
                                Hostname:
                                {" "}
                                {equipment.hostname}
                            </Typography>

                            <Typography>
                                Location:
                                {" "}
                                {equipment.location}
                            </Typography>

                            <Typography>
                                Agent Id:
                                {" "}
                                {equipment.agentId}
                            </Typography>

                            <Typography>
                                Created:
                                {" "}
                                {new Date(
                                    equipment.createdAtUtc
                                ).toLocaleString()}
                            </Typography>

                        </Box>

                        <Box
                            sx={{
                                display: "flex",
                                flexDirection: "column",
                                alignItems: "flex-end",
                                gap: 2
                            }}
                        >

                            <Chip
                                color={
                                    equipment.isActive
                                        ? "success"
                                        : "default"
                                }
                                label={
                                    equipment.isActive
                                        ? "Active"
                                        : "Inactive"
                                }
                                sx={{
                                    fontSize: 16,
                                    height: 40
                                }}
                            />

                            {prediction && (

                                <Chip
                                    color={
                                        getEquipmentStateColor(
                                            prediction.predictedState
                                        ) as any
                                    }
                                    label={
                                        `Prediction: ${
                                            getEquipmentStateName(
                                                prediction.predictedState
                                            )
                                        }`
                                    }
                                    sx={{
                                        fontSize: 16,
                                        height: 40
                                    }}
                                />

                            )}

                        </Box>

                    </Box>

                </CardContent>

            </Card>

            <PredictionCard
                prediction={prediction}
            />

            <MetricsCard
                metrics={metrics}
            />

            <AlertsCard
                alerts={alerts}
            />

            <SubscriptionCard
                initialInApp={
                    subscription?.isInAppEnabled
                    ?? false
                }
                initialEmail={
                    subscription?.isEmailEnabled
                    ?? false
                }
                onSave={saveSubscription}
            />

        </Container>

    );
}