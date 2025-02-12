import PropTypes from 'prop-types';

const Button = ({ text, onClick, icon, iconSize, className }) => {
  return (
    <button 
      className={`bg-gray-800 p-2 rounded flex items-center justify-center space-x-2 text-white hover:bg-gray-700 ${className}`} 
      onClick={onClick}
    >
      {icon && <i className="material-symbols-rounded" style={{ fontSize: iconSize }}>{icon}</i>}
      {text}
    </button>
  );
};

Button.propTypes = {
  text: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  icon: PropTypes.string,
  iconSize: PropTypes.string,
  className: PropTypes.string,
};

export default Button;
