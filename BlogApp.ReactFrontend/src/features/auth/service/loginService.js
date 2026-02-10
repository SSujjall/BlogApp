import { makeRequest } from "../../../common/services/api";

export const loginWithGoogle = async (googleToken) => {
  try {
    return await makeRequest(
      "POST",
      "/Auth/google-login",
      { token: googleToken },
      true
    );
  } catch (error) {
    console.error("Error google login:", error);
    return null;
  }
};

export const login = async (data) => {
  try {
    return await makeRequest("POST", "/Auth/login", data);
  } catch (error) {
    console.error("Error login:", error);
    return null;
  }
};

export const resendVerificationEmail = async (data) => {
  try {
    return await makeRequest("POST", "/Auth/resend-verification", data);
  } catch (error) {
    console.error("Error resend verification email:", error);
    return null;
  }
}