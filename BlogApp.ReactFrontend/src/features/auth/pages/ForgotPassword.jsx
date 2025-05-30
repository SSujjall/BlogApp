import CommonInputField from "../../../components/common/CommonInputField";
import Button from "../../../components/common/Button";
import { useNavigate } from "react-router-dom";

const ForgotPassword = () => {
  const navigate = useNavigate();

  const handleBackClick = () => {
    navigate(-1); // Go back one step in history
  };

  return (
    <div className="min-h-screen flex justify-center items-center">
      <Button
        text="Back"
        icon={"keyboard_backspace"}
        className="fixed top-5 left-5"
        onClick={handleBackClick}
      />

      <form className="text-center border shadow-md p-4 rounded-lg sm:min-w-96 transition-transform">
        <h1 className="text-3xl mb-2">Forgot Password</h1>
        <p className="mb-5 text-gray-500">
          Enter your email address, we will send you reset link.
        </p>
        <CommonInputField
          type={"text"}
          icon={"person"}
          placeholder={"Email"}
          classProp={"py-3 mb-3"}
        />
        <Button
          text="Send Link"
          className={"bg-black text-white hover:bg-gray-700 w-full py-3 mt-5"}
        />
      </form>
    </div>
  );
};

export default ForgotPassword;
