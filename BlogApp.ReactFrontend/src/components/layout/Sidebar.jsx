/* eslint-disable react/prop-types */
const Sidebar = ({ isCollapsed }) => {
    return (
      <div
        className={`w-64 bg-gray-800 text-white p-4 fixed h-full transition-all ${
          isCollapsed ? "w-16" : "w-64"
        }`}
      >
        <h2 className="text-xl mb-4">Blog App</h2>
        {/* Add more sidebar links here */}
        <ul>
          <li>Home</li>
          <li>Blogs</li>
        </ul>
      </div>
    );
  };
  
  export default Sidebar;
  