import { Navigate } from "react-router-dom";
import type { ReactNode } from "react";

import {
    isAuthenticated
} from "../services/authService";

interface Props {
    children: ReactNode;
}

export default function ProtectedRoute({
    children
}: Props) {

    if (!isAuthenticated()) {
        return <Navigate to="/" />;
    }

    return children;
}