import { useState, useEffect } from "react";
import Layout from "../../../components/layout/Layout";
import SubscriptionCard from "../components/SubscriptionCard";
import { getAllSubscriptions } from "../service/subscriptionService";

const Subscriptions = () => {
  const [subscriptions, setSubscriptions] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchSubscriptions = async () => {
      setIsLoading(true);
      try {
        const res = await getAllSubscriptions();
        setSubscriptions(Array.isArray(res?.data) ? res.data : []);
      } catch {
        setError("Failed to load subscriptions.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchSubscriptions();
  }, []);

  if (isLoading) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Loading subscriptions...</p>
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

  if (subscriptions.length === 0) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">
            No subscription plans available.
          </p>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">
          Subscription Plans
        </h1>
        <p className="text-gray-500">Choose a plan that works best for you.</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {subscriptions.map((subscription) => (
          <SubscriptionCard
            key={subscription.subscriptionId}
            subscription={subscription}
          />
        ))}
      </div>
    </Layout>
  );
};

export default Subscriptions;
