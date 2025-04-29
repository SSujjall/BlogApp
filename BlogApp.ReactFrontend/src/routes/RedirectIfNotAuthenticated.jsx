import { Navigate } from "react-router-dom";
import { useAuth } from "../common/contexts/AuthContext";
import PropTypes from "prop-types";

const RedirectIfNotAuthenticated = ({ children }) => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/" />;
  }

  return children;
};

export default RedirectIfNotAuthenticated;

RedirectIfNotAuthenticated.propTypes = {
  children: PropTypes.node.isRequired,
};
