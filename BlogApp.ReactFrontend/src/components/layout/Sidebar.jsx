import { useEffect } from "react";
import { FaTimes } from "react-icons/fa"; // Import close icon
import MenuLinks from "./menu/MenuLink";

const Sidebar = ({ visible, toggleSidebar }) => {
  // Handle screen size changes
  useEffect(() => {
    const mediaQuery = window.matchMedia("(min-width: 1024px)"); // This is for "lg" breakpoint and up
    const handleResize = () => {
      if (mediaQuery.matches) {
        // Close sidebar on large screens and up
        if (visible) toggleSidebar();
      }
    };

    // Add event listener for screen resize
    mediaQuery.addListener(handleResize);

    // Cleanup on unmount
    return () => {
      mediaQuery.removeListener(handleResize);
    };
  }, [visible, toggleSidebar]);

  return (
    <>
      {/* Sidebar for larger screens (visible by default) */}
      <div className="hidden lg:block w-72 bg-gray-100 h-screen p-4 pt-20 text-white">
        <MenuLinks />
      </div>

      {/* Sidebar for smaller screens (hidden by default and visible when the menu is toggled) */}
      <div
        className={`fixed top-0 left-0 w-72 bg-gray-100 h-full p-4 pt-14 text-white z-50 transform ${
          visible ? "translate-x-0" : "-translate-x-full"
        } transition-all duration-300`}
      >
        {/* Close button for smaller screens */}
        <div
          className="absolute top-5 right-4 cursor-pointer"
          onClick={toggleSidebar}
        >
          <FaTimes size={24} color="black" />
        </div>
        <MenuLinks /> {/* Displaying Menu links */}
      </div>
    </>
  );
};

export default Sidebar;
