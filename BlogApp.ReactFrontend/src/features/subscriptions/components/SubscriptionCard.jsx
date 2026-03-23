import PropTypes from "prop-types";
import { useNavigate } from "react-router-dom";

const SubscriptionCard = ({ subscription }) => {
  const { name, description, price, durationInMonths } = subscription;
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/subscriptions/order/${subscription.subscriptionId}`, {
      state: { subscription },
    });
  };

  return (
    <div
      onClick={handleClick}
      className="border border-gray-200 rounded-xl p-6 flex flex-col gap-4 shadow-sm hover:shadow-md hover:border-gray-400 transition-all cursor-pointer bg-white">
      <div className="flex items-center justify-between">
        <h2 className="text-xl font-semibold text-gray-900">{name}</h2>
        <span className="bg-black text-white text-sm font-medium px-3 py-1 rounded-full">
          {durationInMonths === 0 ? "Lifetime" : `${durationInMonths} months`}
        </span>
      </div>

      {description && (
        <p className="text-gray-600 text-sm leading-relaxed">{description}</p>
      )}

      <div className="mt-auto pt-2 border-t border-gray-100">
        <span className="text-2xl font-bold text-gray-900">
          {price === 0 ? "Free" : `Rs. ${price.toLocaleString()}`}
        </span>
      </div>
    </div>
  );
};

SubscriptionCard.propTypes = {
  subscription: PropTypes.shape({
    subscriptionId: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string,
    price: PropTypes.number.isRequired,
    durationInMonths: PropTypes.number.isRequired,
  }).isRequired,
};

export default SubscriptionCard;
