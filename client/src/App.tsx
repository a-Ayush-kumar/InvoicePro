import Navbar from "./components/Navbar";
import Dashboard from "./pages/Dashboard";
import InvoiceEdit from "./pages/InvoiceEdit";
import { Routes, Route } from "react-router-dom";

export default function App() {
  return (
    <>
      <Navbar />

      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/invoice-edit" element={<InvoiceEdit />} />
      </Routes>
    </>
  );
}