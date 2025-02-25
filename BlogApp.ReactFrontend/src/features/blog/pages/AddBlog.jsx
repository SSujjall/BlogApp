import { useState, useRef } from "react";
import Button from "../../../components/common/Button";
import CommonInputField from "../../../components/common/CommonInputField";
import Layout from "../../../components/layout/Layout";
import { XCircle } from "lucide-react";
import { showSuccessToast } from "../../../common/utils/toastHelper";
import { createBlog } from "../service/blogService";

const initFieldValues = {
  title: "",
  description: "",
  imageFile: null,
};

const AddBlog = () => {
  const [preview, setPreview] = useState(null);
  const fileInputRef = useRef(null);

  // request constants
  const [values, setValues] = useState(initFieldValues);

  const handleFieldChange = (e) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
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
  };

  const handleDeleteImage = () => {
    setValues({ ...values, imageFile: null });
    setPreview(null);

    // Reset file input
    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }
  };

  const onPostClick = async (e) => {
    e.preventDefault();
    try {
      const response = await createBlog(values);
      if (response) {
        showSuccessToast("Blog created successfully!");
      }
    } catch (error) {
      console.error("Error creating blog:", error);
    }
  };

  return (
    <Layout>
      <div className="mx-auto max-w-4xl border rounded-md p-4 shadow-sm">
        <h1 className="text-3xl font-bold mb-3">Create Post</h1>

        <CommonInputField
          name="title"
          placeholder="Title"
          classProp="py-3 mb-3"
          onChange={handleFieldChange}
          value={values.title}
        />

        <textarea
          name="description"
          className="px-4 py-3 w-full border rounded-md outline-none focus-within:border-black transition-colors"
          placeholder="Description"
          rows={10}
          onChange={handleFieldChange}
          value={values.description}
        />

        {/* File Upload Area */}
        <div
          className="mt-4 border-2 border-dashed border-gray-400 rounded-md text-center cursor-pointer hover:border-black transition-colors relative overflow-hidden"
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

        <div className="flex justify-end mt-4">
          <Button
            text="Post"
            className="bg-black text-white px-10"
            onClick={onPostClick}
          />
        </div>
      </div>
    </Layout>
  );
};

export default AddBlog;
