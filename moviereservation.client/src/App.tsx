
import { SiteHeader } from "./components/layouts/site-header";

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
function App() {
    return (
        <Router>
            <SiteHeader />
            <Routes>
                <Route path="/" element={<h1>Home</h1>} />
                <Route path="/about" element={<h1>About</h1>} />
            </Routes>
        </Router>
    );
}

export default App;
