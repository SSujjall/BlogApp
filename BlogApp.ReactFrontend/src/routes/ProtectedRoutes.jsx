import { Outlet } from "react-router-dom";
import { isTokenExpired, removeTokens } from "../common/utils/tokenHelper";
import { getToken } from "../common/utils/tokenHelper";

const ProtectedRoute = () => {
  const token = getToken();
  if (!token || isTokenExpired(token)) {
    removeTokens();
    // console.log("Token is missing or expired. Redirecting to login.");
  }
  return <Outlet />;
};

export default ProtectedRoute;
