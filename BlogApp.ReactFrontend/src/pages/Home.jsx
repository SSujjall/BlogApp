import { useState, useEffect } from "react";
import { getBlogs } from "../services/featureServices/blogService";
import { showSuccessToast, showErrorToast } from "../utils/toastHelper";
import Layout from "../components/layout/Layout";

const HomePage = () => {
  const [blogs, setBlogs] = useState([]);

  useEffect(() => {
    const fetchBlogs = async () => {
      try {
        const data = await getBlogs();
        setBlogs(data.data);
        showSuccessToast("Blogs fetched successfully");
      } catch {
        showErrorToast("Error fetching blogs");
      }
    };

    fetchBlogs();
  }, []);

  return (
    <Layout>
      <h1 className="text-3xl font-semibold mb-4">Blogs</h1>
      
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6">
        {blogs.map((blog) => (
          <div key={blog.blogId} className="border p-4 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold mb-2">{blog.title}</h2>
            <img
              src={blog.imageUrl}
              alt={blog.title}
              className="w-full h-64 object-cover mb-4 rounded-lg"
            />
            <p className="text-gray-600">{blog.description}</p>
          </div>
        ))}
      </div>
    </Layout>
  );
};

export default HomePage;
