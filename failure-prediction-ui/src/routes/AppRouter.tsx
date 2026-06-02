import {
    BrowserRouter,
    Routes,
    Route
} from "react-router-dom";

import LoginPage from "../pages/LoginPage";
import EquipmentPage from "../pages/EquipmentPage";
import NotificationsPage from "../pages/NotificationsPage";

import MainLayout from "../layouts/MainLayout";
import ProtectedRoute from "./ProtectedRoute";
import CreateEquipmentPage from "../pages/CreateEquipmentPage";
import EquipmentDetailsPage from "../pages/EquipmentDetailsPage";

export default function AppRouter() {

    return (
        <BrowserRouter>

            <Routes>

                <Route
                    path="/"
                    element={<LoginPage />}
                />

                <Route
                    element={
                        <ProtectedRoute>
                            <MainLayout />
                        </ProtectedRoute>
                    }
                >

                    <Route
                        path="/equipment"
                        element={<EquipmentPage />}
                    />

                    <Route
                        path="/notifications"
                        element={<NotificationsPage />}
                    />

                    <Route
                        path="/equipment/create"
                        element={<CreateEquipmentPage />}
                    />
                    <Route
                        path="/equipment/:id"
                        element={<EquipmentDetailsPage />}
                    />

                </Route>

            </Routes>

        </BrowserRouter>
    );
}