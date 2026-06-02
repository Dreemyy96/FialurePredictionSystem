import {
    Button,
    Container,
    MenuItem,
    TextField,
    Typography
} from "@mui/material";

import { useState } from "react";

import { createEquipment }
    from "../api/equipmentApi";

import { useNavigate }
    from "react-router-dom";

export default function CreateEquipmentPage() {

    const navigate = useNavigate();

    const [name, setName] = useState("");
    const [hostname, setHostname] = useState("");
    const [location, setLocation] = useState("");

    const [type, setType] = useState(1);

    const handleSubmit = async () => {

        await createEquipment({
            agentId: crypto.randomUUID(),
            agentToken: crypto.randomUUID(),

            name,
            hostname,
            location,
            type
        });

        navigate("/equipment");
    };

    return (
        <Container>

            <Typography
                variant="h4"
                sx={{ mb: 3 }}
            >
                Создание оборудования
            </Typography>

            <TextField
                fullWidth
                label="Название"
                margin="normal"
                value={name}
                onChange={e =>
                    setName(e.target.value)
                }
            />

            <TextField
                fullWidth
                label="Hostname"
                margin="normal"
                value={hostname}
                onChange={e =>
                    setHostname(e.target.value)
                }
            />

            <TextField
                fullWidth
                label="Расположение"
                margin="normal"
                value={location}
                onChange={e =>
                    setLocation(e.target.value)
                }
            />

            <TextField
                select
                fullWidth
                margin="normal"
                label="Тип оборудования"
                value={type}
                onChange={e =>
                    setType(Number(e.target.value))
                }
            >
                <MenuItem value={1}>
                    Server
                </MenuItem>

                <MenuItem value={2}>
                    Workstation
                </MenuItem>

                <MenuItem value={3}>
                    Network Device
                </MenuItem>

                <MenuItem value={4}>
                    Storage
                </MenuItem>

                <MenuItem value={5}>
                    Other
                </MenuItem>

            </TextField>

            <Button
                variant="contained"
                onClick={handleSubmit}
            >
                Создать
            </Button>

        </Container>
    );
}