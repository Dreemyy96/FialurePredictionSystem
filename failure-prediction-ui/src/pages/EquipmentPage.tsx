import {
    Container,
    Typography,
    Button,
    Box,
    Card,
    CardContent,
    TextField
} from "@mui/material";

import {
    useEffect,
    useMemo,
    useState
} from "react";

import {
    useNavigate
} from "react-router-dom";

import {
    getEquipment
} from "../api/equipmentApi";

import type {
    Equipment
} from "../types/equipment";

import EquipmentCard
    from "../components/EquipmentCard";

export default function EquipmentPage() {

    const navigate = useNavigate();

    const [equipment, setEquipment] =
        useState<Equipment[]>([]);

    const [search, setSearch] =
        useState("");

    useEffect(() => {

        loadEquipment();

    }, []);

    const loadEquipment = async () => {

        const result =
            await getEquipment();

        setEquipment(result);
    };

    const filteredEquipment =
        useMemo(() => {

            return equipment.filter(x =>

                x.name
                    .toLowerCase()
                    .includes(
                        search.toLowerCase()
                    )

                ||

                x.hostname
                    .toLowerCase()
                    .includes(
                        search.toLowerCase()
                    )
            );

        }, [
            equipment,
            search
        ]);

    const totalCount =
        equipment.length;

    const activeCount =
        equipment.filter(
            x => x.isActive
        ).length;

    const inactiveCount =
        totalCount - activeCount;

    return (

        <Container maxWidth="xl">

            <Box
                sx={{
                    display: "flex",
                    justifyContent:
                        "space-between",
                    alignItems: "center",
                    mt: 3,
                    mb: 3
                }}
            >

                <Typography
                    variant="h4"
                >
                    Оборудование
                </Typography>

                <Button
                    variant="contained"
                    onClick={() =>
                        navigate(
                            "/equipment/create"
                        )
                    }
                >
                    Добавить оборудование
                </Button>

            </Box>

            <Box
                sx={{
                    display: "flex",
                    gap: 2,
                    mb: 3
                }}
            >

                <Card
                    elevation={2}
                    sx={{
                        flex: 1,
                        borderRadius: 3
                    }}
                >

                    <CardContent>

                        <Typography
                            variant="body2"
                        >
                            Всего
                        </Typography>

                        <Typography
                            variant="h4"
                        >
                            {totalCount}
                        </Typography>

                    </CardContent>

                </Card>

                <Card
                    elevation={2}
                    sx={{
                        flex: 1,
                        borderRadius: 3
                    }}
                >

                    <CardContent>

                        <Typography
                            variant="body2"
                        >
                            Active
                        </Typography>

                        <Typography
                            variant="h4"
                            color="success"
                        >
                            {activeCount}
                        </Typography>

                    </CardContent>

                </Card>

                <Card
                    elevation={2}
                    sx={{
                        flex: 1,
                        borderRadius: 3
                    }}
                >

                    <CardContent>

                        <Typography
                            variant="body2"
                        >
                            Inactive
                        </Typography>

                        <Typography
                            variant="h4"
                            color="error"
                        >
                            {inactiveCount}
                        </Typography>

                    </CardContent>

                </Card>

            </Box>

            <TextField
                fullWidth
                label="Поиск оборудования"
                placeholder="Введите имя или hostname..."
                value={search}
                onChange={(e) =>
                    setSearch(
                        e.target.value
                    )
                }
                sx={{ mb: 3 }}
            />

            {filteredEquipment.map(item => (

                <EquipmentCard
                    key={item.id}
                    equipment={item}
                />

            ))}

            {filteredEquipment.length === 0 && (

                <Typography
                    sx={{
                        mt: 4,
                        textAlign: "center"
                    }}
                >
                    Оборудование не найдено
                </Typography>

            )}

        </Container>
    );
}