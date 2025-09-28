/* eslint-disable react/prop-types */
import { createContext, useContext, useState } from "react";
import { removeTokens, setTokens } from "../utils/tokenHelper";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem("token") || null);
  const [refreshToken, setRefreshToken] = useState(
    localStorage.getItem("refreshToken") || null
  );

  const isAuthenticated = !!token;

  const login = (newToken, newRefreshToken) => {
    // localStorage.setItem("token", newToken);
    // localStorage.setItem("refreshToken", newRefreshToken);

    // using tokenHelper.js
    setTokens(newToken, newRefreshToken);

    setToken(newToken);
    setRefreshToken(newRefreshToken);
  };

  const logout = () => {
    // localStorage.removeItem("token");
    // localStorage.removeItem("refreshToken");

    // using tokenHelper.js
    removeTokens();
    
    setToken(null);
    setRefreshToken(null);
  };

  // const tryRefreshToken = async () => {
  //   if (!refreshToken) return logout();

  //   try {
  //     console.log("token hai ", token);
  //     console.log("refreshToken hai ", refreshToken);
  //     const response = await refreshTokenCall(token, refreshToken);
  //     login(response.data.jwtToken, response.data.refreshToken);
  //     console.log("refresh token is called haiiiiii");
  //   } catch {
  //     logout();
  //   }
  // };

  // useEffect(() => {
  //   if (isTokenExpired(token)) {
  //     tryRefreshToken();
  //   }
  // }, []);

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
