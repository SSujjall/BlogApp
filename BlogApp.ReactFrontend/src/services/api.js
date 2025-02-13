import axios from "axios";

// Set up the base URL for your API (replace with your actual API URL)
const API_URL = "https://localhost:7108/api";

const getToken = () => {
  return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJhYWZiYmRmNS0xNTViLTRiMzgtYTE5YS1kNjk0NmRlNjJkYjQiLCJqdGkiOiJmNTM1ZTVmNy1iNWQ5LTQ1ODktYWZhZi1kMmNiY2Y1M2ZjZjAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNzM5NDU4MzQwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTA4LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3My8ifQ.eZZpCLlE6y7F9A9Vf2A6bAgaT9F_1x8tCn7FWc40Dpo";
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
