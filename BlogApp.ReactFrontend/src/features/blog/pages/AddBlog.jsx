import { useState } from "react";
import Button from "../../../components/common/Button";
import CommonInputField from "../../../components/common/CommonInputField";
import Layout from "../../../components/layout/Layout";

const AddBlog = () => {
  const [file, setFile] = useState(null);

  const handleFileChange = (event) => {
    const selectedFile = event.target.files[0];
    if (selectedFile) {
      setFile(selectedFile);
    }
  };

  const handleDragOver = (event) => {
    event.preventDefault();
  };

  const handleDrop = (event) => {
    event.preventDefault();
    const droppedFile = event.dataTransfer.files[0];
    if (droppedFile) {
      setFile(droppedFile);
    }
  };

  return (
    <Layout>
      <div className="mx-auto max-w-4xl border rounded-md p-4">
        <h1 className="text-3xl font-bold mb-3">Create Post</h1>

        <CommonInputField placeholder={"Title"} classProp={"py-3 mb-3"} />

        <textarea
          className="px-4 py-3 w-full border rounded-md outline-none focus-within:border-black transition-colors"
          placeholder="Description"
          rows={10}
        />

        {/* File Upload Area */}
        <div
          className="mt-4 border-2 border-dashed border-gray-400 rounded-md text-center cursor-pointer hover:border-black transition-colors"
          onDragOver={handleDragOver}
          onDrop={handleDrop}
        >
          <input
            type="file"
            id="fileUpload"
            className="hidden"
            onChange={handleFileChange}
          />
          <label htmlFor="fileUpload" className="cursor-pointer">
            {file ? (
              <div className="text-sm text-gray-700 p-6">
                <p className="font-semibold">Selected file:</p>
                <p className="truncate">{file.name}</p>
              </div>
            ) : (
              <p className="text-gray-600 p-6">
                Drag & drop a file here or click to upload
              </p>
            )}
          </label>
        </div>

        <div className="flex justify-end mt-4">
          <Button text={"Post"} className={"bg-black text-white px-5"} />
        </div>
      </div>
    </Layout>
  );
};

export default AddBlog;
