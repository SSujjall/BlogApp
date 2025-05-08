import axios from "axios";
import { getRefreshToken, getToken, setTokens } from "../utils/tokenHelper";

const API_URL = import.meta.env.VITE_API_BASE_URL;
// const API_URL = "https://1kt2mff1-7108.inc1.devtunnels.ms/api";
// const API_URL = import.meta.env.VITE_PROD_API_BASE_URL;

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

// ==== Token Refresh Management ====
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

/*
 * Use response interceptor to:
 * Catch 401 Unauthorized
 * Refresh token
 * Retry failed request
 */
// * Response interceptor to catch 401 and refresh token
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    // console.log(error);

    // Only attempt to refresh if:
    // * It's a 401 error
    // * It's an authenticated request
    // * The user is currently logged in (refresh token and access token exists)
    if (
      error.response?.status === 401 &&
      !originalRequest._retry &&
      originalRequest.requiresAuth &&
      getRefreshToken() &&
      getToken()
    ) {
      originalRequest._retry = true;

      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({
            resolve: (token) => {
              originalRequest.headers.Authorization = `Bearer ${token}`;
              resolve(api(originalRequest));
            },
            reject: (err) => reject(err),
          });
        });
      }

      isRefreshing = true;

      try {
        const refreshToken = getRefreshToken();
        const token = getToken();

        const res = await axios.post(`${API_URL}/Auth/refresh-token`, {
          jwtToken: token,
          refreshToken,
        });

        // console.log("ress:", res);

        const { jwtToken: newToken, refreshToken: newRefresh } = res.data.data;

        // console.log("jwt: ",newToken);
        // console.log("refresh ", newRefresh);

        // Set new tokens and try the previous request again
        setTokens(newToken, newRefresh);
        api.defaults.headers.Authorization = `Bearer ${newToken}`;
        originalRequest.headers.Authorization = `Bearer ${newToken}`;

        processQueue(null, newToken);
        return api(originalRequest); // Retry original request
      } catch (err) {
        processQueue(err, null);
        return Promise.reject(err);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

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

    /*
     * Set Content-Type as json in headers only if the data is NOT FormData
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
