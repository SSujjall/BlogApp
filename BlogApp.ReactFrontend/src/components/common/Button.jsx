import PropTypes from "prop-types";

const Button = ({ text, onClick, icon, iconSize, className, disabled }) => {
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
};

export default Button;
