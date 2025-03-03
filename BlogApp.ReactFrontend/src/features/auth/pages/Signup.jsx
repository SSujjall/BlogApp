import CommonInputField from "../../../components/common/CommonInputField";
import Button from "../../../components/common/Button";
import { Link } from "react-router-dom";
import { useState } from "react";

const initFieldValues = {
  username: "",
  email: "",
  phone: "",
  password: "",
};

const Signup = () => {
  const [values, setValues] = useState(initFieldValues);

  const handleFieldChange = (e) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  return (
    <div className="min-h-screen flex justify-center items-center">
      <Link to={"/"}>
        <Button
          text="Home"
          icon={"keyboard_backspace"}
          className="fixed top-5 left-5"
        />
      </Link>

      <form className="border shadow-md p-4 rounded-lg sm:min-w-96 transition-transform">
        <h1 className="text-3xl text-center mb-5">Signup</h1>
        <CommonInputField
          type={"text"}
          icon={"person"}
          placeholder={"Username"}
          classProp={"py-3 mb-3"}
          name={"username"}
          onChange={handleFieldChange}
          value={values.username}
        />
        <CommonInputField
          type={"text"}
          icon={"mail"}
          placeholder={"Email"}
          classProp={"py-3 mb-3"}
          name={"email"}
          onChange={handleFieldChange}
          value={values.email}
        />
        <CommonInputField
          type={"text"}
          icon={"call"}
          placeholder={"Phone"}
          classProp={"py-3 mb-3"}
          name={"phone"}
          onChange={handleFieldChange}
          value={values.phone}
        />
        <CommonInputField
          type={"password"}
          icon={"password"}
          placeholder={"Password"}
          classProp={"py-3 mb-3"}
          name={"password"}
          onChange={handleFieldChange}
          value={values.password}
        />
        <CommonInputField
          type={"password"}
          icon={"password"}
          placeholder={"Confirm Password"}
          classProp={"py-3 mb-3"}
        />

        <Button
          text="Signup"
          className={"bg-black text-white hover:bg-gray-700 w-full py-3 mt-5"}
          onClick={(e) => {
            e.preventDefault(),
              console.log("Signup button clicked, values: ", values);
          }}
        />

        <p className="mt-3">
          Already have an account?&nbsp;
          <Link to="/login" className="text-blue-500 hover:underline">
            Login
          </Link>
        </p>

        {/* OR separator */}
        <div className="relative flex items-center my-4">
          <hr className="w-full border-gray-300" />
          <span className="absolute left-1/2 transform -translate-x-1/2 bg-white px-2 text-gray-500 font-semibold text-xs">
            OR
          </span>
        </div>

        <Button
          text="Signup With Google"
          className={
            "w-full border border-blue-500 text-blue-500 hover:bg-blue-500 hover:text-white py-3"
          }
        />
      </form>
    </div>
  );
};

export default Signup;
