import { useNavigate, useSearchParams } from "react-router-dom";
import { useState } from "react";
import Button from "../../common/Button";
import CommonInputField from "../../common/CommonInputField";
import { Link } from "react-router-dom";
import PropTypes from "prop-types";
import { isAuthenticated } from "../../../common/utils/tokenHelper";

const TopBar = ({ toggleSidebar }) => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [search, setSearch] = useState(searchParams.get("search") || "");
  const authStatus = isAuthenticated();
  // const [menuVisible, setMenuVisible] = useState(false);

  const handleSearch = (e) => {
    e.preventDefault();
    navigate(search ? `/?search=${encodeURIComponent(search)}` : "/");
  };

  const handleLoginClick = () => {
    navigate("/login");
  };

  // const toggleMenu = () => {
  //   setMenuVisible((prev) => !prev);
  // };

  return (
    <nav className="fixed bg-gray-100 p-3 w-full flex items-center justify-between border-b border-gray-300 z-50">
      {/* Hidden left div with hamburger menu */}
      <div
        className="pr-2 text-2xl font-bold cursor-pointer lg:hidden"
        onClick={toggleSidebar}
      >
        &#9776; {/* Hamburger icon */}
      </div>

      {/* left Title div  */}
      <div className="pr-5 text-2xl font-bold">
        <Link to="/">MyBlog</Link>
      </div>

      {/* Middle Search div */}
      <form
        className="flex-1 flex justify-center py-xs"
        onSubmit={handleSearch}
      >
        <div className="w-full max-w-[560px] mx-auto">
          <CommonInputField
            placeholder={"Search Blog"}
            icon={"search"}
            onChange={(e) => setSearch(e.target.value)}
            isRequired={false}
          />
        </div>
      </form>

      {/* Right div with Login button */}
      <div className="pl-5 gap-xs flex items-center justify-end">
        {!authStatus ? (
          <Button
            text="Login"
            onClick={handleLoginClick}
            icon={"person"}
            iconSize={20}
            className={"text-white bg-gray-800 hover:bg-gray-700"}
          />
        ) : (
          <div className="flex gap-1">
            <Button
              icon={"add"}
              text={"Create"}
              className={"text-black hover:bg-gray-200 rounded-md gap-0"}
              onClick={() => navigate("/blog/addBlog")}
            />

            {/* <div className="menu">
              <Button
                icon={"person"}
                text={"user"}
                onClick={toggleMenu}
                className={"rounded-lg bg-black text-white"}
              />

              {menuVisible && (
                <div className="absolute right-0 mt-2 bg-white border border-gray-300 rounded-lg shadow-lg w-40">
                  <ul>
                    <li>
                      <Link
                        to="/user/profile"
                        className="block px-4 py-2 text-gray-700 hover:bg-gray-200"
                      >
                        Profile
                      </Link>
                    </li>
                    <li>
                      <Link
                        to="/user/settings"
                        className="block px-4 py-2 text-gray-700 hover:bg-gray-200"
                      >
                        Settings
                      </Link>
                    </li>
                    <li>
                      <Button text={"Logout"} className={"bg-red-500  text-white w-full"}/>
                    </li>
                  </ul>
                </div>
              )}
            </div> */}
          </div>
        )}
      </div>
    </nav>
  );
};

TopBar.propTypes = {
  toggleSidebar: PropTypes.func.isRequired,
};

export default TopBar;
