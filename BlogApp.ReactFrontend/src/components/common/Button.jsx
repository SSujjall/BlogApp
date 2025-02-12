import PropTypes from "prop-types";

const Button = ({ text, onClick, icon, iconSize, className }) => {
  return (
    <button
      className={`p-2 rounded flex items-center justify-center gap-1 transition-colors ${className} ${icon ? "pr-3" : ""}`} // add extra padding if there is icon
      onClick={onClick}
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
  text: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  icon: PropTypes.string,
  iconSize: PropTypes.number,
  className: PropTypes.string,
};

export default Button;
