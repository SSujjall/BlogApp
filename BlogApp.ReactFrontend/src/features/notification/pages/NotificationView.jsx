import { useEffect, useState } from "react";
import Layout from "../../../components/layout/Layout";
import NotificationCard from "../components/NotificationCard";
import * as signalR from '@microsoft/signalr';

const NotificationView = () => {
  const [notifications, setNotifications] = useState([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7108/strNotiHub", {
        withCredentials: false
      })
      .build();

    connection.on("ReceiveNotification", (user, message) => {
      setNotifications((prevNotif) => [
        ...prevNotif,
        { user, message }
      ]);
    });

    connection
      .start()
      .then(_ => {
        console.log("SignalR Connection Established");
      })
      .catch((err) => {
        console.error("SignalR Connection Error: ", err);
      });

    return () => {
      connection.stop();
      console.log("SignalR Connection Stopped")
    };
  }, []);

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
          {notifications.map((notifs, index) => (
            <div key={index}>
              <NotificationCard
                Title={notifs.user}
                Desc={notifs.message}
              />
            </div>
          ))}
        </div>

      </div>
    </Layout>
  )
}

export default NotificationView