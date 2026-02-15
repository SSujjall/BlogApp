import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Button from "../../../components/common/Button";
import CommonInputField from "../../../components/common/CommonInputField";
import { resendVerificationEmail } from "../service/signupService";
import {
	showSuccessToast,
	showErrorToast,
	closeAllToasts
} from "../../../common/utils/toastHelper";


const ResendVerification = () => {
	const [isLoading, setIsLoading] = useState(false);
	const [email, setEmail] = useState("");
	
	useEffect(() => {
		closeAllToasts();
	}, [])

	const handleResendVerification = async (e) => {
		e.preventDefault();

		try {
			setIsLoading(true);

			const apiResponse = await resendVerificationEmail({email: email});
			showSuccessToast(apiResponse.message);
		} catch {
      showErrorToast("Error sending verification mail.");
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

			<form
				onSubmit={handleResendVerification}
				className="border shadow-md p-4 rounded-lg sm:min-w-96 transition-transform"
			>
				<h1 className="text-3xl text-center mb-5">Resend Verification</h1>
				<p className="mb-5 text-gray-500">
					Enter your email address, we will send you reset link.
				</p>

				<CommonInputField
					type={"email"}
					icon={"person"}
					placeholder={"Email"}
					classProp={"py-3 mb-3"}
					name={"email"}
					onChange={(e) => setEmail(e.target.value)}
					value={email}
				/>
				 
				<Button
					onClick={'submit'}
					disabled={isLoading}
					className={"bg-black text-white rounded hover:bg-gray-700 w-full py-3 mt-5 flex items-center justify-center"}
					text={isLoading ? "Sending Verification Link..." : "Send Verification Link"}
					isLoading={isLoading}
				/>
			</form>
		</div>
	);
};

export default ResendVerification;