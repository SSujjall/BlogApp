import { useState, useEffect } from "react";
import { getBlogs } from "../services/blogService";
import { showSuccessToast, showErrorToast } from "../utils/toastHelper";
import Layout from "../components/layout/Layout";

const HomePage = () => {
  const [blogs, setBlogs] = useState([]);

  useEffect(() => {
    const fetchBlogs = async () => {
      try {
        const data = await getBlogs();
        setBlogs(data);
        showSuccessToast("Blogs fetched successfully");
      } catch  {
        showErrorToast("Error fetching blogs");
      }
    };

    fetchBlogs();
  }, []);

  return (
    <Layout>
      <h1 className="text-3xl font-semibold mb-4">Blogs</h1>
      {blogs.length === 0 ? (
        <p>No blogs available</p>
      ) : (
        <ul>
          {blogs.map((blog) => (
            <li key={blog.id} className="mb-4 p-4 border border-gray-200 rounded-lg">
              <h2 className="text-xl font-semibold">{blog.title}</h2>
              <p>{blog.description}</p>
            </li>
          ))}
        </ul>
      )}
    </Layout>
  );
};

export default HomePage;
