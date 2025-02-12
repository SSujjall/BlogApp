import { useEffect } from "react";
import { FaTimes } from "react-icons/fa";
import MenuLink from "./menu/MenuLink";
import PropTypes from 'prop-types';

const Sidebar = ({ visible, toggleSidebar }) => {
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
        <MenuLink />
      </div>

      {/* Sidebar for smaller screens */}
      <div
        className={`fixed top-0 left-0 border-r border-gray-300 w-72 bg-gray-100 h-full p-4 pt-20 text-white z-50 transform 
          ${
            visible ? "translate-x-0" : "-translate-x-full"
          } transition-all duration-300`}
      >
        <button
          className="absolute top-4 right-4 cursor-pointer"
          onClick={toggleSidebar}
          aria-label="Close sidebar"
        >
          <FaTimes size={24} color="black" />
        </button>

        <MenuLink />
      </div>
    </>
  );
};

Sidebar.propTypes = {
  visible: PropTypes.bool.isRequired,
  toggleSidebar: PropTypes.func.isRequired,
};

export default Sidebar;
