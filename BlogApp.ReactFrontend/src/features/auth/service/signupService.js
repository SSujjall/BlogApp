import { makeRequest } from "../../../common/services/api";

export const signup = async (data) => {
  try {
    return await makeRequest("POST", "/Auth/register", data);
  } catch {
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