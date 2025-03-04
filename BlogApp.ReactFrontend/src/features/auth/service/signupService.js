import { makeRequest } from "../../../common/services/api";

export const signup = async (data) => {
  try {
    return await makeRequest("POST", "/Auth/register", data);
  } catch {
    return null;
  }
};
