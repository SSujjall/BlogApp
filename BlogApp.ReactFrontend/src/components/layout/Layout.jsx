/* eslint-disable react/prop-types */
import { useState } from "react";
import Searchbar from "./Searchbar";
import Sidebar from "./Sidebar";

const Layout = ({ children }) => {
  const [sidebarVisible, setSidebarVisible] = useState(false);

  const toggleSidebar = () => {
    setSidebarVisible(!sidebarVisible);
  };

  return (
    <div className="flex">
      <Searchbar toggleSidebar={toggleSidebar} />
      <Sidebar visible={sidebarVisible} toggleSidebar={toggleSidebar} />
      <div className="p-4 pt-20">{children}</div>
    </div>
  );
};

export default Layout;
