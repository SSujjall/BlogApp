import { Navigate } from "react-router-dom";
import { useAuth } from "../common/contexts/AuthContext";
import PropTypes from "prop-types";
import { showErrorToast } from "../common/utils/toastHelper";

const RedirectOnlyIfAuthenticated = ({ children }) => {
  const { isAuthenticated } = useAuth();

  if (!isAuthenticated) {
    showErrorToast("Need to be logged in to view notifications.");
    return <Navigate to="/" />;
  }

  return children;
};

export default RedirectOnlyIfAuthenticated;

RedirectOnlyIfAuthenticated.propTypes = {
  children: PropTypes.node.isRequired,
};
