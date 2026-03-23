import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Layout from "../../../components/layout/Layout";
import { getUserOrders } from "../service/orderService";

const ORDER_STATUS = {
  0: { label: "Pending", className: "bg-yellow-100 text-yellow-700" },
  1: { label: "Completed", className: "bg-green-100 text-green-700" },
  2: { label: "Canceled", className: "bg-red-100 text-red-600" },
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const OrderHistory = () => {
  const [orders, setOrders] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrders = async () => {
      setIsLoading(true);
      try {
        const res = await getUserOrders();
        const data = Array.isArray(res?.data) ? res.data : [];
        setOrders([...data].reverse());
      } catch {
        setError("Failed to load order history.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchOrders();
  }, []);

  if (isLoading) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Loading order history...</p>
        </div>
      </Layout>
    );
  }

  if (error) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-red-500">{error}</p>
        </div>
      </Layout>
    );
  }

  if (orders.length === 0) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">No orders found.</p>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900 mb-1">Order History</h1>
        <p className="text-gray-500 text-sm">
          Click an order to view details and manage payment.
        </p>
      </div>

      <div className="flex flex-col gap-3">
        {orders.map((order) => {
          const statusInfo = ORDER_STATUS[order.status] ?? {
            label: "Unknown",
            className: "bg-gray-100 text-gray-600",
          };
          const isPending = order.status === 0;

          return (
            <div
              key={order.orderId}
              onClick={() =>
                navigate(`/settings/order-detail/${order.orderId}`)
              }
              className="border border-gray-200 rounded-xl bg-white p-5 cursor-pointer hover:border-gray-400 hover:shadow-sm transition-all"
            >
              <div className="flex items-start justify-between gap-4">
                <div className="min-w-0">
                  <p className="font-semibold text-gray-900 truncate">
                    {order.subscription?.name ?? "Unknown Plan"}
                  </p>
                  <p className="text-xs text-gray-400 mt-0.5">
                    Order #{order.orderId} &middot;{" "}
                    {formatDate(order.createdAt)}
                  </p>
                  <p className="text-sm text-gray-500 mt-1">
                    {order.subscription?.durationInMonths === 0
                      ? "Lifetime"
                      : `${order.subscription?.durationInMonths} months`}
                  </p>
                </div>

                <div className="flex flex-col items-end gap-2 shrink-0">
                  <span className="text-base font-bold text-gray-900">
                    Rs. {order.amount.toLocaleString()}
                  </span>
                  <span
                    className={`text-xs font-medium px-2.5 py-1 rounded-full ${statusInfo.className}`}
                  >
                    {statusInfo.label}
                  </span>
                  {isPending && (
                    <span className="text-xs text-blue-500 flex items-center gap-0.5">
                      <i className="material-symbols-rounded text-sm">
                        payment
                      </i>
                      Payment required
                    </span>
                  )}
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </Layout>
  );
};

export default OrderHistory;
