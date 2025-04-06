import CommonInputField from "../../../components/common/CommonInputField";
import Button from "../../../components/common/Button";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { GoogleLogin, GoogleOAuthProvider } from "@react-oauth/google";
import { login, loginWithGoogle } from "../service/loginService";
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

const initFieldValues = {
  username: "",
  email: ""
};

const Login = () => {
  const [values, setValues] = useState(initFieldValues);
  const [isLoading, setIsLoading] = useState(false);
  const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;
  const navigate = useNavigate();

  const handleFieldChange = (e) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const handleLogin = async () => {
    const payload = {
      username: values.username,
      password: values.password,
    };

    try {
      setIsLoading(true);

      const apiResponse = await login(payload);
      if (apiResponse.statusCode === 200) {
        setTokens(apiResponse.data.jwtToken, apiResponse.data.refreshToken);
        navigate("/");
        window.location.reload();
        showSuccessToast(apiResponse.message);
      } else {
        let errorMessage = apiResponse.message;
        if (apiResponse.errors) {
          const errorMessages = Object.values(apiResponse.errors).join(" ");
          errorMessage = `${apiResponse.message}. ${errorMessages}`;
        }
        showErrorToast(errorMessage);
      }
    } catch {
      showErrorToast("Error Logging In");
    } finally {
      setIsLoading(false);
    }
  };

  const handleGoogleLogin = async (credentialResponse) => {
    const googleToken = credentialResponse.credential;
    // console.log("Google Token:", googleToken);

    const response = await loginWithGoogle(googleToken);
    if (response) {
      setTokens(response.data.jwtToken, response.data.refreshToken);
      navigate("/");
      // window.location.reload();
      showSuccessToast("Logged in successfully");
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
          name={"username"}
          onChange={handleFieldChange}
          value={values.username}
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

        <Button 
          onClick={handleLogin}
          disabled={isLoading}
          className={"bg-black text-white rounded hover:bg-gray-700 w-full py-3 mt-5 flex items-center justify-center"}
          text={isLoading ? "Logging in..." : "Login"}
          isLoading={isLoading}
        />

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
            logo_alignment="center"
            auto_select={false}
          />
        </GoogleOAuthProvider>
      </form>
    </div>
  );
};

export default Login;
