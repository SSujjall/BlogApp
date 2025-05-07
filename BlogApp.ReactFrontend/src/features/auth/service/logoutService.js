import { makeRequest } from "../../../common/services/api";

export const logout = async () => {
  try {
    return await makeRequest("POST", "/Auth/logout", null, true);
  } catch (error) {
    console.error("Error logout:", error);
    return null;
  }
};