import { useEffect } from "react";
import {
  showSuccessToast,
  showErrorToast,
  closeAllToasts
} from "../../../common/utils/toastHelper";

const ResendVerification = () => {
    useEffect(() => {
        closeAllToasts();
    },[])

    return(<div>ResendVerification Page</div>);
};

export default ResendVerification;