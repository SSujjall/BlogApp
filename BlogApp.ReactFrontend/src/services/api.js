import axios from "axios";

// Set up the base URL for your API (replace with your actual API URL)
const API_URL = "https://localhost:7108/api";

// Create an Axios instance with base configuration
const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Generic function to make API calls
const makeRequest = async (method, url, data = null) => {
  try {
    const response = await api({
      method,
      url,
      data,
    });
    return response.data; // return the response data
  } catch (error) {
    console.error("API call error:", error);
    throw error; // You can customize the error handling as needed
  }
};

// Export the generic function
export { makeRequest };
