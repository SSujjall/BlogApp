import { useNavigate, useSearchParams } from "react-router-dom";
import { useState, useEffect } from "react";
import Button from "../../common/Button";
import CommonInputField from "../../common/CommonInputField";
import { Link } from "react-router-dom";
import PropTypes from "prop-types";
import { isAuthenticated } from "../../../common/utils/tokenHelper";

const Searchbar = ({ toggleSidebar }) => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [search, setSearch] = useState(searchParams.get("search") || "");
  const [authStatus, setAuthStatus] = useState(false);

  useEffect(() => {
    setAuthStatus(isAuthenticated());
  }, []);

  const handleSearch = (e) => {
    e.preventDefault();
    navigate(search ? `/?search=${encodeURIComponent(search)}` : "/");
  };

  const handleLoginClick = () => {
    navigate("/login");
  };

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
          <>
            <Button
              icon={"add"}
              text={"Create"}
              className={"text-black hover:bg-gray-200 rounded-full"}
            />

            <Button icon={"person"} className={"bg-black text-white rounded-full"}/>
          </>
        )}
      </div>
    </nav>
  );
};

Searchbar.propTypes = {
  toggleSidebar: PropTypes.func.isRequired,
};

export default Searchbar;
