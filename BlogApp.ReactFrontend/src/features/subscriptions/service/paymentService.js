import { makeRequest } from "../../../common/services/api";

export const initiatePayment = async (orderId, provider) => {
  return await makeRequest("POST", "/Payment/initiate", { orderId, provider }, true);
};

export const verifyPayment = async (externalTxnId, data) => {
  return await makeRequest("POST", "/Payment/verify", { externalTxnId, data }, true);
};

export const getOrderPayments = async (orderId) => {
  return await makeRequest("GET", `/Payment/get-all/${orderId}`, null, true);
};

export const retryPayment = async (paymentId) => {
  return await makeRequest("GET", `/Payment/retry/${paymentId}`, null, true);
};
