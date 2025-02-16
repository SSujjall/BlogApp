import axios from "axios";

// Set up the base URL for your API (replace with your actual API URL)
const API_URL = "https://localhost:7108/api";

const getToken = () => {
  return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJkY2E4M2FkMC00MDQ0LTQ0NDItOGE0Yi02MWJhZjc2MjAxNTgiLCJqdGkiOiJlNzIzMTI5Ni01NGRjLTRiOTktOTk5Mi1mMDM2ZDdkYWRkOTAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNzM5Njg2MTc2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTA4LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3My8ifQ.nmVm6LCuSj6NSQHPP0TagplAtcjl8Red-MLrO2s2iHE";
} // put this in tokenHelper.js later

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
    const token = getToken();
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
