import { useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { ChevronDown, ChevronUp } from "lucide-react";
import menuData from "../../../routes/menuData";

const MenuLink = () => {
  const [openMenus, setOpenMenus] = useState({});
  const location = useLocation();

  const toggleMenu = (menuName) => {
    setOpenMenus((prev) => ({
      ...prev,
      [menuName]: !prev[menuName],
    }));
  };

  // Check if any child of a parent menu is active
  const isParentActive = (children) => {
    return children.some((child) => child.link === location.pathname);
  };

  const renderSingleLink = (item) => (
    <Link
      key={item.name}
      to={item.link}
      className={`flex px-4 py-3 gap-1 rounded-lg transition-colors duration-200 
        ${
          location.pathname === item.link
            ? "bg-gray-200 text-gray-900 font-medium"
            : "text-gray-700 hover:bg-gray-200 hover:text-gray-900"
        }`}
    >
      {item.icon && (
        <i className="material-symbols-rounded" style={{ userSelect: "none" }}>
          {item.icon}
        </i>
      )}
      {item.name}
    </Link>
  );

  const renderParentLink = (item) => {
    const hasActiveChild = isParentActive(item.children);
    const isOpen = openMenus[item.name];

    return (
      <div key={item.name} className="mb-2">
        <button
          onClick={() => toggleMenu(item.name)}
          className={`w-full flex px-4 py-3 gap-1 items-center justify-between rounded-lg transition-colors duration-200 
            ${
              hasActiveChild
                ? "bg-gray-200 text-gray-900 font-medium"
                : isOpen
                ? "bg-gray-100 text-gray-900"
                : "text-gray-700 hover:bg-gray-200 hover:text-gray-900"
            }`}
        >
          {/* Left side: Icon + Name */}
          <div className="flex items-center gap-1">
            {item.parentIcon && (
              <i
                className="material-symbols-rounded"
                style={{ userSelect: "none" }}
              >
                {item.parentIcon}
              </i>
            )}
            <span>{item.name}</span>
          </div>

          {/* Right side: Expand/Collapse icon */}
          {isOpen ? (
            <ChevronUp
              className={`w-4 h-4 ${
                hasActiveChild ? "text-gray-900" : "text-gray-600"
              }`}
            />
          ) : (
            <ChevronDown
              className={`w-4 h-4 ${
                hasActiveChild ? "text-gray-900" : "text-gray-600"
              }`}
            />
          )}
        </button>

        <div
          className={`overflow-hidden transition-all duration-200 ease-in-out
            ${isOpen ? "max-h-96 opacity-100" : "max-h-0 opacity-0"}`}
        >
          <div className="pl-4 mt-1 space-y-1">
            {item.children.map((child) => (
              <Link
                key={child.name}
                to={child.link}
                className={`flex items-center px-4 py-2 gap-1 rounded-lg transition-colors duration-200
                  ${
                    location.pathname === child.link
                      ? "bg-gray-200 text-gray-900 font-medium"
                      : "text-gray-700 hover:bg-gray-200 hover:text-gray-900"
                  }`}
              >
                {child.icon && (
                  <i
                    className="material-symbols-rounded text-xl"
                    style={{ userSelect: "none" }}
                  >
                    {child.icon}
                  </i>
                )}
                {child.name}
              </Link>
            ))}
          </div>
        </div>
      </div>
    );
  };

  return (
    <nav className="space-y-2">
      {menuData.map((item) =>
        item.type === "single" ? renderSingleLink(item) : renderParentLink(item)
      )}
    </nav>
  );
};

export default MenuLink;
