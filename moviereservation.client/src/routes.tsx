import { Route, Routes } from "react-router-dom";
import LoginPage from "@/pages/login";
import OtpPage from "@/pages/otp";

export default function PageRoutes() {
    return (
        <Routes>
            <Route path={ '/' } element={<LoginPage />}/>       
            <Route path={ '/otp' } element={<OtpPage />}/>       
        </Routes>
    )
}