/* eslint-disable react/prop-types */
import { useState } from "react";
import TopBar from "./menus/TopBar";
import Sidebar from "./menus/Sidebar";

const Layout = ({ children }) => {
  const [sidebarVisible, setSidebarVisible] = useState(false);

  const toggleSidebar = () => {
    setSidebarVisible(!sidebarVisible);
  };

  return (
    <div className="flex h-screen overflow-hidden">
      <TopBar toggleSidebar={toggleSidebar} />
      <Sidebar visible={sidebarVisible} toggleSidebar={toggleSidebar} />
      <div className="p-4 pt-20 flex-1 overflow-y-auto">{children}</div>
    </div>
  );
};

export default Layout;
