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
