import { useState } from "react";
import PropTypes from "prop-types";

const CommonInputField = ({
  type,
  placeholder,
  icon,
  value,
  onChange,
  classProp,
}) => {
  const [isPasswordShown, setIsPasswordShown] = useState(false);

  return (
    <div className="relative">
      <input
        type={isPasswordShown ? "text" : type}
        placeholder={placeholder}
        className={`w-full p-2 pl-10 pr-12 rounded-md focus:outline-none ${classProp}`}
        value={value}
        onChange={onChange}
        required
      />
      {icon && (
        <i
          className="material-symbols-rounded absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500"
          style={{ userSelect: "none" }}
        >
          {icon}
        </i>
      )}

      {type === "password" && (
        <i
          onClick={() => setIsPasswordShown((prevState) => !prevState)}
          className="material-symbols-rounded absolute right-3 top-1/2 transform -translate-y-1/2 cursor-pointer text-gray-500"
          style={{ userSelect: "none" }}
        >
          {isPasswordShown ? "visibility" : "visibility_off"}
        </i>
      )}
    </div>
  );
};

CommonInputField.propTypes = {
  type: PropTypes.string,
  onChange: PropTypes.func,
  placeholder: PropTypes.string,
  icon: PropTypes.string,
  value: PropTypes.string,
  classProp: PropTypes.string,
};

export default CommonInputField;
