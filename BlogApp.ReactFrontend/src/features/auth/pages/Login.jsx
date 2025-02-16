import CommonInputField from "../../../components/common/CommonInputField"
import Button from "../../../components/common/Button"
import { Link } from "react-router-dom"

const Login = () => {
  return (
    <div className="min-h-screen flex justify-center items-center">
      <Link to={"/"}><Button text="Home" icon={"keyboard_backspace"} className="fixed top-5 left-5" /></Link>
      
      <form className="border shadow-md p-4 rounded-lg sm:min-w-96 transition-transform">
        <h1 className="text-3xl text-center mb-5">Login</h1>
        <CommonInputField type={"text"} icon={"person"} placeholder={"Username or email"} classProp={"py-3 mb-3"} />
        <CommonInputField type={"password"} icon={"password"} placeholder={"Password"} classProp={"py-3 mb-3"} />
        <Link to={"/forgot-password"} className="font-semibold text-gray-500 hover:text-black hover:underline">Forgot password?</Link>

        <Button text="Login" className={"bg-black text-white hover:bg-gray-700 w-full py-3 mt-5"} />

        <p className="mt-3">Don&apos;t have an account?&nbsp;<Link to="/signup" className="text-blue-500 hover:underline">Signup</Link></p>

        {/* OR separator */}
        <div className="relative flex items-center my-4">
          <hr className="w-full border-gray-300" />
          <span className="absolute left-1/2 transform -translate-x-1/2 bg-white px-2 text-gray-500 font-semibold text-xs">OR</span>
        </div>

        <Button text="Login With Google" className={"w-full border border-blue-500 text-blue-500 hover:bg-blue-500 hover:text-white py-3"} />
      </form>
    </div>
  )
}

export default Login