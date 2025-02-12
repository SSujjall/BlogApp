import { toast } from "react-toastify";

// Helper function to show success toast
export const showSuccessToast = (message) => {
  toast.success(message, {
    position: toast.POSITION.TOP_RIGHT,
    autoClose: 5000,
  });
};

// Helper function to show error toast
export const showErrorToast = (message) => {
  toast.error(message, {
    position: toast.POSITION.TOP_RIGHT,
    autoClose: 5000,
  });
};
