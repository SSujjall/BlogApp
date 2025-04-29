import Layout from "../../../components/layout/Layout";
import NotificationCard from "../components/NotificationCard";

const NotificationView = () => {
  return (
    <Layout>
      <h1 className="text-2xl font-bold">Notifications</h1>

      <div className="mx-auto max-w-4xl border rounded-md p-4 shadow-sm mt-3">
        {/* top div */}
        <div className="flex justify-end" style={{ userSelect: "none" }}>
          <p className="font-bold text-gray-600 hover:text-gray-800 transition-all cursor-pointer">
            Mark all as read
          </p>
        </div>

        <div className="notifications-container mt-5">
          {Array(5).fill().map((_, index) => (
            <NotificationCard
              Title={"Hello"}
              Desc={"K gardai xau. Test notif."}
            />
          ))}
        </div>
      </div>
    </Layout>
  )
}

export default NotificationView