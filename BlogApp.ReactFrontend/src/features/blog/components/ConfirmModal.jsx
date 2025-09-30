import PropTypes from "prop-types";

const ConfirmModal = ({
	isOpen,
	title = "Confirm Action",
	message = "Are you sure you want to proceed?",
	onConfirm,
	onCancel,
}) => {
	if (!isOpen) return null;

	return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div
        className="bg-white rounded-lg p-6 shadow-lg"
        style={{ width: "30vw", minWidth: "340px", maxWidth: "500px" }}
      >
        <h2 className="text-xl font-semibold mb-4">{title}</h2>
        <p className="mb-6">{message}</p>
        <div className="flex justify-end gap-3">
          <button
            className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 w-full"
            onClick={onCancel}
          >
            Cancel
          </button>
          <button
            className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600 w-full"
            onClick={onConfirm}
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}


ConfirmModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  title: PropTypes.string,
  message: PropTypes.string.isRequired,
  onConfirm: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
};

export default ConfirmModal;