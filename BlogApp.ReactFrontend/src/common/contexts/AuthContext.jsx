/* eslint-disable react/prop-types */
import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem("token") || null);
  const [refreshToken, setRefreshToken] = useState(
    localStorage.getItem("refreshToken") || null
  );

  const isAuthenticated = !!token;

  const login = (newToken, newRefreshToken) => {
    localStorage.setItem("token", newToken);
    localStorage.setItem("refreshToken", newRefreshToken);
    setToken(newToken);
    setRefreshToken(newRefreshToken);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    setToken(null);
    setRefreshToken(null);
  };

  return (
    <AuthContext.Provider
      value={{
        token,
        refreshToken,
        isAuthenticated,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);