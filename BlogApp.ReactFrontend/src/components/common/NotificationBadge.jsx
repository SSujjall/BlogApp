const NotificationBadge = ({ numberOfNoti }) => {
  return (
    <div class="flex px-0.5 items-center">
      <button class="relative flex">
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