import Button from "../common/Button";

const Searchbar = () => {
  return (
    <>
      <nav className="fixed bg-red-400 p-3 w-full flex items-center justify-between">
        <div className="pr-5 text-2xl font-bold">MyBlog</div>

        <div className="flex-1 flex justify-center py-xs">
          <div className="w-full max-w-[560px] m:mx-auto">
            {/* Search bar */}
            <input
              type="text"
              placeholder="Search"
              className="w-full p-2 rounded-md"
            />
          </div>
        </div>

        <div className="pl-5 gap-xs flex items-center justify-end">
          <Button text="Login" icon={"person"} iconSize={20} />
        </div>
      </nav>
    </>
  );
};

export default Searchbar;
