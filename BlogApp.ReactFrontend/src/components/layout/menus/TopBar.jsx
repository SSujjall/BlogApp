import { useNavigate, useSearchParams } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import Button from "../../common/Button";
import CommonInputField from "../../common/CommonInputField";
import { Link } from "react-router-dom";
import PropTypes from "prop-types";
import { useAuth } from "../../../common/contexts/AuthContext";
import NotificationBadge from "../../common/NotificationBadge";

const TopBar = ({ toggleSidebar }) => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [search, setSearch] = useState(searchParams.get("search") || "");
  const { isAuthenticated } = useAuth();

  const [showMobileSearch, setShowMobileSearch] = useState(false);
  const searchContainerRef = useRef(null);
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

  // Close search bar when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (
        searchContainerRef.current &&
        !searchContainerRef.current.contains(event.target) &&
        showMobileSearch
      ) {
        setShowMobileSearch(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [showMobileSearch]);

  return (
    <nav className="fixed bg-gray-100 p-3 w-full flex items-center justify-between border-b border-gray-300 z-50">
      <section className="flex left-section">
        {/* Hidden left div with hamburger menu */}
        {!showMobileSearch && (
          <div
            className="pr-2 text-2xl font-bold cursor-pointer lg:hidden"
            onClick={toggleSidebar}
          >
            &#9776; {/* Hamburger icon */}
          </div>
        )}

        {/* left Title div  */}
        {!showMobileSearch && (
          <div className="pr-5 text-2xl font-bold">
            <Link to="/" onClick={() => window.reload()}>
              MyBlog
            </Link>
          </div>
        )}
      </section>

      {/* Middle Search div */}
      <div
        ref={searchContainerRef}
        className={`${showMobileSearch ? "flex-1" : "hidden sm:flex flex-1"
          } justify-center py-xs`}
      >
        <form className="w-full max-w-[560px] mx-auto" onSubmit={handleSearch}>
          <CommonInputField
            placeholder={"Search Blog"}
            icon={"search"}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            isRequired={false}
          />
        </form>
      </div>

      {/* Right div with Login button */}
      <div
        className={`${showMobileSearch ? "" : "pl-5"
          } gap-1 flex items-center justify-end`}
      >
        {/* Mobile search icon - only visible on small screens */}
        {!showMobileSearch && (
          <Button
            icon={"search"}
            className="sm:hidden text-black hover:bg-gray-200 rounded-full"
            onClick={() => setShowMobileSearch(true)}
          />
        )}

        {/* Login and user action buttons */}
        {!showMobileSearch && (
          <>
            {!isAuthenticated ? (
              <Button
                text="Login"
                onClick={handleLoginClick}
                icon={"person"}
                iconSize={20}
                className={"text-white bg-gray-800 hover:bg-gray-700"}
              />
            ) : (
              <div className="flex">
                <NotificationBadge
                  numberOfNoti={10} />

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
                          <Button
                            text={"Logout"}
                            className={"bg-red-500  text-white w-full"}
                          />
                        </li>
                      </ul>
                    </div>
                  )}
                </div> */}
              </div>
            )}
          </>
        )}
      </div>
    </nav>
  );
};

TopBar.propTypes = {
  toggleSidebar: PropTypes.func.isRequired,
};

export default TopBar;
