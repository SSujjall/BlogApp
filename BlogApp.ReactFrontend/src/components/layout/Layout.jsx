/* eslint-disable react/prop-types */
import { useState } from "react";
import Sidebar from "./Sidebar";

const Layout = ({ children }) => {
  const [isSidebarCollapsed, setIsSidebarCollapsed] = useState(false);

  return (
    <div className="flex">
      <Sidebar isCollapsed={isSidebarCollapsed} />
      <div className="flex-1 ml-64 p-4">
        <div className="flex justify-between mb-4">
          <button onClick={() => setIsSidebarCollapsed(!isSidebarCollapsed)}>
            Toggle Sidebar
          </button>
        </div>
        {children}
      </div>
    </div>
  );
};

export default Layout;
