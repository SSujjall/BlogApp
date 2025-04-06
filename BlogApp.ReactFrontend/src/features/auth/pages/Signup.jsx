import CommonInputField from "../../../components/common/CommonInputField";
import Button from "../../../components/common/Button";
import { Link } from "react-router-dom";
import { useState } from "react";
import { signup } from "../service/signupService";
import {
  showSuccessToast,
  showErrorToast,
} from "../../../common/utils/toastHelper";

const initFieldValues = {
  username: "",
  email: "",
  phone: "",
  password: "",
};

const Signup = () => {
  const [values, setValues] = useState(initFieldValues);
  const [isLoading, setIsLoading] = useState(false);

  const handleFieldChange = (e) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const handleSignup = async () => {
    const payload = {
      username: values.username,
      email: values.email,
      password: values.password,
    };

    setIsLoading(true);
    try {
      const apiResponse = await signup(payload);
      if (apiResponse.statusCode === 200) {
        showSuccessToast(apiResponse.message);
      } else {
        let errorMessage = apiResponse.message;
        if (apiResponse.errors) {
          const errorMessages = Object.values(apiResponse.errors).join(" ");
          errorMessage = `${apiResponse.message} ${errorMessages}`;
        }
        showErrorToast(errorMessage);
      }
    } catch {
      showErrorToast("Error Occured");
    } finally {
      setIsLoading(false);
    }
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
          onClick={handleSignup}
          disabled={isLoading}
          isLoading={isLoading}
        />
        {isLoading && (
          <div className="h-5 w-5 border-4 m-auto mt-2 border-t-black rounded-full animate-spin"></div>
        )}

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
