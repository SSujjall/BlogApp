import { useParams } from "react-router-dom";
import { useState, useRef, useEffect } from "react";
import Layout from "../../../components/layout/Layout";
import CommonInputField from "../../../components/common/CommonInputField";
import { XCircle } from "lucide-react";
import Button from "../../../components/common/Button";
import { showErrorToast, showSuccessToast } from "../../../common/utils/toastHelper";
import { updateBlog, getBlogById } from "../service/blogService";

const initFieldValues = {
  title: "",
  description: "",
  imageFile: null,
};

const EditBlog = () => {
  const { blogId: paramBlogId } = useParams();
  const [preview, setPreview] = useState(null);
  const fileInputRef = useRef(null);
  const [isLoading, setIsLoading] = useState(false);

  //reequest constants
  const [values, setValues] = useState(initFieldValues);

  const [valid, setValid] = useState({
    title: true,
    description: true,
    imageFile: true,
  });

	useEffect(() => {
		const fetchBlogDetail = async () => {
			setIsLoading(true);
			try {
				const response = await getBlogById(paramBlogId);
				if (response) {
					setValues({
						title: response.data.title,
						description: response.data.description,
						imageFile: null, // Reset imageFile to null
					});
					setPreview(response.data.imageUrl); // Set preview URL
				}
			}catch {
				showErrorToast("Error fetching your blog details");
			}
			finally {
				setIsLoading(false);
			}
		};
		fetchBlogDetail();
	}, [paramBlogId]);

  const handleFieldChange = (e) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });

    // Remove red border once the user types
    if (name === "title" || name === "description") {
      setValid((prev) => ({
        ...prev,
        [name]: value.trim() !== "", // Check if field is non-empty
      }));
    }
  };

  const handleFileChange = (event) => {
    const selectedFile = event.target.files[0];
    if (selectedFile) {
      // Update both imageFile and preview together
      setValues({
        ...values,
        imageFile: selectedFile,
      });
      setPreview(URL.createObjectURL(selectedFile)); // Set preview URL
    }
    setValid((prev) => ({
      ...prev,
      imageFile: selectedFile !== null, // Ensure file is selected
    }));
  };

  const handleDragOver = (event) => {
    event.preventDefault();
  };

  const handleDrop = (event) => {
    event.preventDefault();
    const droppedFile = event.dataTransfer.files[0];
    if (droppedFile) {
      // Update both imageFile and preview together
      setValues({
        ...values,
        imageFile: droppedFile,
      });
      setPreview(URL.createObjectURL(droppedFile)); // Set preview URL
    }
    setValid((prev) => ({
      ...prev,
      imageFile: droppedFile !== null, // Ensure file is selected
    }));
  };

  const handleDeleteImage = () => {
    setValues({ ...values, imageFile: null });
    setPreview(null);

    // Reset file input
    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }
  };

  const onSaveClick = async (e) => {
    e.preventDefault();

    // const isTitleValid = values.title.trim() !== "";
    // const isDescriptionValid = values.description.trim() !== "";
    // const isImageValid = values.imageFile !== null;

    // setValid({
    //   title: isTitleValid,
    //   description: isDescriptionValid,
    //   imageFile: isImageValid,
    // });

    // if (isTitleValid && isDescriptionValid && isImageValid) {
      setIsLoading(true);
      try {
        const response = await updateBlog(paramBlogId, values);
        if (response) {
          showSuccessToast("Blog edited successfully!");
        }
      } catch (error) {
        console.error("Error editing blog:", error);
      } finally {
        setIsLoading(false);
      }
    // }
  };

  return (
    <Layout>
      <div className="mx-auto max-w-4xl border rounded-md p-4 shadow-sm">
        <h1 className="text-3xl font-bold mb-3">Edit Post</h1>

        <CommonInputField
          name="title"
          placeholder="Title"
          classProp={`py-3 mb-3 ${
            valid.title ? "" : "border-2 border-red-500"
          }`}
          onChange={handleFieldChange}
          value={values.title}
        />

        <textarea
          name="description"
          className={`px-4 py-3 w-full border rounded-md outline-none focus-within:border-black 
            transition-colors ${
              valid.description ? "" : "border-2 border-red-500"
            }`}
          placeholder="Description"
          rows={10}
          onChange={handleFieldChange}
          value={values.description}
        />

        {/* File Upload Area */}
        <div
          className={`mt-4 border-2 border-dashed ${
            valid.imageFile ? "border-gray-400" : "border-red-500"
          } 
            rounded-md text-center cursor-pointer hover:border-black transition-colors relative overflow-hidden`}
          onDragOver={handleDragOver}
          onDrop={handleDrop}
        >
          <input
            ref={fileInputRef}
            name="imageFile"
            type="file"
            id="fileUpload"
            className="hidden"
            onChange={handleFileChange}
          />

          {preview ? (
            <div className="relative w-full h-80 flex justify-center items-center pointer-events-none">
              <img
                src={preview}
                alt="Uploaded Preview"
                className="w-full h-full object-contain rounded-md"
              />
              <button
                onClick={(e) => {
                  e.stopPropagation(); // Prevent click from bubbling up
                  handleDeleteImage();
                }}
                className="absolute top-2 right-2 bg-black text-white p-1 rounded-full shadow-md hover:bg-gray-700 transition pointer-events-auto"
              >
                <XCircle size={24} />
              </button>
            </div>
          ) : (
            <label
              htmlFor="fileUpload"
              className="cursor-pointer block w-full h-full"
            >
              <p className="text-gray-600 p-6">
                Drag & drop a file here or click to upload
              </p>
            </label>
          )}
        </div>

        <div className="flex justify-end mt-4 items-center">
          {isLoading && (
            <div className="w-5 h-5 mr-2 border-4 border-t-4 border-gray-300 border-t-black rounded-full animate-spin"></div>
          )}
          <Button
            text="Save"
            className="bg-black text-white px-10"
            onClick={onSaveClick}
            disabled={isLoading}
          />
        </div>
      </div>
    </Layout>
  );
};

export default EditBlog;
