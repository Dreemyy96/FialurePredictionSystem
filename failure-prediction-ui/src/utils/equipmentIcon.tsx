import DnsIcon
    from "@mui/icons-material/Dns";

import ComputerIcon
    from "@mui/icons-material/Computer";

import RouterIcon
    from "@mui/icons-material/Router";

import StorageIcon
    from "@mui/icons-material/Storage";

import DevicesOtherIcon
    from "@mui/icons-material/DevicesOther";

export const getEquipmentIcon =
    (type: number) => {

        switch (type) {

            case 1:
                return <DnsIcon sx={{ fontSize: 60 }} />;

            case 2:
                return <ComputerIcon sx={{ fontSize: 60 }} />;

            case 3:
                return <RouterIcon sx={{ fontSize: 60 }} />;

            case 4:
                return <StorageIcon sx={{ fontSize: 60 }} />;

            default:
                return (
                    <DevicesOtherIcon
                        sx={{ fontSize: 60 }}
                    />
                );
        }
    };