import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Layout from "../../../components/layout/Layout";
import { getUserOrders, cancelOrder } from "../service/orderService";
import {
  getOrderPayments,
  initiatePayment,
  retryPayment,
} from "../service/paymentService";

const ORDER_STATUS = {
  0: { label: "Pending", className: "bg-yellow-100 text-yellow-700" },
  1: { label: "Completed", className: "bg-green-100 text-green-700" },
  2: { label: "Canceled", className: "bg-red-100 text-red-600" },
};

const PAYMENT_STATUS = {
  0: { label: "Pending", className: "bg-yellow-100 text-yellow-700" },
  1: { label: "Completed", className: "bg-green-100 text-green-700" },
  2: { label: "Failed", className: "bg-red-100 text-red-600" },
  4: { label: "Canceled", className: "bg-gray-100 text-gray-500" },
};

const PROVIDER_NAMES = { 1: "eSewa", 2: "Khalti" };

const PROVIDERS = [
  { label: "eSewa", value: 1 },
  { label: "Khalti", value: 2 },
];

const formatDate = (dateString) => {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const OrderDetail = () => {
  const { orderId } = useParams();
  const navigate = useNavigate();

  const [order, setOrder] = useState(null);
  const [payments, setPayments] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const [selectedProvider, setSelectedProvider] = useState(1);
  const [isInitiating, setIsInitiating] = useState(false);
  const [initError, setInitError] = useState(null);

  const [isCanceling, setIsCanceling] = useState(false);
  const [cancelError, setCancelError] = useState(null);
  const [showCancelConfirm, setShowCancelConfirm] = useState(false);

  // Per-payment retry state: { [paymentId]: { isLoading, error } }
  const [paymentRetry, setPaymentRetry] = useState({});

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      try {
        const [ordersRes, paymentsRes] = await Promise.all([
          getUserOrders(),
          getOrderPayments(orderId),
        ]);

        const allOrders = Array.isArray(ordersRes?.data) ? ordersRes.data : [];
        const found = allOrders.find((o) => o.orderId === parseInt(orderId));
        if (!found) {
          setError("Order not found.");
          return;
        }
        setOrder(found);
        setPayments(Array.isArray(paymentsRes?.data) ? paymentsRes.data : []);
      } catch {
        setError("Failed to load order details.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [orderId]);

  const handleRetryPayment = async () => {
    setIsInitiating(true);
    setInitError(null);
    try {
      const res = await initiatePayment(parseInt(orderId), selectedProvider);
      if (!res?.status) {
        setInitError(res?.message || "Failed to initiate payment.");
        return;
      }
      const { redirectUrl, externalTxnId } = res.data;
      localStorage.setItem(
        "pendingPayment",
        JSON.stringify({ externalTxnId, provider: selectedProvider }),
      );
      window.location.href = redirectUrl;
    } catch {
      setInitError("Something went wrong. Please try again.");
    } finally {
      setIsInitiating(false);
    }
  };

  const handleCancel = async () => {
    setIsCanceling(true);
    setCancelError(null);
    try {
      const res = await cancelOrder(parseInt(orderId));
      if (!res?.status) {
        setCancelError(res?.message || "Failed to cancel order.");
        return;
      }
      setOrder((prev) => ({ ...prev, status: 2 }));
      setShowCancelConfirm(false);
    } catch {
      setCancelError("Something went wrong. Please try again.");
    } finally {
      setIsCanceling(false);
    }
  };

  const handlePaymentRetry = async (payment) => {
    const { paymentId, provider } = payment;
    setPaymentRetry((prev) => ({
      ...prev,
      [paymentId]: { isLoading: true, error: null },
    }));
    try {
      const res = await retryPayment(paymentId);
      if (!res?.status) {
        setPaymentRetry((prev) => ({
          ...prev,
          [paymentId]: {
            isLoading: false,
            error: res?.message || "Failed to retry payment.",
          },
        }));
        return;
      }
      const { redirectUrl, externalTxnId } = res.data;
      localStorage.setItem(
        "pendingPayment",
        JSON.stringify({ externalTxnId, provider }),
      );
      window.location.href = redirectUrl;
    } catch {
      setPaymentRetry((prev) => ({
        ...prev,
        [paymentId]: {
          isLoading: false,
          error: "Something went wrong. Please try again.",
        },
      }));
    }
  };

  if (isLoading) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Loading order details...</p>
        </div>
      </Layout>
    );
  }

  if (error || !order) {
    return (
      <Layout>
        <div className="flex flex-col justify-center items-center min-h-[50vh] gap-4">
          <p className="text-lg text-red-500">{error ?? "Order not found."}</p>
          <button
            onClick={() => navigate("/settings/order-history")}
            className="text-sm text-gray-500 hover:text-gray-800 underline"
          >
            Back to Order History
          </button>
        </div>
      </Layout>
    );
  }

  const orderStatusInfo = ORDER_STATUS[order.status] ?? {
    label: "Unknown",
    className: "bg-gray-100 text-gray-600",
  };
  const isPending = order.status === 0;

  return (
    <Layout>
      <div className="max-w-2xl mx-auto">
        {/* Back */}
        <button
          onClick={() => navigate("/settings/order-history")}
          className="flex items-center gap-1 text-gray-500 hover:text-gray-800 mb-6 transition-colors"
        >
          <i className="material-symbols-rounded text-lg">arrow_back</i>
          <span className="text-sm">Back to Order History</span>
        </button>

        {/* Order Details Card */}
        <div className="border border-gray-200 rounded-xl bg-white p-6 mb-6">
          <div className="flex items-start justify-between mb-4">
            <div>
              <h1 className="text-xl font-bold text-gray-900">
                {order.subscription?.name ?? "Unknown Plan"}
              </h1>
              <p className="text-xs text-gray-400 mt-1">
                Order #{order.orderId}
              </p>
            </div>
            <span
              className={`text-xs font-medium px-3 py-1 rounded-full ${orderStatusInfo.className}`}
            >
              {orderStatusInfo.label}
            </span>
          </div>

          <div className="grid grid-cols-2 gap-y-3 text-sm">
            <span className="text-gray-500">Plan</span>
            <span className="text-gray-900 font-medium">
              {order.subscription?.name}
            </span>

            <span className="text-gray-500">Duration</span>
            <span className="text-gray-900 font-medium">
              {order.subscription?.durationInMonths === 0
                ? "Lifetime"
                : `${order.subscription?.durationInMonths} months`}
            </span>

            <span className="text-gray-500">Amount</span>
            <span className="text-gray-900 font-bold">
              Rs. {order.amount.toLocaleString()}
            </span>

            <span className="text-gray-500">Created</span>
            <span className="text-gray-900">{formatDate(order.createdAt)}</span>

            {order.updatedAt && (
              <>
                <span className="text-gray-500">Last Updated</span>
                <span className="text-gray-900">
                  {formatDate(order.updatedAt)}
                </span>
              </>
            )}
          </div>

          {/* Actions for pending orders */}
          {isPending && (
            <div className="border-t border-gray-100 mt-5 pt-5">
              <p className="text-xs text-gray-500 mb-3">
                Complete your payment:
              </p>
              <div className="flex items-center gap-3 flex-wrap mb-3">
                {PROVIDERS.map((p) => (
                  <button
                    key={p.value}
                    onClick={() => setSelectedProvider(p.value)}
                    className={`px-4 py-1.5 rounded-lg border text-sm font-medium transition-all ${
                      selectedProvider === p.value
                        ? "border-black bg-black text-white"
                        : "border-gray-300 text-gray-700 hover:border-gray-500"
                    }`}
                  >
                    {p.label}
                  </button>
                ))}

                <button
                  onClick={handleRetryPayment}
                  disabled={isInitiating}
                  className="ml-auto flex items-center gap-2 px-4 py-1.5 rounded-lg bg-black text-white text-sm font-medium disabled:opacity-60"
                >
                  {isInitiating ? (
                    <>
                      <div className="w-3.5 h-3.5 border-2 border-gray-400 border-t-white rounded-full animate-spin" />
                      Processing...
                    </>
                  ) : (
                    <>
                      <i className="material-symbols-rounded text-base">
                        payment
                      </i>
                      Pay Now
                    </>
                  )}
                </button>
              </div>
              {initError && <p className="text-red-500 text-xs">{initError}</p>}

              {/* Cancel */}
              {!showCancelConfirm ? (
                <button
                  onClick={() => setShowCancelConfirm(true)}
                  className="mt-2 text-sm text-red-500 hover:text-red-700 underline"
                >
                  Cancel this order
                </button>
              ) : (
                <div className="mt-3 flex items-center gap-3 flex-wrap">
                  <p className="text-sm text-gray-700">
                    Are you sure you want to cancel?
                  </p>
                  <button
                    onClick={handleCancel}
                    disabled={isCanceling}
                    className="flex items-center gap-1.5 px-3 py-1 rounded-lg bg-red-500 text-white text-sm font-medium disabled:opacity-60"
                  >
                    {isCanceling ? (
                      <>
                        <div className="w-3 h-3 border-2 border-red-300 border-t-white rounded-full animate-spin" />
                        Canceling...
                      </>
                    ) : (
                      "Yes, Cancel"
                    )}
                  </button>
                  <button
                    onClick={() => setShowCancelConfirm(false)}
                    className="text-sm text-gray-500 hover:text-gray-800"
                  >
                    Keep Order
                  </button>
                  {cancelError && (
                    <p className="text-red-500 text-xs w-full">{cancelError}</p>
                  )}
                </div>
              )}
            </div>
          )}
        </div>

        {/* Payments Section */}
        <h2 className="text-lg font-semibold text-gray-900 mb-3">
          Payment Attempts
        </h2>

        {payments.length === 0 ? (
          <div className="border border-gray-200 rounded-xl bg-white p-6 text-center text-gray-400 text-sm">
            No payment attempts for this order.
          </div>
        ) : (
          <div className="flex flex-col gap-3">
            {payments.map((payment) => {
              const paymentStatusInfo = PAYMENT_STATUS[payment.status] ?? {
                label: "Unknown",
                className: "bg-gray-100 text-gray-600",
              };
              const canRetry = payment.status === 0 || payment.status === 2;
              const retryState = paymentRetry[payment.paymentId];

              return (
                <div
                  key={payment.paymentId}
                  className="border border-gray-200 rounded-xl bg-white p-5"
                >
                  <div className="flex items-start justify-between mb-3">
                    <div>
                      <p className="font-medium text-gray-900">
                        {PROVIDER_NAMES[payment.provider] ??
                          `Provider ${payment.provider}`}
                      </p>
                      <p className="text-xs text-gray-400 mt-0.5">
                        Payment #{payment.paymentId} &middot;{" "}
                        {formatDate(payment.createdAt)}
                      </p>
                    </div>
                    <div className="flex items-center gap-3 shrink-0">
                      <span className="text-base font-bold text-gray-900">
                        Rs. {payment.amount.toLocaleString()}
                      </span>
                      <span
                        className={`text-xs font-medium px-2.5 py-1 rounded-full ${paymentStatusInfo.className}`}
                      >
                        {paymentStatusInfo.label}
                      </span>
                    </div>
                  </div>

                  {payment.externalTransactionId && (
                    <p className="text-xs text-gray-400 font-mono truncate mb-3">
                      Txn: {payment.externalTransactionId}
                    </p>
                  )}

                  {canRetry && (
                    <div className="border-t border-gray-100 pt-3">
                      <button
                        onClick={() => handlePaymentRetry(payment)}
                        disabled={retryState?.isLoading}
                        className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-black text-white text-sm font-medium disabled:opacity-60"
                      >
                        {retryState?.isLoading ? (
                          <>
                            <div className="w-3.5 h-3.5 border-2 border-gray-400 border-t-white rounded-full animate-spin" />
                            Redirecting...
                          </>
                        ) : (
                          <>
                            <i className="material-symbols-rounded text-base">
                              refresh
                            </i>
                            Retry with {PROVIDER_NAMES[payment.provider]}
                          </>
                        )}
                      </button>
                      {retryState?.error && (
                        <p className="text-red-500 text-xs mt-2">
                          {retryState.error}
                        </p>
                      )}
                    </div>
                  )}
                </div>
              );
            })}
          </div>
        )}
      </div>
    </Layout>
  );
};

export default OrderDetail;
