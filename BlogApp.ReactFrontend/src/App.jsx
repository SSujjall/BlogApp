import './App.css'
import { ToastContainer } from "react-toastify";
import AppRoutes from "./routes/Routes";
import 'react-toastify/dist/ReactToastify.css';


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
