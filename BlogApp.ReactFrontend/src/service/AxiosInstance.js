import axios from "axios";

const AxiosInstance = axios.create({
  baseURL: "backendURL", // Replace with your API base URL
  timeout: 10000, // Request timeout (in milliseconds)
  headers: {
    "Content-Type": "application/json",
  },
});

// Add a request interceptor
AxiosInstance.interceptors.request.use(
  (config) => {
    // Optionally attach a token for authenticated requests
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add a response interceptor
AxiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle API errors globally
    if (error.response) {
      console.error("API Error:", error.response.data.message || error.message);
    } else {
      console.error("Network Error:", error.message);
    }
    return Promise.reject(error);
  }
);

export default AxiosInstance;
