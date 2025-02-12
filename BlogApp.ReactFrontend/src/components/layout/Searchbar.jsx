import { useNavigate } from "react-router-dom";
import Button from "../common/Button";
import CommonInputField from "../common/CommonInputField";
import { Link } from "react-router-dom";
import PropTypes from "prop-types";

const Searchbar = ({ toggleSidebar }) => {
  const navigate = useNavigate();

  const handleLoginClick = () => {
    navigate("/login");
  };

  return (
    <nav className="fixed bg-gray-100 p-3 w-full flex items-center justify-between border-b border-gray-300">
      {/* Hidden left div with hamburger menu */}
      <div
        className="pr-2 text-2xl font-bold cursor-pointer lg:hidden"
        onClick={toggleSidebar}
      >
        &#9776; {/* Hamburger icon */}
      </div>

      {/* left div  */}

      <div className="pr-5 text-2xl font-bold">
        <Link to="/">MyBlog</Link>
      </div>

      {/* Middle div */}
      <div className="flex-1 flex justify-center py-xs">
        <div className="w-full max-w-[560px] mx-auto">
          <CommonInputField
            placeholder={"Search Blog"}
            icon={"search"}
            classProp={"focus:ring-2 focus:ring-blue-500"}
          />
        </div>
      </div>

      {/* Right div with Login button */}
      <div className="pl-5 gap-xs flex items-center justify-end">
        <Button
          text="Login"
          onClick={handleLoginClick}
          icon={"person"}
          iconSize={20}
          className={"bg-gray-800 hover:bg-gray-700"}
        />
      </div>
    </nav>
  );
};

Searchbar.propTypes = {
  toggleSidebar: PropTypes.func.isRequired,
};

export default Searchbar;
