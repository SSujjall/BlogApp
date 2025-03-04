import CommonInputField from "../../../components/common/CommonInputField";
import Button from "../../../components/common/Button";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { GoogleLogin, GoogleOAuthProvider } from "@react-oauth/google";
import { loginWithGoogle } from "../service/loginService";
import { setTokens } from "../../../common/utils/tokenHelper";
import {
  showSuccessToast,
  showErrorToast,
} from "../../../common/utils/toastHelper";

{
  /* FOR GOOGLE LOGIN:
  #Enable "Google People API" 
  -Go to APIs & Services > Library.
  -Search for "Google People API" (needed for profile/email access).
  -Click Enable.

  #Configure OAuth Client
  In Authorized JavaScript Origins, add your react running url (http) http://localhost:5173
  (Add your production domain if deployed)
  In Authorized Redirect URIs do the same as above.
*/
}

const Login = () => {
  const [isButtonDisabled, setIsButtonDisabled] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;
  const navigate = useNavigate();

  const handleLogin = () => {
    console.log("Login clicked");
    setIsButtonDisabled(true);
    setIsLoading(true);
    // Set a timer to re-enable the button after 3 seconds
    setTimeout(() => {
      setIsButtonDisabled(false);
      setIsLoading(false);
    }, 3000);
  };

  const handleGoogleLogin = async (credentialResponse) => {
    const googleToken = credentialResponse.credential;
    console.log("Google Token:", googleToken);

    const response = await loginWithGoogle(googleToken);
    if (response) {
      setTokens(response.data.jwtToken, response.data.refreshToken);
      showSuccessToast("Logged in successfully");
      navigate("/");
      window.location.reload();
    } else {
      showErrorToast("Google Login Failed");
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
        <h1 className="text-3xl text-center mb-5">Login</h1>
        <CommonInputField
          type={"text"}
          icon={"person"}
          placeholder={"Username or email"}
          classProp={"py-3 mb-3"}
        />
        <CommonInputField
          type={"password"}
          icon={"password"}
          placeholder={"Password"}
          classProp={"py-3 mb-3"}
        />

        <div className="flex justify-between">
          <div className="flex items-center text-gray-500 hover:text-black transition-all">
            <input id="rememberMe" type="checkbox" className="h-4 w-4 mr-2" />
            <label htmlFor="rememberMe" className="select-none">
              Remember me
            </label>
          </div>
          <Link
            to={"/forgot-password"}
            className="font-semibold text-gray-500 hover:text-black hover:underline"
          >
            Forgot password?
          </Link>
        </div>

        <button
          className="bg-black text-white rounded hover:bg-gray-700 w-full py-3 mt-5 flex items-center justify-center"
          onClick={handleLogin}
          disabled={isButtonDisabled}
        >
          {isLoading && (
            <div className="w-5 h-5 mr-2 border-4 border-t-4 border-gray-300 border-t-white rounded-full animate-spin"></div>
          )}
          {isLoading ? "Logging in..." : "Login"}
        </button>

        <p className="mt-3">
          Don&apos;t have an account?&nbsp;
          <Link to="/signup" className="text-blue-500 hover:underline">
            Signup
          </Link>
        </p>

        {/* OR separator */}
        <div className="relative flex items-center my-4">
          <hr className="w-full border-gray-300" />
          <span className="absolute left-1/2 transform -translate-x-1/2 bg-white px-2 text-gray-500 font-semibold text-xs">
            OR
          </span>
        </div>

        {/* Google Login Button with provider */}
        <GoogleOAuthProvider clientId={clientId}>
          <GoogleLogin
            onSuccess={handleGoogleLogin}
            onError={() => console.log("Google Login Failed")}
          />
        </GoogleOAuthProvider>
      </form>
    </div>
  );
};

export default Login;
