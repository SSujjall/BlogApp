import PropTypes from "prop-types";

Button.propTypes = {
  label: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  type: PropTypes.string,
};

const Button = ({ label, onClick, type = "button" }) => {
  return (
    <button type={type} onClick={onClick} className="">
      {label}
    </button>
  );
};

export default Button;