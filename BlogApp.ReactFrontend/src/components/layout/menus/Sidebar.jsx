import { useEffect } from "react";
import { FaTimes } from "react-icons/fa";
import MenuLink from "./MenuLink";
import PropTypes from "prop-types";
import Button from "../../common/Button";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../common/contexts/AuthContext";
import { logout } from "../../../features/auth/service/logoutService";
import {
  showSuccessToast,
  showErrorToast,
} from "../../../common/utils/toastHelper";

const Sidebar = ({ visible, toggleSidebar }) => {
  const navigate = useNavigate();
  const { isAuthenticated, logout: contextLogout } = useAuth();

  // Handle screen size changes
  useEffect(() => {
    const mediaQuery = window.matchMedia("(min-width: 1024px)");
    const handleResize = () => {
      if (mediaQuery.matches && visible) {
        toggleSidebar();
      }
    };

    mediaQuery.addEventListener("change", handleResize);
    return () => mediaQuery.removeEventListener("change", handleResize);
  }, [visible, toggleSidebar]);

  const handleLogoutClick = async () => {
    try {
      const apiResponse = await logout();
      if (apiResponse.statusCode === 200) {
        navigate("/");

        contextLogout();
        showSuccessToast("Logout successful");
      } else {
        let errorMessage = apiResponse.message;
        if (apiResponse.errors) {
          const errorMessages = Object.values(apiResponse.errors).join(" ");
          errorMessage = `${apiResponse.message}. ${errorMessages}`;
        }
        showErrorToast(errorMessage);
      }
    } catch (error) {
      console.error("Error during logout:", error);
    }
  };

  return (
    <>
      {/* Overlay - only visible on small screens when sidebar is open */}
      {visible && (
        <div
          className="lg:hidden fixed inset-0 z-40"
          onClick={toggleSidebar}
          aria-hidden="true"
        />
      )}

      {/* Sidebar for larger screens */}
      <div className="hidden lg:block w-72 bg-gray-100 border-r border-gray-300 h-screen p-4 pt-20 text-white">
        <div className="flex flex-col justify-between h-full">
          <MenuLink />
          {isAuthenticated && (
            <Button
              icon={"logout"}
              text={"Logout"}
              className={
                "border border-gray-700 text-gray-700 hover:bg-red-600 hover:border-red-600 w-full hover:text-white mb-5"
              }
              onClick={handleLogoutClick}
            />
          )}
        </div>
      </div>

      {/* Sidebar for smaller screens */}
      <div
        className={`fixed top-0 left-0 border-r border-gray-300 w-72 bg-gray-100 h-full p-4 pt-20 text-white z-50 transform 
          ${
            visible ? "translate-x-0" : "-translate-x-full"
          } transition-all duration-300`}
      >
        <button
          className="absolute top-5 right-4 cursor-pointer"
          onClick={toggleSidebar}
          aria-label="Close sidebar"
        >
          <FaTimes size={24} color="black" />
        </button>

        <div className="flex flex-col justify-between h-full">
          <MenuLink />
          {isAuthenticated && (
            <Button
              icon={"logout"}
              text={"Logout"}
              className={
                "border border-gray-700 text-gray-700 hover:bg-red-600 hover:border-red-600 w-full hover:text-white mb-5"
              }
              onClick={handleLogoutClick}
            />
          )}
        </div>
      </div>
    </>
  );
};

Sidebar.propTypes = {
  visible: PropTypes.bool.isRequired,
  toggleSidebar: PropTypes.func.isRequired,
};

export default Sidebar;
