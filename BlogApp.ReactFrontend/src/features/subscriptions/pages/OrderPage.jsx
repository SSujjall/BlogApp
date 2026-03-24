import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Layout from "../../../components/layout/Layout";
import Button from "../../../components/common/Button";
import { createOrder } from "../service/orderService";
import { initiatePayment } from "../service/paymentService";

const PROVIDERS = [
  { label: "eSewa", value: 1 },
  { label: "Khalti", value: 2 },
];

const OrderPage = () => {
  const { state } = useLocation();
  const navigate = useNavigate();
  const subscription = state?.subscription;

  const [provider, setProvider] = useState(1);
  const [isProcessing, setIsProcessing] = useState(false);
  const [error, setError] = useState(null);

  if (!subscription) {
    return (
      <Layout>
        <div className="flex flex-col justify-center items-center min-h-[50vh] gap-4">
          <p className="text-lg text-gray-600">
            Subscription details not found.
          </p>
          <Button
            text="Back to Subscriptions"
            onClick={() => navigate("/subscriptions")}
            className="bg-black text-white"
          />
        </div>
      </Layout>
    );
  }

  const handlePay = async () => {
    setIsProcessing(true);
    setError(null);

    try {
      // Step 1: Create order
      const orderRes = await createOrder(subscription.subscriptionId);
      if (!orderRes?.status) {
        setError(orderRes?.message || "Failed to create order.");
        return;
      }
      const orderId = orderRes.data.orderId;

      // Step 2: Initiate payment
      const paymentRes = await initiatePayment(orderId, provider);
      if (!paymentRes?.status) {
        setError(paymentRes?.message || "Failed to initiate payment.");
        return;
      }

      const { redirectUrl, externalTxnId } = paymentRes.data;

      // Store pending payment info for use after redirect
      localStorage.setItem(
        "pendingPayment",
        JSON.stringify({ externalTxnId, provider }),
      );

      // Step 3: Redirect to payment gateway
      window.location.href = redirectUrl;
    } catch {
      setError("Something went wrong. Please try again.");
    } finally {
      setIsProcessing(false);
    }
  };

  return (
    <Layout>
      <div className="max-w-lg mx-auto">
        {/* Back link */}
        <button
          onClick={() => navigate("/subscriptions")}
          className="flex items-center gap-1 text-gray-500 hover:text-gray-800 mb-6 transition-colors"
        >
          <i className="material-symbols-rounded text-lg">arrow_back</i>
          <span className="text-sm">Back to Plans</span>
        </button>

        <h1 className="text-2xl font-bold text-gray-900 mb-6">
          Complete Your Order
        </h1>

        {/* Subscription Summary */}
        <div className="border border-gray-200 rounded-xl p-6 mb-6 bg-white">
          <h2 className="text-sm font-medium text-gray-500 uppercase tracking-wide mb-4">
            Order Summary
          </h2>
          <div className="flex items-start justify-between mb-3">
            <div>
              <p className="text-lg font-semibold text-gray-900">
                {subscription.name}
              </p>
              {subscription.description && (
                <p className="text-sm text-gray-500 mt-1">
                  {subscription.description}
                </p>
              )}
            </div>
            <span className="bg-gray-100 text-gray-700 text-xs font-medium px-2.5 py-1 rounded-full ml-4 shrink-0">
              {subscription.durationInMonths === 0
                ? "Lifetime"
                : `${subscription.durationInMonths} months`}
            </span>
          </div>
          <div className="border-t border-gray-100 pt-3 mt-3 flex items-center justify-between">
            <span className="text-gray-500 text-sm">Total</span>
            <span className="text-2xl font-bold text-gray-900">
              {subscription.price === 0
                ? "Free"
                : `Rs. ${subscription.price.toLocaleString()}`}
            </span>
          </div>
        </div>

        {/* Payment Provider */}
        <div className="border border-gray-200 rounded-xl p-6 mb-6 bg-white">
          <h2 className="text-sm font-medium text-gray-500 uppercase tracking-wide mb-4">
            Payment Method
          </h2>
          <div className="flex gap-3">
            {PROVIDERS.map((p) => (
              <button
                key={p.value}
                onClick={() => setProvider(p.value)}
                className={`flex-1 py-3 px-4 rounded-lg border-2 font-medium transition-all ${
                  provider === p.value
                    ? "border-black bg-black text-white"
                    : "border-gray-200 text-gray-700 hover:border-gray-400"
                }`}
              >
                {p.label}
              </button>
            ))}
          </div>
        </div>

        {/* Error */}
        {error && <p className="text-red-500 text-sm mb-4">{error}</p>}

        {/* Pay Button */}
        <Button
          text={`Pay with ${PROVIDERS.find((p) => p.value === provider)?.label}`}
          onClick={handlePay}
          disabled={isProcessing}
          isLoading={isProcessing}
          className="w-full bg-black text-white py-3 text-base font-semibold"
        />
      </div>
    </Layout>
  );
};

export default OrderPage;
