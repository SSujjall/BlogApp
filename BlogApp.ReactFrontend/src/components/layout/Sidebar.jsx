import { useEffect } from "react";
import { FaTimes } from "react-icons/fa"; // Import close icon

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
      <div className="hidden md:block w-72 bg-gray-100 h-screen p-4 pt-20 text-white">
        <ul className="flex flex-col gap-1">
          <li className="w-full p-4 bg-gray-300 text-black rounded">Navlink 1</li>
          <li className="w-full p-4 hover:bg-gray-300 text-black rounded">Navlink 2</li>
        </ul>
      </div>

      {/* Sidebar for smaller screens (hidden by default and visible when the menu is toggled) */}
      <div
        className={`fixed top-0 left-0 w-72 bg-gray-100 h-full p-4 pt-20 text-white z-50 transform ${
          visible ? "translate-x-0" : "-translate-x-full"
        } transition-all duration-300`}
      >
        {/* Close button for smaller screens */}
        <div className="absolute top-4 right-4 cursor-pointer" onClick={toggleSidebar}>
          <FaTimes size={24} color="black" />
        </div>

        <ul className="flex flex-col gap-1">
          <li className="w-full p-4 bg-gray-300 text-black rounded">Navlink 1</li>
          <li className="w-full p-4 hover:bg-gray-300 text-black rounded">Navlink 2</li>
        </ul>
      </div>
    </>
  );
};

export default Sidebar;
