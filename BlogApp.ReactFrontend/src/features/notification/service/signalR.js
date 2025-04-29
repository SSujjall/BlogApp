import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7108/strNotiHub", {
    withCredentials: false
  })
  .build();

let notifications = [];

// Function to start the SignalR connection
const startConnection = () => {
  connection
    .start()
    .then(_ => {
      console.log("SignalR Connection Established");
    })
    .catch((err) => {
      console.error("SignalR Connection Error: ", err);
    });
}

// Function to receive notifications
const setupNotificationListener = (callback) => {
  connection.on("ReceiveNotification", (user, message) => {
    notifications.push({ user, message });
    callback(notifications); // Call the callback function passed to this setup
  });
}

// Function to send a notification
const sendNotification = async (user, message) => {
  try {
    await connection.invoke("SendMessage", user, message);
  } catch (err) {
    console.error("Error sending notification: ", err);
  }
};

// Stop the connection when needed
const stopConnection = () => {
  connection.stop().then(() => {
    console.log("SignalR connection stopped");
  });
};

export {
  startConnection,
  setupNotificationListener,
  sendNotification,
  stopConnection
};