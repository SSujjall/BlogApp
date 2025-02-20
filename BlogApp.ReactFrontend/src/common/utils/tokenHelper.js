// Get the token from localStorage
export const getToken = () => {
  return localStorage.getItem("token");
};

// Set the token in localStorage
export const setToken = (token) => {
  localStorage.setItem("token", token);
};

// Remove the token from localStorage (logout)
export const removeToken = () => {
  localStorage.removeItem("token");
};

// Check if the user is authenticated
export const isAuthenticated = () => {
  return !!getToken(); // Returns true if the token exists
};
