/* eslint-disable react/prop-types */
import Searchbar from "./Searchbar";
import Sidebar from "./Sidebar";

const Layout = ({ children }) => {
  return (
    <div className="flex">
      <Searchbar />
      <Sidebar />
      <div className="p-4 pt-20">{children}</div>
    </div>
  );
};

export default Layout;
