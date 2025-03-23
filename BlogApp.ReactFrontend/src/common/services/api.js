import axios from "axios";
import { getToken } from "../utils/tokenHelper";

// const API_URL = import.meta.env.VITE_API_BASE_URL;
// const API_URL = "https://1kt2mff1-7108.inc1.devtunnels.ms/api";
const API_URL = import.meta.env.VITE_PROD_API_BASE_URL;

// Create an Axios instance with base configuration
const api = axios.create({
  baseURL: API_URL,
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
    const config = {
      method,
      url,
      data,
      requiresAuth,
      headers: {},
    };

    /* Set Content-Type as json in headers only if the data is NOT FormData
     * else let axios handle it
     * This is for when api call is made with data that is not JSON
     * EG: CreateBlog api which accepts only [FromForm] attribute requests in the controller
     * It means that it accepts a form-data as request, not JSON
     */
    if (!(data instanceof FormData)) {
      config.headers["Content-Type"] = "application/json";
    }

    const response = await api(config);
    return response.data; // return the response data
  } catch (error) {
    console.error("API call error:", error);
  }
};

// Export the generic function
export { makeRequest };
