import './App.css'
import { ToastContainer } from "react-toastify";
import AppRoutes from "./routes/Routes";

function App() {
  return (
    <>
      <AppRoutes />
      {/* Toast container to show the notifications */}
      <ToastContainer />
    </>
  )
}

export default App
