import {
    Card,
    CardContent,
    Typography,
    Chip,
    Button,
    Box
} from "@mui/material";

import {
    useNavigate
} from "react-router-dom";

import type {
    Equipment
} from "../types/equipment";

import {
    getEquipmentIcon
} from "../utils/equipmentIcon";

import {
    getEquipmentTypeName
} from "../utils/equipmentType";

type Props = {
    equipment: Equipment;
};

export default function EquipmentCard({
    equipment
}: Props) {

    const navigate = useNavigate();

    return (

        <Card 
            elevation={2}
            sx={{
                mb: 2,
                transition: "all 0.2s ease",
                cursor: "pointer",
                borderRadius: 3,

                "&:hover": {
                    transform: "translateY(-4px)",
                    boxShadow: 8
                }
            }}>

            <CardContent>

                <Box
                    sx={{
                        display: "flex",
                        gap: 3,
                        minHeight: 120,
                        alignItems: "stretch"
                    }}
                >

                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center"
                        }}
                    >
                        {getEquipmentIcon(
                            equipment.type
                        )}
                    </Box>

                    <Box
                        sx={{
                            flexGrow: 1
                        }}
                    >

                        <Typography variant="h5">
                            {equipment.name}
                        </Typography>

                        <Typography>
                            {getEquipmentTypeName(
                                equipment.type
                            )}
                        </Typography>

                        <Typography>
                            {equipment.location}
                        </Typography>

                        <Typography>
                            {equipment.hostname}
                        </Typography>

                    </Box>

                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            justifyContent: "space-between",
                            alignItems: "flex-end"
                        }}
                    >

                        <Button
                            variant="contained"
                            onClick={() =>
                                navigate(
                                    `/equipment/${equipment.id}`
                                )
                            }
                        >
                            Подробнее
                        </Button>

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
                        />

                    </Box>

                </Box>

            </CardContent>

        </Card>
    );
}