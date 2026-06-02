export const getEquipmentTypeName =
    (type: number) => {

    switch (type) {

        case 1:
            return "Server";

        case 2:
            return "Workstation";

        case 3:
            return "Network Device";

        case 4:
            return "Storage";

        default:
            return "Other";
    }
};