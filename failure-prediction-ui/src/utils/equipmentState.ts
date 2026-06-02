export const getEquipmentStateName =
    (state: number) => {

    switch (state) {

        case 1:
            return "Normal";

        case 2:
            return "Warning";

        case 3:
            return "Critical";

        default:
            return "Unknown";
    }
};

export const getEquipmentStateColor =
    (state: number) => {

    switch (state) {

        case 1:
            return "success";

        case 2:
            return "warning";

        case 3:
            return "error";

        default:
            return "default";
    }
};