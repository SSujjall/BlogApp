import PropTypes from "prop-types";

const Button = ({ text, onClick, icon, iconSize, className, disabled, isLoading }) => {
  return (
    <button
      className={`p-2 rounded flex items-center justify-center gap-1 transition-colors ${className} ${icon ? "pr-3" : ""}`} // add extra padding if there is icon
      onClick={onClick}
      disabled={disabled}
    >
      {icon && (
        <i
          className="material-symbols-rounded"
          style={{ fontSize: iconSize, userSelect: "none" }}
        >
          {icon}
        </i>
      )}
      {text}

      {isLoading && (
        <div className="w-5 h-5 ml-2 border-4 border-t-4 border-gray-300 border-t-white rounded-full animate-spin"></div>
      )}
    </button>
  );
};

Button.propTypes = {
  text: PropTypes.string,
  onClick: PropTypes.func,
  icon: PropTypes.string,
  iconSize: PropTypes.number,
  className: PropTypes.string,
  disabled: PropTypes.bool,
  isLoading: PropTypes.bool,
};

export default Button;
