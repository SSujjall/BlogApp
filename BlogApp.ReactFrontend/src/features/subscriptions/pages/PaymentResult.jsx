import { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import Layout from "../../../components/layout/Layout";
import Button from "../../../components/common/Button";
import { verifyPayment } from "../service/paymentService";

const PaymentResult = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const [status, setStatus] = useState("verifying"); // "verifying" | "success" | "failed"
  const [message, setMessage] = useState("");

  useEffect(() => {
    const verify = async () => {
      const esewaData = searchParams.get("data");
      const khaltiPidx = searchParams.get("pidx");

      const pending = JSON.parse(
        localStorage.getItem("pendingPayment") || "null",
      );
      localStorage.removeItem("pendingPayment");

      let externalTxnId = "";
      let data = "";

      if (esewaData) {
        // eSewa: use stored externalTxnId, send data from URL
        externalTxnId = pending?.externalTxnId || "";
        data = esewaData;
      } else if (khaltiPidx) {
        // Khalti: pidx from URL is the externalTxnId, data is blank
        externalTxnId = khaltiPidx;
        data = "";
      } else {
        setStatus("failed");
        setMessage("No payment data found in the callback URL.");
        return;
      }

      const res = await verifyPayment(externalTxnId, data);

      if (res?.status && res?.data === true) {
        setStatus("success");
        setMessage(res.message || "Payment verified successfully!");
      } else {
        setStatus("failed");
        setMessage(res?.message || "Payment verification failed.");
      }
    };

    verify();
  }, []);

  return (
    <Layout>
      <div className="flex flex-col items-center justify-center min-h-[60vh] text-center">
        {status === "verifying" && (
          <>
            <div className="w-12 h-12 mb-4 border-4 border-gray-200 border-t-black rounded-full animate-spin" />
            <p className="text-lg text-gray-600">Verifying your payment...</p>
          </>
        )}

        {status === "success" && (
          <>
            <div className="w-16 h-16 rounded-full bg-green-100 flex items-center justify-center mb-4">
              <i
                className="material-symbols-rounded text-green-600"
                style={{ fontSize: 36 }}
              >
                check_circle
              </i>
            </div>
            <h1 className="text-2xl font-bold text-gray-900 mb-2">
              Payment Successful
            </h1>
            <p className="text-gray-500 mb-6">{message}</p>
            <Button
              text="Go to Home"
              onClick={() => navigate("/")}
              className="bg-black text-white px-6"
            />
          </>
        )}

        {status === "failed" && (
          <>
            <div className="w-16 h-16 rounded-full bg-red-100 flex items-center justify-center mb-4">
              <i
                className="material-symbols-rounded text-red-500"
                style={{ fontSize: 36 }}
              >
                cancel
              </i>
            </div>
            <h1 className="text-2xl font-bold text-gray-900 mb-2">
              Payment Failed
            </h1>
            <p className="text-gray-500 mb-6">{message}</p>
            <Button
              text="Try Again"
              onClick={() => navigate("/subscriptions")}
              className="bg-black text-white px-6"
            />
          </>
        )}
      </div>
    </Layout>
  );
};

export default PaymentResult;
