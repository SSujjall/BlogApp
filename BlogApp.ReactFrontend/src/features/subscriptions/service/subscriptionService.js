import { makeRequest } from "../../../common/services/api";

export const getAllSubscriptions = async () => {
  return await makeRequest("GET", "/Subscription/get-all");
};
