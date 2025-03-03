// Get the token from localStorage
export const getToken = () => {
  return localStorage.getItem("token");
};

export const getRefreshToken = () => {
  return localStorage.getItem("refreshToken");
};

// Set the token in localStorage
export const setTokens = (token, refreshToken) => {
  localStorage.setItem("token", token);
  localStorage.setItem("refreshToken", refreshToken);

};

// Remove the token from localStorage (logout)
export const removeTokens = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("refreshToken");
};

// Check if the user is authenticated
export const isAuthenticated = () => {
  return !!getToken(); // Returns true if the token exists
};
