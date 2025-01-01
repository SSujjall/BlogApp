import PropTypes from "prop-types";

Header.propTypes = {
  title: PropTypes.string.isRequired,
};

const Header = ({ title }) => {
  return <div>{title}</div>;
};

export default Header;
