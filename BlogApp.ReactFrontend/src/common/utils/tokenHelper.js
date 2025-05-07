/**
 * * AuthContext.jsx uses this file.
 */


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

export const isTokenExpired = (token) => {
  if (!token) return true;

  const [, payload] = token.split('.');
  if (!payload) return true;

  try {
    const decoded = JSON.parse(atob(payload));
    const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
    return currentTime > decoded.exp ; // Check if the token has expired
  } catch {
    return true; // If there's an error, assume the token is expired
  }
}