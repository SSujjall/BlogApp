import { makeRequest } from "../../../common/services/api";

export const createOrder = async (subscriptionId) => {
  return await makeRequest("POST", "/Order/create", { subscriptionId }, true);
};

export const getUserOrders = async () => {
  return await makeRequest("GET", "/Order/get-user-orders", null, true);
};

export const cancelOrder = async (orderId) => {
  return await makeRequest("POST", "/Order/cancel", { orderId }, true);
};
