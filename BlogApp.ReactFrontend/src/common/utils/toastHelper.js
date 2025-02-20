import { toast } from "react-toastify";

// Helper function to show success toast
export const showSuccessToast = (message) => {
  toast.success(message);
};

// Helper function to show error toast
export const showErrorToast = (message) => {
  toast.error(message);
};

export const showWarningToast = (message) => {
  toast.warning(message);
}
