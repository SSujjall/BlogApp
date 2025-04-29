const NotificationCard = ({ Title, Desc }) => {
  return (
    /* Container div */
    /* grid-cols-[2rem_1fr_3rem]::> 
      *     2rem: The first column will have a fixed width of 2rem.
      *     1fr: The second column will take up one fractional unit of the available space. This makes it flexible and responsive.
      *     3rem: The third column will have a fixed width of 3rem.
    */
    <div className="grid gap-1 grid-cols-[3rem_1fr_3rem] border items-center mb-2 py-2 px-1 cursor-pointer">
      {/* Left Div */}
      <div className="flex">
        <i
          className="material-symbols-rounded border rounded-full"
          style={{ fontSize: 40, userSelect: "none" }}
        >
          person
        </i>
      </div>

      {/* Mid Div */}
      <div className="flex flex-col items-start justify-center">
        {/* Title Div */}
        <div className="w-full">
          <p className="font-bold">{Title}</p>
        </div>

        {/* Desc div */}
        <div className="w-full">
          <p className="text-sm text-gray-700">{Desc}</p>
        </div>
      </div>

      {/* Right Div */}
      <div className="flex items-start justify-center h-full">
        <div>
          <i
            className="material-symbols-rounded p-1 text-gray-600 rounded-full hover:bg-gray-200 hover:text-black transition-all"
            style={{ fontSize: 30, userSelect: "none" }}
          >
            more_horiz
          </i>
        </div>
      </div>
    </div>
  )
}

export default NotificationCard