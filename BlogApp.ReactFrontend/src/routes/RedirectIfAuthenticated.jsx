import { Navigate } from "react-router-dom";
import { useAuth } from "../common/contexts/AuthContext";
import PropTypes from "prop-types";

const RedirectIfAuthenticated = ({ children }) => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/" />;
  }

  return children;
};

export default RedirectIfAuthenticated;

RedirectIfAuthenticated.propTypes = {
  children: PropTypes.node.isRequired,
};
