import { makeRequest } from "../../../common/services/api";

export const signup = async (data) => {
  try {
    return await makeRequest(
      "POST",
      "/Auth/register",
      data
    );
  } catch (error) {
    console.error("Error signup:", error);
    return null;
  }
};
