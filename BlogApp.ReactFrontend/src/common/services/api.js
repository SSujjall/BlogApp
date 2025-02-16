import axios from "axios";
import { getToken } from "../utils/tokenHelper";

const API_URL = "https://localhost:7108/api";


// Create an Axios instance with base configuration
const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor to add token when required
api.interceptors.request.use((config) => {
  if (config.requiresAuth) {
    const token = getToken(); // getting token from local storage
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }
  return config;
});

// Generic function to make API calls
const makeRequest = async (method, url, data = null, requiresAuth = false) => {
  try {
    const response = await api({
      method,
      url,
      data,
      requiresAuth,
    });
    return response.data; // return the response data
  } catch (error) {
    console.error("API call error:", error);
  }
};

// Export the generic function
export { makeRequest };
