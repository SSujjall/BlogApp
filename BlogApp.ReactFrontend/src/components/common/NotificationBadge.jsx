import { useNavigate } from "react-router-dom"

const NotificationBadge = ({ numberOfNoti }) => {
  const navigate = useNavigate();
  
  const onButtonClick = () => {
    navigate("/notifications");
  }
  
  return (
    <div className="flex px-2 items-center hover:bg-gray-200 rounded transition-colors">
      <button className="relative flex" onClick={onButtonClick}>
        <i
          className="material-symbols-rounded"
        >
          notifications
        </i>

        <span className="absolute bg-red-500 text-blue-100 px-1 text-xs font-bold rounded-full -top-2 -right-2">
          {numberOfNoti}
        </span>
      </button>
    </div>
  )
}

export default NotificationBadge