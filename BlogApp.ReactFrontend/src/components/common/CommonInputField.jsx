import { useState } from "react";
import PropTypes from "prop-types";

const CommonInputField = ({
  name,
  type,
  placeholder,
  icon,
  value,
  onChange,
  classProp,
  isRequired = true,
}) => {
  const [isPasswordShown, setIsPasswordShown] = useState(false);

  return (
    <div
      className={`${classProp} px-2 py-2 flex bg-white items-center w-full border rounded-md focus-within:border-black transition-colors`}
    >
      {icon && (
        <span
          className="text-gray-500 flex items-center"
          style={{ userSelect: "none" }}
        >
          <i className="material-symbols-rounded leading-none"> {icon} </i>
        </span>
      )}

      <input
        name={name}
        type={isPasswordShown ? "text" : type}
        placeholder={placeholder}
        className="flex-1 outline-none px-2 h-full text-base w-full"
        value={value}
        onChange={onChange}
        {...isRequired && { required: true }}
      />

      {type === "password" && (
        <span
          onClick={() => setIsPasswordShown((prevState) => !prevState)}
          className="cursor-pointer text-gray-500 flex items-center"
        >
          <i
            className="material-symbols-rounded leading-none text-xl"
            style={{ userSelect: "none" }}
          >
            {isPasswordShown ? "visibility" : "visibility_off"}
          </i>
        </span>
      )}
    </div>
  );
};

CommonInputField.propTypes = {
  name: PropTypes.string,
  type: PropTypes.string,
  onChange: PropTypes.func,
  placeholder: PropTypes.string,
  icon: PropTypes.string,
  value: PropTypes.string,
  classProp: PropTypes.string,
  isRequired: PropTypes.bool,
};

export default CommonInputField;
