import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import OtpPage from "./pages/OtpPage";
import WeatherForecast from "./pages/weather-forecast";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/otp" element={<OtpPage />} />
                <Route path="/weather" element={<WeatherForecast />} />
            </Routes>
        </Router>
    );
}

export default App;
